using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyLoggerLibraryUI.Interface
{
    public interface IMyLoggerUiOptionsBuilder
    {
        IServiceCollection Services { get; }
    }
}
