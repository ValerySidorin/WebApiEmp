using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiEmp.Services
{
    public static class LoggerExtensions
    {
        public static ILoggerFactory AddFile(this ILoggerFactory loggerFactory, string filename)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), filename);
            loggerFactory.AddProvider(new FileLoggerProvider(path));
            return loggerFactory;
        }
    }
}
