using System.Collections.Generic;
using Faker;
using Light.GuardClauses;

namespace AspNetCoreService.CoreModel
{
    public static class ContactFactory
    {
        public static Range<int> NumberOfContactsRange { get; } =
            Range.FromInclusive(1).ToInclusive(50_000);

        public static List<Contact> GenerateFakeData(int numberOfContacts = 100)
        {
            numberOfContacts.MustBeIn(NumberOfContactsRange, nameof(numberOfContacts));

            var contacts = new List<Contact>(numberOfContacts);
            for (var i = 0; i < numberOfContacts; i++)
            {
                var firstName = Name.First();
                var lastName = Name.Last();
                var fullName = $"{firstName} {lastName}";
                var contact = new Contact
                {
                    Id = i + 1,
                    FirstName = firstName,
                    LastName = lastName,
                    DateOfBirth = DateOfBirth.CreateRandom(),
                    Address = $"{Address.StreetAddress()}, {Address.ZipCode()} {Address.Country()}",
                    EmailAddress = Internet.Email(fullName),
                    CountryOfOrigin = Country.Name(),
                };
                contacts.Add(contact);
            }

            return contacts;
        }
    }
}