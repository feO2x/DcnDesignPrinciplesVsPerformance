using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SyncVsAsync.AspNetCoreService
{
    [ApiController]
    [Route("api")]
    public sealed class SyncVsAsyncController : ControllerBase
    {
        [HttpGet("synchronous")]
        public ActionResult GetSynchronous(int waitIntervalInMilliseconds)
        {
            if (CheckIfWaitIntervalIsErroneous(waitIntervalInMilliseconds))
                return ValidationProblem();

            Thread.Sleep(waitIntervalInMilliseconds);
            ThreadPoolWatcher.Instance.UpdateUsedThreads();
            return Ok("Hi from synchronous method");
        }

        [HttpGet("asynchronous")]
        public async Task<ActionResult> GetAsynchronous(int waitIntervalInMilliseconds)
        {
            if (CheckIfWaitIntervalIsErroneous(waitIntervalInMilliseconds))
                return ValidationProblem();

            await Task.Delay(waitIntervalInMilliseconds);
            ThreadPoolWatcher.Instance.UpdateUsedThreads();
            return Ok("Hi from asynchronous method");
        }

        [HttpGet("threadingResults")]
        public ActionResult Get()
        {
            var threadPoolWatcher = ThreadPoolWatcher.Instance;
            var threadingResults =
                new
                {
                    threadPoolWatcher.MaximumWorkerThreads,
                    threadPoolWatcher.MaximumCompletionPortThreads,
                    threadPoolWatcher.UsedWorkerThreads,
                    threadPoolWatcher.UsedCompletionPortThreads
                };

            threadPoolWatcher.Reset();

            return Ok(threadingResults);
        }

        private bool CheckIfWaitIntervalIsErroneous(int waitIntervalInMilliseconds)
        {
            if (waitIntervalInMilliseconds >= 10)
                return false;

            ModelState.AddModelError(nameof(waitIntervalInMilliseconds), $"{nameof(waitIntervalInMilliseconds)} must be at least 10.");
            return true;
        }
    }
}