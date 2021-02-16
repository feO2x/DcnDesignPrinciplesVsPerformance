using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AspNetCoreService.ContactsWebApi.DeleteContact
{
    [ApiController]
    [Route("api/contacts/delete")]
    public sealed class DeleteContactController : ControllerBase
    {
        public DeleteContactController(Func<IDeleteContactSession> createSession,
                                       ILogger<DeleteContactController> logger)
        {
            CreateSession = createSession;
            Logger = logger;
        }

        private Func<IDeleteContactSession> CreateSession { get; }
        private ILogger<DeleteContactController> Logger { get; }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(int id)
        {
            if (id < 1)
            {
                ModelState.AddModelError(nameof(id), "id must be a positive number");
                return ValidationProblem();
            }

            await using var session = CreateSession();
            var contact = await session.GetContactAsync(id);
            if (contact == null)
                return NotFound();

            session.DeleteContact(contact);
            try
            {
                await session.SaveChangesAsync();
                Logger.LogInformation("Contact {@Contact} was delete successfully", contact);
            }
            catch (DbUpdateConcurrencyException exception)
            {
                Logger.LogWarning(exception, "Could not delete contact {@Contact}, likely because a parallel operation deleted the entity at the same time.", contact);
            }

            return NoContent();
        }
    }
}