using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreService.CoreModel;
using AspNetCoreService.DataAccess;
using Light.GuardClauses;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreService.ContactsWebApi.GetContacts
{
    public sealed class EfGetContactsSession : EfReadOnlySession, IGetContactsSession
    {
        public EfGetContactsSession(DatabaseContext context) : base(context) { }

        public Task<int> GetTotalNumberOfContactsAsync(string? searchTerm) =>
            CreateBaseQuery(searchTerm).CountAsync();

        public Task<List<Contact>> GetContactsAsync(int skip, int take, string? searchTerm) =>
            CreateBaseQuery(searchTerm).OrderBy(contact => contact.LastName)
                                       .Skip(skip)
                                       .Take(take)
                                       .ToListAsync();

        private IQueryable<Contact> CreateBaseQuery(string? searchTerm)
        {
            IQueryable<Contact> query = Context.Contacts;
            if (!searchTerm.IsNullOrWhiteSpace())
                query = ApplyFilter(query, searchTerm);
            return query;
        }

        private static IQueryable<Contact> ApplyFilter(IQueryable<Contact> query, string searchTerm) =>
            query.Where(contact => contact.FirstName.StartsWith(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                   contact.LastName.StartsWith(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                   contact.CountryOfOrigin.StartsWith(searchTerm, StringComparison.OrdinalIgnoreCase));
    }
}