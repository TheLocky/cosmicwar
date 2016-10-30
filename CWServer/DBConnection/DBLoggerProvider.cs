using System;
using System.IO;
using System.Web;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CWServer.DBConnection {
    public class DBLoggerProvider : ILoggerProvider {
        public ILogger CreateLogger(string categoryName) {
            return new DBLogger(categoryName);
        }

        public void Dispose() {}

        public static void AddToContext(DbContext context) {
            var serviceProvider = context.GetInfrastructure();
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            loggerFactory.AddProvider(new DBLoggerProvider());
        }

        public class DBLogger : ILogger {
            private readonly string _category;
            private readonly string _logFile;

            public DBLogger(string category) {
                _category = category;
                var dir = Path.Combine(HttpRuntime.AppDomainAppPath, "Logs");
                Directory.CreateDirectory(dir);
                _logFile = Path.Combine(dir, $"{_category}.txt");
                if (File.Exists(_logFile)) File.Delete(_logFile);
            }

            public void Log(LogLevel logLevel, int eventId, object state,
                Exception exception, Func<object, Exception, string> formatter) {
                var logString = formatter(state, exception) + "\n";
                File.AppendAllText(_logFile, logString);
                Console.WriteLine($"[{_category}] {logString}");
            }

            public bool IsEnabled(LogLevel logLevel) {
                return true;
            }

            public IDisposable BeginScopeImpl(object state) {
                return null;
            }
        }
    }
}
