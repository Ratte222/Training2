using System;
using System.Collections.Generic;
using System.Text;

namespace MyLoggerLibrary.LoggerUIMiddleware
{
    public class LoggerMidlleware
    {
        public async Task Invoke(HttpContext httpContext)
        {
            var httpMethod = httpContext.Request.Method;
            var path = httpContext.Request.Path.Value;

            // If the RoutePrefix is requested (with or without trailing slash), redirect to index URL
            if (httpMethod == "GET" && Regex.IsMatch(path, $"^/{Regex.Escape(_options.RoutePrefix)}/api/logs/?$", RegexOptions.IgnoreCase))
            {
                try
                {
                    httpContext.Response.ContentType = "application/json;charset=utf-8";
                    if (!CanAccess(httpContext))
                    {
                        httpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                        return;
                    }

                    var result = await FetchLogsAsync(httpContext);
                    httpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                    await httpContext.Response.WriteAsync(result);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                    var errorMessage = httpContext.Request.IsLocal()
                        ? JsonConvert.SerializeObject(new { errorMessage = ex.Message })
                        : JsonConvert.SerializeObject(new { errorMessage = "Internal server error" });

                    await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(new { errorMessage }));
                }

                return;
            }

            if (httpMethod == "GET" && Regex.IsMatch(path, $"^/?{Regex.Escape(_options.RoutePrefix)}/?$", RegexOptions.IgnoreCase))
            {
                var indexUrl = httpContext.Request.GetEncodedUrl().TrimEnd('/') + "/index.html";
                RespondWithRedirect(httpContext.Response, indexUrl);
                return;
            }

            if (httpMethod == "GET" && Regex.IsMatch(path, $"^/{Regex.Escape(_options.RoutePrefix)}/?index.html$", RegexOptions.IgnoreCase))
            {
                await RespondWithIndexHtml(httpContext.Response);
                return;
            }

            await _staticFileMiddleware.Invoke(httpContext);
        }
    }
}
