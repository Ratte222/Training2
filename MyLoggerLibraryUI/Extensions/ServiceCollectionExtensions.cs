using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyLoggerLibraryUI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMyLoggerUi(this IServiceCollection services, Action<MyLoggerUiOptionsBuilder> optionsBuilder)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (optionsBuilder == null)
                throw new ArgumentNullException(nameof(optionsBuilder));

            var builder = new MyLoggerUiOptionsBuilder(services);
            optionsBuilder.Invoke(builder);

            return services;
        }
    }
}
