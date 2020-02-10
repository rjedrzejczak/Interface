using System;
using System.IO;

namespace Interface
{
    class Program
    {
        static ILogger GetLogger()
        {
            //return new FileLogger(@"D:\SomeLog\test.txt");
            return new ConsoleLogger()
            {
                LogLevel = LogLevel.Debug
            };
        }

        static void Main(string[] args)
        {
            var logger = GetLogger();
            logger.LogMessage("Super sayan!!");
            Console.WriteLine("Hello World!");
            Console.ReadLine();
        }
    }

    public interface ILogger
    {
        event Action<string, LogLevel> OnLogMessage;
        LogLevel LogLevel { get; set; }
        void LogMessage(string message, LogLevel logLevel = LogLevel.Verbose);
    }

    public enum LogLevel
    {
        Verbose,
        Debug,
        Critical
        
    }

    public class ConsoleLogger : ILogger
    {
        public event Action<string, LogLevel> OnLogMessage = (message, level) => { };
        public LogLevel LogLevel { get; set; }

        public void LogMessage(string message, LogLevel logLevel)
        {
            if (logLevel > LogLevel)
            {
                 Console.WriteLine($"'{message}'");
            }
        }
    }

    public class FileLogger : ILogger
    {
        private readonly string _mLogPath;
        
        public FileLogger(string logPath)
        {
            _mLogPath = logPath;
            var directory = Path.GetDirectoryName(_mLogPath);
            Directory.CreateDirectory(directory);
        }

        public event Action<string, LogLevel> OnLogMessage = (message, level) => { };
        public LogLevel LogLevel { get; set; }

        public void LogMessage(string message, LogLevel logLevel)
        {
            if (logLevel > LogLevel)
            {
                using (var fileStream = new StreamWriter(File.OpenWrite(_mLogPath)))
                {
                    //Move to the end of the file
                    fileStream.BaseStream.Seek(0, SeekOrigin.End);
                    fileStream.WriteLine(message);
                }
            }
        }
    }
}