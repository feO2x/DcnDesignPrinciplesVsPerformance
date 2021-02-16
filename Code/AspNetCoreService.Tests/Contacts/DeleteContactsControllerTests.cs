using System;
using System.Reflection;
using System.Threading.Tasks;
using AspNetCoreService.ContactsWebApi.DeleteContact;
using AspNetCoreService.CoreModel;
using AspNetCoreService.Tests.TestHelpers;
using FluentAssertions;
using Light.GuardClauses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace AspNetCoreService.Tests.Contacts
{
    public sealed class DeleteContactsControllerTests : WebApiControllerTests<DeleteContactController>
    {
        public DeleteContactsControllerTests(ITestOutputHelper output) : base("api/contacts/delete")
        {
            var loggerFactory = output.CreateLoggerFactory();
            Session = new DeleteContactSessionMock();
            SessionFactory = new FactorySpy<DeleteContactSessionMock>(Session);
            Controller = new DeleteContactController(SessionFactory.GetInstance, loggerFactory.CreateLogger<DeleteContactController>());
        }

        private DeleteContactController Controller { get; }
        private DeleteContactSessionMock Session { get; }
        private FactorySpy<DeleteContactSessionMock> SessionFactory { get; }

        [Fact]
        public static void DeleteContactMustBeHttpDelete()
        {
            var httpDeleteAttribute = ControllerType.GetMethod(nameof(DeleteContactController.DeleteContact))?.GetCustomAttribute<HttpDeleteAttribute>();
            httpDeleteAttribute.MustNotBeNull().Template.Should().Be("{id}");
        }

        [Fact]
        public async Task DeleteExistingContact()
        {
            var result = await Controller.DeleteContact(42);

            result.Should().BeOfType<NoContentResult>();
            VerifyContactWasDeleted();
        }

        [Fact]
        public async Task DeleteNonExistingContact()
        {
            var result = await Controller.DeleteContact(109);

            result.Should().BeOfType<NotFoundResult>();
            Session.CapturedContact.Should().BeNull();
            Session.SaveChangesMustNotHaveBeenCalled()
                   .MustHaveBeenDisposed();
        }

        [Fact]
        public async Task RaceConditionOnSaveChanges()
        {
            Session.ThrowDbUpdateConcurrencyException = true;

            var result = await Controller.DeleteContact(42);

            result.Should().BeOfType<NoContentResult>();
            VerifyContactWasDeleted();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-62183912)]
        public async Task InvalidId(int invalidId)
        {
            var result = await Controller.DeleteContact(invalidId);

            var expectedResult = Controller.ValidationProblem();
            result.MustBeEquivalentToValidationProblem(expectedResult);
            SessionFactory.InstanceMustNotHaveBeenCreated();
        }

        private void VerifyContactWasDeleted()
        {
            Session.CapturedContact.Should().BeSameAs(Session.Contact);
            Session.SaveChangesMustHaveBeenCalled()
                   .MustHaveBeenDisposed();
        }

        private sealed class DeleteContactSessionMock : BaseSessionMock<DeleteContactSessionMock>, IDeleteContactSession
        {
            public Contact Contact { get; } =
                new()
                {
                    Id = 42,
                    FirstName = "John",
                    LastName = "Doe",
                    Address = "135 Fantasy Road, Michigan",
                    CountryOfOrigin = "USA",
                    DateOfBirth = new DateTime(1979, 8, 18),
                    EmailAddress = "john.doe@gmail.com"
                };

            public Contact? CapturedContact { get; private set; }

            public bool ThrowDbUpdateConcurrencyException { get; set; }

            public ValueTask<Contact?> GetContactAsync(int id) =>
                id == 42 ? new ValueTask<Contact?>(Contact) : new ValueTask<Contact?>((Contact?) null);

            public void DeleteContact(Contact contact) => CapturedContact = contact;

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