using System;
using System.Threading.Tasks;
using AspNetCoreService.CoreModel;
using AspNetCoreService.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AspNetCoreService.ContactsWebApi.UpdateContact
{
    [ApiController]
    [Route("api/contacts/update")]
    public sealed class UpdateContactController : ControllerBase
    {
        public UpdateContactController(ContactValidator validator,
                                         Func<IUpdateContactSession> createSession,
                                         ILogger<UpdateContactController> logger)
        {
            Validator = validator;
            CreateSession = createSession;
            Logger = logger;
        }

        private ContactValidator Validator { get; }
        private Func<IUpdateContactSession> CreateSession { get; }
        private ILogger<UpdateContactController> Logger { get; }

        [HttpPut]
        public async Task<IActionResult> UpdateContact([FromBody] Contact contact)
        {
            var badRequestResult = await this.CheckForErrorsAsync(contact, Validator);
            if (badRequestResult != null)
                return badRequestResult;

            await using var session = CreateSession();
            session.UpdateContact(contact);
            try
            {
                await session.SaveChangesAsync();
                Logger.LogInformation("Contact {@Contact} was updated successfully", contact);
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }
        }
    }
}