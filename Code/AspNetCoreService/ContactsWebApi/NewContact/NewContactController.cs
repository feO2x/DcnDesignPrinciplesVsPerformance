using System;
using System.Threading.Tasks;
using AspNetCoreService.CoreModel;
using AspNetCoreService.Infrastructure;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AspNetCoreService.ContactsWebApi.NewContact
{
    [ApiController]
    [Route("api/contacts/new")]
    public sealed class NewContactController : ControllerBase
    {
        public NewContactController(NewContactDtoValidator validator,
                                      Func<INewContactSession> createSession,
                                      IMapper mapper,
                                      ILogger<NewContactController> logger)
        {
            Validator = validator;
            CreateSession = createSession;
            Mapper = mapper;
            Logger = logger;
        }

        private NewContactDtoValidator Validator { get; }
        private Func<INewContactSession> CreateSession { get; }
        private IMapper Mapper { get; }
        private ILogger<NewContactController> Logger { get; }

        [HttpPost]
        public async Task<IActionResult> CreateNewContact([FromBody] NewContactDto newContactDto)
        {
            var badRequestResult = await this.CheckForErrorsAsync(newContactDto, Validator);
            if (badRequestResult != null)
                return badRequestResult;

            var contact = Mapper.Map<NewContactDto, Contact>(newContactDto);
            await using var session = CreateSession();
            session.AddContact(contact);
            await session.SaveChangesAsync();
            Logger.LogInformation("A new contact {@Contact} was created", contact);
            return Created("/api/contacts/" + contact.Id, contact);
        }
    }
}