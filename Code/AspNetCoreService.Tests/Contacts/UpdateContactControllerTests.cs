using System;
using System.Threading.Tasks;
using AspNetCoreService.ContactsWebApi.UpdateContact;
using AspNetCoreService.CoreModel;
using AspNetCoreService.Tests.CoreModel;
using AspNetCoreService.Tests.TestHelpers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace AspNetCoreService.Tests.Contacts
{
    public sealed class UpdateContactControllerTests : WebApiControllerTests<UpdateContactController>
    {
        public UpdateContactControllerTests(ITestOutputHelper output) : base("api/contacts/update")
        {
            var loggerFactory = output.CreateLoggerFactory();
            var validator = new ContactValidator(new CountryNameValidatorStub(), loggerFactory.CreateLogger<ContactValidator>());
            Session = new UpdateContactSessionMock();
            SessionFactory = new FactorySpy<UpdateContactSessionMock>(Session);
            Controller = new UpdateContactController(validator, SessionFactory.GetInstance, loggerFactory.CreateLogger<UpdateContactController>());
        }

        private UpdateContactController Controller { get; }

        private UpdateContactSessionMock Session { get; }

        private FactorySpy<UpdateContactSessionMock> SessionFactory { get; }

        [Fact]
        public static void UpdateContactMustBeHttpPut() =>
            ControllerType.GetMethod(nameof(UpdateContactController.UpdateContact)).Should().BeDecoratedWith<HttpPutAttribute>();

        [Fact]
        public async Task UpdateValidContact()
        {
            var contact = CreateContact();

            var result = await Controller.UpdateContact(contact);

            result.Should().BeOfType<NoContentResult>();
            SessionMustBeSavedAndDisposed(contact);
        }

        [Fact]
        public async Task ContactNotPresent()
        {
            var contact = CreateContact();
            Session.ThrowDbUpdateConcurrencyException = true;

            var result = await Controller.UpdateContact(contact
);

            result.Should().BeOfType<NotFoundResult>();
            SessionMustBeSavedAndDisposed(contact
);
        }

        private void SessionMustBeSavedAndDisposed(Contact contact)
        {
            Session.CapturedContact.Should().BeSameAs(contact);
            Session.SaveChangesMustHaveBeenCalled()
                   .MustHaveBeenDisposed();
        }

        [Fact]
        public async Task InvalidContact()
        {
            var invalidContact = CreateContact(0);

            var result = await Controller.UpdateContact(invalidContact);

            var expectedResult = Controller.ValidationProblem();
            result.MustBeEquivalentToValidationProblem(expectedResult);
            SessionFactory.InstanceMustNotHaveBeenCreated();
        }

        private static Contact CreateContact(int id = 42) =>
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
        

        private sealed class UpdateContactSessionMock : BaseSessionMock<UpdateContactSessionMock>, IUpdateContactSession
        {
            public Contact? CapturedContact { get; private set; }

            public bool ThrowDbUpdateConcurrencyException { get; set; }

            public void UpdateContact(Contact contact) => CapturedContact = contact;

            public override Task SaveChangesAsync()
            {
                base.SaveChangesAsync();
                if (ThrowDbUpdateConcurrencyException)
                    throw new DbUpdateConcurrencyException();
                return Task.CompletedTask;
            }
        }
    }
}