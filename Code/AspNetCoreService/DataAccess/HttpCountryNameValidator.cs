using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AspNetCoreService.CoreModel;

namespace AspNetCoreService.DataAccess
{
    public sealed class HttpCountryNameValidator : ICountryNameValidator
    {
        public async Task<bool> CheckIfCountryNameIsValidAsync(string countryName, CancellationToken cancellationToken)
        {
            using var httpClient = new HttpClient();
            var encodedCountryName = WebUtility.UrlEncode(countryName);
            var url = $"https://restcountries.eu/rest/v2/name/{encodedCountryName}?fullText=true";
            var response = await httpClient.GetAsync(url, cancellationToken);
            return response.StatusCode == HttpStatusCode.OK;
        }
    }
}