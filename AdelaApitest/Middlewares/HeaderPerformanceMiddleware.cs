using System.Diagnostics;

namespace RestApiExample.Middlewares
{
    /// <summary>
    /// Adds processing time to every request as header under key 'performanceMilliseconds'.
    /// </summary>
    public class HeaderPerformanceMiddleware
    {
        private readonly RequestDelegate _next;

        public HeaderPerformanceMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            Stopwatch sw = Stopwatch.StartNew();
            context.Response.OnStarting(() =>
            {
                sw.Stop();
                context.Response.Headers.Add("performanceMilliseconds", sw.ElapsedMilliseconds.ToString());
                return Task.CompletedTask;
            });

            await _next(context);
        }
    }
}
