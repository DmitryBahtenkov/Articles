using System.Text;
using System.IO;
using System.Collections.Generic;
namespace FactoryMethod.Classic.LogParser
{
    public class LogEntry
    {
        public string Text { get;set; }
    }
    public abstract class LogReaderBase
    {
        public IEnumerable<LogEntry> Read()
        {
            using (var stream = GetStream())
            {
                using(var reader = new StreamReader(stream))
                {
                    string line = null;
                    while((line = reader.ReadLine()) != null)
                    {
                        // логика по обработке данных
                        yield return new LogEntry{ Text = line };
                    }
                }
            }
        }

        // фабричный метод
        protected abstract Stream GetStream();
    }

    public class FileLogReader : LogReaderBase
    {
        protected override Stream GetStream()
        {
            return File.Open("log.txt", FileMode.Open);
        }
    }

    public class HttpLogReader : LogReaderBase
    {
        protected override Stream GetStream()
        {
            // используем MemoryStream для имитации http-запроса
            var stream = new MemoryStream();
            var data = "LOG ERROR\nLOG INFO";
            var bytes = Encoding.Default.GetBytes(data);
            stream.Write(bytes, 0, bytes.Length);
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }
    }
}