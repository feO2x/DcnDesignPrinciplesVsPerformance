using System;
using System.Threading.Tasks;
using AspNetCoreService.CoreModel;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreService.ContactsWebApi.GetContact
{
    [ApiController]
    [Route("api/contacts")]
    public sealed class GetContactController : ControllerBase
    {
        public GetContactController(Func<IGetContactSession> createSession) => CreateSession = createSession;

        private Func<IGetContactSession> CreateSession { get; }

        [HttpGet("{id}")]
        public async Task<ActionResult<Contact>> GetContact(int id)
        {
            if (id < 1)
            {
                ModelState.AddModelError("id", "The id must be positive.");
                return ValidationProblem();
            }

            await using var session = CreateSession();
            var contact = await session.GetContactAsync(id);
            if (contact == null)
                return NotFound();
            return contact;
        }
    }
}