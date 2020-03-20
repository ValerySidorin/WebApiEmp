using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiEmp
{
    public class FileLogger : ILogger
    {
        private string filepath;

        public FileLogger(string path)
        {
            filepath = path;
        }
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (formatter != null)
            {
                File.AppendAllText(filepath, formatter(state, exception) + Environment.NewLine);
            }
        }
    }
}
