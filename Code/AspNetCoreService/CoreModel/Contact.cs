using System;

namespace AspNetCoreService.CoreModel
{
    public sealed class Contact : Entity<Contact>, IContactProperties
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        public string Address { get; set; } = string.Empty;

        public string CountryOfOrigin { get; set; } = string.Empty;

        public string EmailAddress { get; set; } = string.Empty;
    }
}