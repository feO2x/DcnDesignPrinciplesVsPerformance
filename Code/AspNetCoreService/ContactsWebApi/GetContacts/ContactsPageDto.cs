using System.Collections.Generic;
using AspNetCoreService.CoreModel;

namespace AspNetCoreService.ContactsWebApi.GetContacts
{
    public sealed class ContactsPageDto
    {
        public ContactsPageDto(int totalNumberOfContacts, List<Contact> contacts)
        {
            TotalNumberOfContacts = totalNumberOfContacts;
            Contacts = contacts;
        }

        public int TotalNumberOfContacts { get; }

        public List<Contact> Contacts { get; }
    }
}