using System.Threading;
using System.Threading.Tasks;

namespace AspNetCoreService.CoreModel
{
    public interface ICountryNameValidator
    {
        Task<bool> CheckIfCountryNameIsValidAsync(string countryName, CancellationToken cancellationToken);
    }
}