using System;
using System.Threading.Tasks;
using AspNetCoreService.CoreModel;

namespace AspNetCoreService.ContactsWebApi.GetContact
{
    public interface IGetContactSession : IAsyncDisposable
    {
        ValueTask<Contact?> GetContactAsync(int id);
    }
}