using AspNetCoreService.CoreModel;
using AspNetCoreService.DataAccess;

namespace AspNetCoreService.ContactsWebApi.UpdateContact
{
    public sealed class EfUpdateContactSession : EfSession, IUpdateContactSession
    {
        public EfUpdateContactSession(DatabaseContext context) : base(context) { }

        public void UpdateContact(Contact contact) => Context.Update(contact);
    }
}