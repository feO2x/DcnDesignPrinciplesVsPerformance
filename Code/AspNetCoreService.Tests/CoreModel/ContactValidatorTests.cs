using System;
using System.Threading.Tasks;
using AspNetCoreService.CoreModel;
using AspNetCoreService.Tests.TestHelpers;
using FluentAssertions;
using FluentValidation.TestHelper;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace AspNetCoreService.Tests.CoreModel
{
    public sealed class ContactValidatorTests
    {
        public ContactValidatorTests(ITestOutputHelper output)
        {
            var logger = output.CreateLoggerFactory().CreateLogger<ContactValidator>();
            Validator = new ContactValidator(new CountryNameValidatorStub(), logger);
        }

        private ContactValidator Validator { get; }

        [Fact]
        public static void ContactValidatorMustDeriveFromBaseContactValidator() =>
            typeof(ContactValidator).Should().BeDerivedFrom<BaseContactValidator<Contact>>();

        [Theory]
        [InlineData(1)]
        [InlineData(25)]
        [InlineData(240192)]
        public async Task ValidId(int id)
        {
            var contact = CreateContact(id);

            var result = await Validator.TestValidateAsync(contact);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-845)]
        public async Task InvalidId(int invalidId)
        {
            var contact = CreateContact(invalidId);

            var result = await Validator.TestValidateAsync(contact);

            result.ShouldHaveValidationErrorFor(a => a.Id);
        }

        private static Contact CreateContact(int id) =>
            new()
            {
                Id = id,
                FirstName = "John",
                LastName = "Doe",
                Address = "135 Fantasy Road, Michigan",
                CountryOfOrigin = "USA",
                DateOfBirth = new DateTime(1979, 8, 18),
                EmailAddress = "john.doe@gmail.com"
            };
    }
}