using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreService.Infrastructure
{
    public static class Validation
    {
        public static bool CheckForErrors<T>(this ControllerBase controller,
                                             T value,
                                             IValidator<T> validator,
                                             [NotNullWhen(true)] out ActionResult? badRequestResult)
        {
            var validationResult = validator.Validate(value);
            if (validationResult.IsValid)
            {
                badRequestResult = null;
                return false;
            }

            validationResult.AddToModelState(controller.ModelState, null);
            badRequestResult = controller.ValidationProblem();
            return true;
        }

        public static async Task<ActionResult?> CheckForErrorsAsync<T>(this ControllerBase controller,
                                                                       T value,
                                                                       IValidator<T> validator)
        {
            var validationResult = await validator.ValidateAsync(value);
            if (validationResult.IsValid)
                return null;

            validationResult.AddToModelState(controller.ModelState, null);
            return controller.ValidationProblem();
        }
    }
}