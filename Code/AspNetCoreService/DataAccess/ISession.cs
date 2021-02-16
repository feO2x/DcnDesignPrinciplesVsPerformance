using System;
using System.Threading.Tasks;

namespace AspNetCoreService.DataAccess
{
    public interface ISession : IAsyncDisposable
    {
        Task SaveChangesAsync();
    }
}