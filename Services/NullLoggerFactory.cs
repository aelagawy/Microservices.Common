using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservices.Common.Services
{
    using Microsoft.Extensions.Logging;
    using System;

    public class NullLoggerFactory : ILoggerFactory
    {
        public void AddProvider(ILoggerProvider provider) { }

        public ILogger CreateLogger(string categoryName) => new NullLogger();

        public void Dispose() { }

        private class NullLogger : ILogger
        {
            public IDisposable BeginScope<TState>(TState state) => NullScope.Instance;

            public bool IsEnabled(LogLevel logLevel) => false;

            public void Log<TState>(
                LogLevel logLevel,
                EventId eventId,
                TState state,
                Exception exception,
                Func<TState, Exception, string> formatter)
            { }

            private class NullScope : IDisposable
            {
                public static NullScope Instance { get; } = new NullScope();
                public void Dispose() { }
            }
        }
    }

}
