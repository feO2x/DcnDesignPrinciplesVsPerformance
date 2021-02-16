using System;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;

namespace AspNetCoreService.CoreModel
{
    public abstract class BaseContactValidator<T> : AbstractValidator<T>
        where T : class, IContactProperties
    {
        protected BaseContactValidator(ICountryNameValidator countryNameValidator, ILogger logger)
        {
            RuleFor(contact => contact.FirstName).MinimumLength(2);
            RuleFor(contact => contact.LastName).MinimumLength(2);
            RuleFor(contact => contact.DateOfBirth).GreaterThan(new DateTime(1900, 1, 1)).LessThan(DateTime.Today);
            RuleFor(contact => contact.Address).MinimumLength(10);
            RuleFor(contact => contact.EmailAddress).EmailAddress();
            RuleFor(contact => contact.CountryOfOrigin).ValidateCountryNameAsync(countryNameValidator, logger);
        }

        protected override bool PreValidate(ValidationContext<T> context, ValidationResult result)
        {
            if (context.InstanceToValidate == null)
            {
                result.Errors.Add(new ValidationFailure(string.Empty, "You cannot pass null in the request body."));
                return false;
            }

            context.InstanceToValidate.TrimStringProperties();
            return true;
        }
    }
}