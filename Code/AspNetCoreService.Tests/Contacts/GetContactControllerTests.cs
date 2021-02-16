using System;
using System.Reflection;
using System.Threading.Tasks;
using AspNetCoreService.ContactsWebApi.GetContact;
using AspNetCoreService.CoreModel;
using AspNetCoreService.Tests.TestHelpers;
using FluentAssertions;
using Light.GuardClauses;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace AspNetCoreService.Tests.Contacts
{
    public sealed class GetContactControllerTests : WebApiControllerTests<GetContactController>
    {
        public GetContactControllerTests() : base("api/contacts")
        {
            Session = new GetContactSessionMock();
            SessionFactory = new FactorySpy<GetContactSessionMock>(Session);
            Controller = new GetContactController(SessionFactory.GetInstance);
        }

        private GetContactController Controller { get; }

        private GetContactSessionMock Session { get; }

        private FactorySpy<GetContactSessionMock> SessionFactory { get; }

        [Fact]
        public static void GetContactMustBeHttpGet()
        {
            var httpGetAttribute = ControllerType.GetMethod(nameof(GetContactController.GetContact))?.GetCustomAttribute<HttpGetAttribute>();
            httpGetAttribute.MustNotBeNull().Template.Should().Be("{id}");
        }

        [Fact]
        public async Task GetExistingContact()
        {
            var result = await Controller.GetContact(42);

            result.Value.Should().BeEquivalentTo(Session.Contact);
            Session.MustHaveBeenDisposed();
        }

        [Fact]
        public async Task ContactNotFound()
        {
            var result = await Controller.GetContact(17);

            result.Result.Should().BeOfType<NotFoundResult>();
            Session.MustHaveBeenDisposed();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-18939)]
        public async Task InvalidId(int invalidId)
        {
            var result = await Controller.GetContact(invalidId);

            var expectedResult = Controller.ValidationProblem();
            result.Result.MustBeEquivalentToValidationProblem(expectedResult);
            SessionFactory.InstanceMustNotHaveBeenCreated();
        }

        private sealed class GetContactSessionMock : BaseReadOnlySessionMock<GetContactSessionMock>, IGetContactSession
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

            public ValueTask<Contact?> GetContactAsync(int id) =>
                id == 42 ? new(Contact) : new ValueTask<Contact?>((Contact?) null);
        }
    }
}