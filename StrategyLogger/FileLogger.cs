using System;
using System.IO;

namespace StrategyLogger
{
    public class FileLogger : ILogger
    {
        private const string FileName = "log.txt";
        public void LogError(string message)
        {
            // открываем поток к файлу, чтобы добавить в него
            // сообщение о событии. Директива using сама его потом закроет.
            using (var stream = new StreamWriter(FileName, true))
            {
                var text = $"{DateTime.Now} | ERROR: {message}";
                stream.WriteLine(text);
            }
        }

        public void LogInfo(string message)
        {
            using (var stream = new StreamWriter(FileName, true))
            {
                var text = $"{DateTime.Now} | INFO: {message}";
                stream.WriteLine(text);
            }
        }
    }
}