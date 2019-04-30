using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;

namespace ConsoleSample
{
    internal static class Program
    {
        private static void Main()
        {
            var logger = LogManager.GetCurrentClassLogger();

            var servicesProvider = BuildDi();
            var runner = servicesProvider.GetRequiredService<Runner>();
            runner.DoAction("Action1");

            logger.Debug("outer context test");
            Console.WriteLine("Press ANY key to exit");
            Console.ReadKey();
            LogManager.Shutdown();
        }

        private static IServiceProvider BuildDi()
        {
            return new ServiceCollection()
                .AddSingleton<Runner>() // Runner is the custom class
                .AddLogging(loggingBuilder =>
                {
                    // configure Logging with NLog
                    loggingBuilder.ClearProviders();
                    loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                    loggingBuilder.AddNLog(new NLogProviderOptions {ParseMessageTemplates = true});
                })
                .BuildServiceProvider();
        }
    }

    public class Runner
    {
        private readonly ILogger<Runner> _logger;

        public Runner(ILogger<Runner> logger)
        {
            _logger = logger;
        }

        public void DoAction(string name)
        {
            try
            {
                string a = null;
                a.ToString();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "custom message");
            }

            _logger.LogWarning("test information log");
        }
    }
}