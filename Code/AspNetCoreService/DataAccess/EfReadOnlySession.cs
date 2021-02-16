using System;
using System.Threading.Tasks;

namespace AspNetCoreService.DataAccess
{
    public abstract class EfReadOnlySession : IAsyncDisposable
    {
        protected EfReadOnlySession(DatabaseContext context) => Context = context;

        protected DatabaseContext Context { get; }

        public ValueTask DisposeAsync() => Context.DisposeAsync();
    }
}