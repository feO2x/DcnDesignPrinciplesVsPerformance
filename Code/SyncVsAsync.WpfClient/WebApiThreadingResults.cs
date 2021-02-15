using Light.GuardClauses;

namespace SyncVsAsync.WpfClient
{
    public sealed class WebApiThreadingResults
    {
        public WebApiThreadingResults(int maximumCompletionPortThreads, int maximumWorkerThreads, int usedWorkerThreads, int usedCompletionPortThreads)
        {
            MaximumCompletionPortThreads = maximumCompletionPortThreads.MustNotBeLessThan(0, nameof(maximumCompletionPortThreads));
            MaximumWorkerThreads = maximumWorkerThreads.MustNotBeLessThan(0, nameof(maximumWorkerThreads));
            UsedWorkerThreads = usedWorkerThreads.MustNotBeLessThan(0, nameof(usedWorkerThreads));
            UsedCompletionPortThreads = usedCompletionPortThreads.MustNotBeLessThan(0, nameof(usedCompletionPortThreads));
        }

        public int MaximumCompletionPortThreads { get; }
        public int MaximumWorkerThreads { get; }
        public int UsedWorkerThreads { get; }
        public int UsedCompletionPortThreads { get; }
    }
}