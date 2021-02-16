using System;
using System.Threading;
using System.Threading.Tasks;
using AspNetCoreService.CoreModel;

namespace AspNetCoreService.Tests.CoreModel
{
    public sealed class CountryNameValidatorStub : ICountryNameValidator
    {
        public bool IsValidCountry { get; set; } = true;

        public bool ThrowException { get; set; }

        public Task<bool> CheckIfCountryNameIsValidAsync(string countryName, CancellationToken cancellationToken)
        {
            if (ThrowException)
                throw new Exception("An exception occurred while validating the country");
            return Task.FromResult(IsValidCountry);
        }
    }
}