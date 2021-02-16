using System.Threading.Tasks;

namespace AspNetCoreService.DataAccess
{
    public abstract class EfSession : EfReadOnlySession, ISession
    {
        protected EfSession(DatabaseContext context) : base(context) { }

        public Task SaveChangesAsync() => Context.SaveChangesAsync();
    }
}