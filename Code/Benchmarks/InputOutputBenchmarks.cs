using System.Linq;
using BenchmarkDotNet.Attributes;
using Benchmarks.EfCoreDataAccess;
using Microsoft.EntityFrameworkCore;
using Raven.Client.Documents;
using Employee = Benchmarks.DataAccess.Employee;

namespace Benchmarks
{
    [InvocationCount(1024, 16)]
    public class InputOutputBenchmarks
    {
        public DbContextOptions<DatabaseContext>? DbContextOptions;
        public IDocumentStore? RavenDbStore;

        [GlobalSetup(Target = nameof(LoadEmployeeFromRavenDb))]
        public void SetupRavenDbConnection()
        {
            RavenDbStore = new DocumentStore
                {
                    Urls = new[] { "http://localhost:10001" },
                    Database = "AdventureWorks"
                }
               .Initialize();
        }

        [GlobalSetup(Target = nameof(LoadPersonFromMsSqlViaEfCore))]
        public void SetupEntityFrameworkContext()
        {
            DbContextOptions =
                new DbContextOptionsBuilder<DatabaseContext>()
                   .UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=AdventureWorks2016;Integrated Security=True")
                   .Options;
        }

        [Benchmark(Baseline = true)]
        public Employee? LoadEmployeeFromRavenDb()
        {
            using var session = RavenDbStore!.OpenSession();
            return session.Load<Employee>("employees/9-A");
        }

        [Benchmark]
        public Person? LoadPersonFromMsSqlViaEfCore()
        {
            using var context = new DatabaseContext(DbContextOptions);
            return context.People
                          .Include(p => p.BusinessEntity.BusinessEntityAddresses)
                          .Include(p => p.PersonPhones)
                          .FirstOrDefault(p => p.BusinessEntityId == 1663);
        }
    }
}