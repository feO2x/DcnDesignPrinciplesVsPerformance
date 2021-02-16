using AspNetCoreService.CoreModel;
using AspNetCoreService.DataAccess;

namespace AspNetCoreService.ContactsWebApi.NewContact
{
    public sealed class EfNewContactSession : EfSession, INewContactSession
    {
        public EfNewContactSession(DatabaseContext context) : base(context) { }

        public void AddContact(Contact contact) => Context.Contacts.Add(contact);
    }
}