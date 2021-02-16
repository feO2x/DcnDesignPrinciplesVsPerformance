using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspNetCoreService.CoreModel;

namespace AspNetCoreService.ContactsWebApi.GetContacts
{
    public interface IGetContactsSession : IAsyncDisposable
    {
        Task<int> GetTotalNumberOfContactsAsync(string? searchTerm);
        Task<List<Contact>> GetContactsAsync(int skip, int take, string? searchTerm);
    }
}