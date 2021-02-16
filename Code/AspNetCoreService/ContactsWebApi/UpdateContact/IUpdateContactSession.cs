using AspNetCoreService.CoreModel;
using AspNetCoreService.DataAccess;

namespace AspNetCoreService.ContactsWebApi.UpdateContact
{
    public interface IUpdateContactSession : ISession
    {
        void UpdateContact(Contact contact);
    }
}