using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspNetCoreService.CoreModel;
using AspNetCoreService.Infrastructure;
using AspNetCoreService.Paging;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreService.ContactsWebApi.GetContacts
{
    [ApiController]
    [Route("api/contacts")]
    public sealed class GetContactsController : ControllerBase
    {
        public GetContactsController(PageDtoValidator validator, Func<IGetContactsSession> createSession)
        {
            Validator = validator;
            CreateSession = createSession;
        }

        private PageDtoValidator Validator { get; }

        private Func<IGetContactsSession> CreateSession { get; }


        [HttpGet]
        public async Task<ActionResult<ContactsPageDto>> GetContacts([FromQuery] PageDto pageDto)
        {
            if (this.CheckForErrors(pageDto, Validator, out var badRequestResult))
                return badRequestResult;

            await using var session = CreateSession();
            var totalNumberOfContacts = await session.GetTotalNumberOfContactsAsync(pageDto.SearchTerm);
            List<Contact> contacts;
            if (totalNumberOfContacts > pageDto.Skip)
                contacts = await session.GetContactsAsync(pageDto.Skip, pageDto.Take, pageDto.SearchTerm);
            else
                contacts = new List<Contact>(0);
            return new ContactsPageDto(totalNumberOfContacts, contacts);
        }
    }
}