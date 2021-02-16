using System.Threading.Tasks;
using AspNetCoreService.DataAccess;
using FluentAssertions;

namespace AspNetCoreService.Tests.TestHelpers
{
    public abstract class BaseSessionMock<T> : BaseReadOnlySessionMock<T>, ISession
        where T : BaseSessionMock<T>
    {
        public int SaveChangesAsyncCallCount { get; private set; }

        public virtual Task SaveChangesAsync()
        {
            SaveChangesAsyncCallCount++;
            return Task.CompletedTask;
        }

        public T SaveChangesMustHaveBeenCalled()
        {
            SaveChangesAsyncCallCount.Should().Be(1);
            return (T) this;
        }

        public T SaveChangesMustNotHaveBeenCalled()
        {
            SaveChangesAsyncCallCount.Should().Be(0);
            return (T) this;
        }
    }
}