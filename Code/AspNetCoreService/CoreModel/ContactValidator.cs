using FluentValidation;
using Microsoft.Extensions.Logging;

namespace AspNetCoreService.CoreModel
{
    public sealed class ContactValidator : BaseContactValidator<Contact>
    {
        public ContactValidator(ICountryNameValidator countryNameValidator, ILogger<ContactValidator> logger) : base(countryNameValidator, logger)
        {
            RuleFor(contact => contact.Id).GreaterThan(0);
        }
    }
}