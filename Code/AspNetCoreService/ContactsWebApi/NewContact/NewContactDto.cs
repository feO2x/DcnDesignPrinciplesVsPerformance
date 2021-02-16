using System;
using AspNetCoreService.CoreModel;

namespace AspNetCoreService.ContactsWebApi.NewContact
{
    public sealed class NewContactDto : IContactProperties
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; } = string.Empty;
        public string CountryOfOrigin { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
    }
}