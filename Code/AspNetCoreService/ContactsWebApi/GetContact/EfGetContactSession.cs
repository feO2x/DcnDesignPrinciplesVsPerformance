using System.Threading.Tasks;
using AspNetCoreService.CoreModel;
using AspNetCoreService.DataAccess;

namespace AspNetCoreService.ContactsWebApi.GetContact
{
    public sealed class EfGetContactSession : EfReadOnlySession, IGetContactSession
    {
        public EfGetContactSession(DatabaseContext context) : base(context) { }

        public ValueTask<Contact?> GetContactAsync(int id) =>
#nullable disable
            Context.Contacts.FindAsync(id);
#nullable restore
    }
}