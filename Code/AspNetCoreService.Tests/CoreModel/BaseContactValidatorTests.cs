using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AspNetCoreService.CoreModel;
using AspNetCoreService.Tests.TestHelpers;
using FluentValidation.TestHelper;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace AspNetCoreService.Tests.CoreModel
{
    public sealed class BaseContactValidatorTests
    {
        public BaseContactValidatorTests(ITestOutputHelper output)
        {
            var logger = output.CreateLoggerFactory().CreateLogger<ContactValidatorDummy>();
            CountryNameValidator = new CountryNameValidatorStub();
            Validator = new ContactValidatorDummy(CountryNameValidator, logger);
        }

        private ContactValidatorDummy Validator { get; }

        private CountryNameValidatorStub CountryNameValidator { get; }

        [Fact]
        public async Task ValidContact()
        {
            var contact = CreateContact();

            var result = await Validator.TestValidateAsync(contact);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData("")]
        [InlineData("B")]
        [InlineData("\t\tt")]
        public Task InvalidFirstName(string invalidFirstName)
        {
            var contact = CreateContact();
            contact.FirstName = invalidFirstName;

            return CheckValidationErrorAsync(contact, c => c.FirstName);
        }

        [Theory]
        [InlineData("")]
        [InlineData("F")]
        [InlineData("T       ")]
        public Task InvalidLastName(string invalidLastName)
        {
            var contact = CreateContact();
            contact.LastName = invalidLastName;

            return CheckValidationErrorAsync(contact, c => c.LastName);
        }

        [Theory]
        [MemberData(nameof(InvalidDateOfBirthData))]
        public Task InvalidDateOfBirth(DateTime invalidDateOfBirth)
        {
            var contact = CreateContact();
            contact.DateOfBirth = invalidDateOfBirth;

            return CheckValidationErrorAsync(contact, c => c.DateOfBirth);
        }

        [Theory]
        [InlineData("Too short")]
        [InlineData("AtLeast10")]
        [InlineData("")]
        [InlineData("          NotEnough\t")]
        public Task InvalidAddress(string invalidAddress)
        {
            var contact = CreateContact();
            contact.Address = invalidAddress;

            return CheckValidationErrorAsync(contact, c => c.Address);
        }

        [Theory]
        [InlineData("this is not an email address")]
        [InlineData("missing.domain@")]
        [InlineData("@just-domain.com")]
        [InlineData("where.is.the.add.sign.eu")]
        [InlineData("")]
        [InlineData("  extra-padding  ")]
        public Task InvalidEmailAddress(string invalidEmailAddress)
        {
            var contact = CreateContact();
            contact.EmailAddress = invalidEmailAddress;

            return CheckValidationErrorAsync(contact, c => c.EmailAddress);
        }

        [Fact]
        public Task InvalidCountryName()
        {
            var contact = CreateContact();
            contact.CountryOfOrigin = "Invalid Country Name";
            CountryNameValidator.IsValidCountry = false;

            return CheckValidationErrorAsync(contact, c => c.CountryOfOrigin);
        }

        [Fact]
        public Task ErrorDuringCountryValidation()
        {
            var contact = CreateContact();
            contact.CountryOfOrigin = "Germany";
            CountryNameValidator.ThrowException = true;

            return CheckValidationErrorAsync(contact, c => c.CountryOfOrigin);
        }

        public static readonly TheoryData<DateTime> InvalidDateOfBirthData =
            new()
            {
                new DateTime(1899, 12, 31),
                DateTime.Today.AddDays(1)
            };

        private async Task CheckValidationErrorAsync<TProperty>(Contact contact,
                                                                Expression<Func<Contact, TProperty>> memberAccessor)
        {
            var result = await Validator.TestValidateAsync(contact);
            result.ShouldHaveValidationErrorFor(memberAccessor);
        }

        private static Contact CreateContact() =>
            new()
            {
                FirstName = "John",
                LastName = "Doe",
                Address = "135 Fantasy Road, Michigan",
                CountryOfOrigin = "USA",
                DateOfBirth = new DateTime(1979, 8, 18),
                EmailAddress = "john.doe@gmail.com"
            };

        private sealed class ContactValidatorDummy : BaseContactValidator<Contact>
        {
            public ContactValidatorDummy(ICountryNameValidator countryNameValidator, ILogger logger) : base(countryNameValidator, logger) { }
        }
    }
}