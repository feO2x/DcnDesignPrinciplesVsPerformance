using System;
using System.Text;
using Light.GuardClauses;

namespace SyncVsAsync.WpfClient
{
    public sealed class WebApiPerformanceResults
    {
        public WebApiPerformanceResults(TimeSpan elapsedTime,
                                        int successfulCalls,
                                        int erroneousCalls,
                                        WebApiThreadingResults threadingResults)
        {
            ElapsedTime = elapsedTime.MustNotBeLessThanOrEqualTo(TimeSpan.Zero, nameof(elapsedTime));
            SuccessfulCalls = successfulCalls.MustNotBeLessThan(0, nameof(successfulCalls));
            ErroneousCalls = erroneousCalls.MustNotBeLessThan(0, nameof(erroneousCalls));
            ThreadingResults = threadingResults.MustNotBeNull(nameof(threadingResults));
        }

        public TimeSpan ElapsedTime { get; }
        public int SuccessfulCalls { get; }
        public int ErroneousCalls { get; }
        public WebApiThreadingResults ThreadingResults { get; }

        public override string ToString()
        {
            return new StringBuilder().AppendLine($"Performed {SuccessfulCalls + ErroneousCalls} API calls in {ElapsedTime.TotalSeconds:N2} seconds, {ErroneousCalls} of them being erroneous.")
                                      .AppendLine($"On the Web API Service, out of a maximum of {ThreadingResults.MaximumWorkerThreads} worker threads, {ThreadingResults.UsedWorkerThreads} were used concurrently.")
                                      .AppendLine($"Out of a maximum of {ThreadingResults.MaximumCompletionPortThreads} IO completion threads, {ThreadingResults.UsedCompletionPortThreads} were used concurrently.")
                                      .ToString();
        }
    }
}