using AspNetCoreService.CoreModel;
using AspNetCoreService.DataAccess;

namespace AspNetCoreService.ContactsWebApi.NewContact
{
    public interface INewContactSession : ISession
    {
        void AddContact(Contact contact);
    }
}