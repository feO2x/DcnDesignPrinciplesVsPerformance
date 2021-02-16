using System.Threading.Tasks;
using AspNetCoreService.CoreModel;
using AspNetCoreService.DataAccess;

namespace AspNetCoreService.ContactsWebApi.DeleteContact
{
    public interface IDeleteContactSession : ISession
    {
        ValueTask<Contact?> GetContactAsync(int id);

        void DeleteContact(Contact contact);
    }
}