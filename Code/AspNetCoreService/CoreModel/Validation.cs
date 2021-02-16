using System;
using FluentValidation;
using Light.GuardClauses;
using Microsoft.Extensions.Logging;

namespace AspNetCoreService.CoreModel
{
    public static class Validation
    {
        // ReSharper disable ConstantConditionalAccessQualifier -- as TContact instances usually are crossing the process boundary (JSON, EF), we explicitly disable NRT support here
#nullable disable
        public static TContact TrimStringProperties<TContact>(this TContact contact)
            where TContact : class, IContactProperties
        {
            contact.MustNotBeNullReference(nameof(contact));

            contact.FirstName = contact.FirstName?.Trim();
            contact.LastName = contact.LastName?.Trim();
            contact.Address = contact.Address?.Trim();
            contact.CountryOfOrigin = contact.CountryOfOrigin?.Trim();
            contact.EmailAddress = contact.EmailAddress?.Trim();

            return contact;
        }
#nullable restore
        // ReSharper restore ConstantConditionalAccessQualifier

        public static IRuleBuilder<TEntity, string> ValidateCountryNameAsync<TEntity>(this IRuleBuilderInitial<TEntity, string> ruleBuilder,
                                                                                      ICountryNameValidator countryNameValidator,
                                                                                      ILogger logger) =>
            ruleBuilder.CustomAsync(async (countryName, context, cancellationToken) =>
            {
                try
                {
                    var result = await countryNameValidator.CheckIfCountryNameIsValidAsync(countryName, cancellationToken);
                    if (!result)
                        context.AddFailure($"The country \"{countryName}\" does not exist.");
                }
                catch (Exception exception)
                {
                    logger.LogError(exception, "Error while checking country name {CountryName}", countryName);
                    context.AddFailure($"The country \"{countryName}\" could not be validated due to a network error.");
                }
            });
    }
}