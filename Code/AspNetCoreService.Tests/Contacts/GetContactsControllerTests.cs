using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreService.ContactsWebApi.GetContacts;
using AspNetCoreService.CoreModel;
using AspNetCoreService.Paging;
using AspNetCoreService.Tests.TestHelpers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace AspNetCoreService.Tests.Contacts
{
    public sealed class GetContactsControllerTests : WebApiControllerTests<GetContactsController>
    {
        public GetContactsControllerTests() : base("api/contacts") { }

        [Fact]
        public static void GetContactsMustBeDecoratedWithHttpGetAttribute() =>
            ControllerType.GetMethod(nameof(GetContactsController.GetContacts)).Should().BeDecoratedWith<HttpGetAttribute>();

        [Theory]
        [InlineData(0, 20)]
        [InlineData(20, 30)]
        [InlineData(90, 25)]
        [InlineData(110, 25)]
        public static async Task ReturnContactsOnValidRequest(int skip, int take)
        {
            var session = new GetContactsSessionMock(100);
            var controller = new GetContactsController(new PageDtoValidator(), () => session);

            var actionResult = await controller.GetContacts(new PageDto { Skip = skip, Take = take });

            var expectedContacts = session.Contacts.Skip(skip).Take(take).ToList();
            var expectedResult = new ContactsPageDto(100, expectedContacts);
            actionResult.Value.Should().BeEquivalentTo(expectedResult);
            session.MustHaveBeenDisposed();
        }

        private sealed class GetContactsSessionMock : BaseReadOnlySessionMock<GetContactsSessionMock>, IGetContactsSession
        {
            public GetContactsSessionMock(int numberOfContacts)
            {
                Contacts = ContactFactory.GenerateFakeData(numberOfContacts);
            }

            public List<Contact> Contacts { get; }

            public Task<int> GetTotalNumberOfContactsAsync(string? searchTerm) =>
                Task.FromResult(Contacts.Count);

            public Task<List<Contact>> GetContactsAsync(int skip, int take, string? searchTerm) =>
                Task.FromResult(Contacts.Skip(skip).Take(take).ToList());
        }
    }
}