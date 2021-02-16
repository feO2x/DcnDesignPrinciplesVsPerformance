using System.Threading.Tasks;
using AspNetCoreService.CoreModel;
using AspNetCoreService.DataAccess;

namespace AspNetCoreService.ContactsWebApi.DeleteContact
{
    public sealed class EfDeleteContactSession : EfSession, IDeleteContactSession
    {
        public EfDeleteContactSession(DatabaseContext context) : base(context) { }
        public ValueTask<Contact?> GetContactAsync(int id) => Context.FindAsync<Contact?>(id);
        public void DeleteContact(Contact contact) => Context.Contacts.Remove(contact);
    }
}