using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;
using MyLoggerLibraryUI.Middleware;

namespace MyLoggerLibraryUI.Extensions
{
    public static class MyLoggerOptionBuilderExtensions
    {
        public static IApplicationBuilder UseMyLoggerUI(this IApplicationBuilder applicationBuilder)
        {
            if (applicationBuilder == null)
                throw new ArgumentNullException(nameof(applicationBuilder));
            return applicationBuilder.UseMiddleware<MyLoggerUiMidlleware>();
        }
    }
}
