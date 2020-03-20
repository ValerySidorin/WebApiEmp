using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiEmp
{
    public class FileLoggerProvider : ILoggerProvider
    {
        private string filepath;
        public FileLoggerProvider(string path)
        {
            filepath = path;
        }
        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(filepath);
        }

        public void Dispose()
        {
            
        }
    }
}
