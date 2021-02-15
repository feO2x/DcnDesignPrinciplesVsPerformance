using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Light.GuardClauses;
using Newtonsoft.Json;

namespace SyncVsAsync.WpfClient
{
    public sealed class WebApiPerformanceManager
    {
        private readonly HttpClient _httpClient;
        private readonly Stopwatch _stopWatch = new();

        public WebApiPerformanceManager(HttpClient httpClient)
        {
            _httpClient = httpClient.MustNotBeNull(nameof(httpClient));
        }

        public async Task<WebApiPerformanceResults> MeasureApiCallsAsync(bool isCallingAsynchronousApi, int numberOfCalls, int waitIntervalInMilliseconds)
        {
            numberOfCalls.MustBeGreaterThan(0, nameof(numberOfCalls));

            var targetUrl = isCallingAsynchronousApi ? "http://localhost:5000/api/asynchronous" : "http://localhost:5000/api/synchronous";
            targetUrl = targetUrl + "?waitIntervalInMilliseconds=" + waitIntervalInMilliseconds;

            _stopWatch.Restart();
            var tasks = new Task<bool>[numberOfCalls];
            for (var i = 0; i < numberOfCalls; i++)
            {
                tasks[i] = CallApiAsync(targetUrl);
            }

            await Task.WhenAll(tasks);
            _stopWatch.Stop();

            var response = await _httpClient.GetAsync("http://localhost:5000/api/threadingResults");
            var threadingResults = JsonConvert.DeserializeObject<WebApiThreadingResults>(await response.Content.ReadAsStringAsync());

            return new WebApiPerformanceResults(_stopWatch.Elapsed, tasks.Count(t => t.Result), tasks.Count(t => t.Result == false), threadingResults);
        }

        private async Task<bool> CallApiAsync(string targetUrl)
        {
            try
            {
                // All calls are started directly from the UI thread. ConfigureAwait(false) tells
                // the asynchronous state machine that is generated for this method to run the
                // continuation of this method on the Thread Pool instead of the UI thread.
                // ConfigureAwait(true) is the default and pushes the continuation on the calling
                // thread if (and only if) it had a SynchronizationContext associated with it.
                var response = await _httpClient.GetAsync(targetUrl).ConfigureAwait(false);
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}