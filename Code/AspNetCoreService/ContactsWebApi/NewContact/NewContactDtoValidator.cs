using AspNetCoreService.CoreModel;
using Microsoft.Extensions.Logging;

namespace AspNetCoreService.ContactsWebApi.NewContact
{
    public sealed class NewContactDtoValidator : BaseContactValidator<NewContactDto>
    {
        public NewContactDtoValidator(ICountryNameValidator countryNameValidator, ILogger<NewContactDtoValidator> logger)
            : base(countryNameValidator, logger) { }
    }
}