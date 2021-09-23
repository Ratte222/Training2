using Microsoft.Extensions.DependencyInjection;
using MyLoggerLibraryUI.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyLoggerLibraryUI
{
    public class MyLoggerUiOptionsBuilder: IMyLoggerUiOptionsBuilder
    {
        private readonly IServiceCollection _services;

        IServiceCollection IMyLoggerUiOptionsBuilder.Services => _services;
        public MyLoggerUiOptionsBuilder(IServiceCollection services)
        {
            _services = services;
        }

    }
}
