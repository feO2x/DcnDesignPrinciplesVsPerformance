using AspNetCoreService.CoreModel;
using Light.GuardClauses;
using Light.GuardClauses.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCoreService.DataAccess
{
    public static class DataAccessModule
    {
        public static IServiceCollection AddDataAccessModule(this IServiceCollection services, IConfiguration configuration)
        {
            var numberOfContacts = configuration.GetValue("numberOfContacts", 100);
            var allowedRange = ContactFactory.NumberOfContactsRange;
            if (numberOfContacts.IsNotIn(allowedRange))
                throw new InvalidConfigurationException($"{nameof(numberOfContacts)} must be between {allowedRange.From} and {allowedRange.To}, but you provided {numberOfContacts}. Please adjust appsettings.json.");

            var options = new DbContextOptionsBuilder<DatabaseContext>().UseInMemoryDatabase("Contacts")
                                                                        .Options;
            var contacts = ContactFactory.GenerateFakeData(numberOfContacts);
            using var context = new DatabaseContext(options);
            context.Contacts.AddRange(contacts);
            context.SaveChanges();

            return services.AddTransient<DatabaseContext>()
                           .AddSingleton<DbContextOptions>(options);
        }
    }
}