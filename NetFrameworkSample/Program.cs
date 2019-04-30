using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace NetFrameworkSample
{
    internal class Program
    {
        private static void Main()
        {
            var logger = LogManager.GetCurrentClassLogger();

            var servicesProvider = BuildDi();
            var runner = servicesProvider.GetRequiredService<Runner>();
            runner.DoAction("Action1");

            logger.Fatal("outer context test");
            Console.WriteLine("Press ANY key to exit");
            Console.ReadKey();
            LogManager.Shutdown();
        }

        private static IServiceProvider BuildDi()
        {
            return new ServiceCollection()
                .AddSingleton<Runner>()
                .AddLogging(loggingBuilder =>
                {
                    loggingBuilder.ClearProviders();
                    loggingBuilder.SetMinimumLevel(LogLevel.Trace);
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