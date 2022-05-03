using System;

namespace StrategyLogger
{
    public class ConsoleLogger : ILogger
    {
        public void LogError(string message)
        {
            // балуемся с цветом текста в консоли, так как
            // логгирование должно быть красивым
            Console.ForegroundColor = ConsoleColor.Red;
            // выводим сообщение с типом события и текущей датой
            Console.Write($"{DateTime.Now} | ERROR: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message);
        }

        public void LogInfo(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"{DateTime.Now} | INFO: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message);
        }
    }
}