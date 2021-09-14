using BLL.Interfaces;
using BLL.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using BenchmarkDotNet.Running;
using ProjectForBenchmark.Benchmark;

namespace ProjectForBenchmark
{
    //https://benchmarkdotnet.org/articles/samples/IntroConfigSource.html
    class Program
    {
        static void Main(string[] args)
        {
            ServiceProvider services = null;
            Startup startup = new Startup();
            startup.ConfigureService(ref services);
            var scope = services.CreateScope();
            //IAnnouncementService announcementService = scope.ServiceProvider.GetRequiredService<IAnnouncementService>();
            //announcementService.GetAll();
            //BenchmarkRunner.Run<TestBency>();
            BenchmarkRunner.Run<AnnouncementBenchy>();
            Console.WriteLine("Hello World!");
        }
    }
}
