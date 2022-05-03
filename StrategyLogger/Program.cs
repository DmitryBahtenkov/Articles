using System;

namespace StrategyLogger
{
    class Program
    {
        static void Main(string[] args)
        {
            Calculator calculator;
            Console.WriteLine("Введите 1 для консольного логгирования нажмите Enter для использования файлового логгера");
            
            var input = Console.ReadLine();
            
            // инициализируем наш необходимой реализацией логгера
            if (input == "1")
                calculator = new Calculator(new ConsoleLogger());
            else
                calculator = new Calculator(new FileLogger());
            
            Console.WriteLine("Введите первое число: ");
            var first = int.Parse(Console.ReadLine());
            
            Console.WriteLine("Введите второе число: ");
            var second = int.Parse(Console.ReadLine());

            Console.WriteLine("Введите действие: ");
            var action = Console.ReadLine();
            
            // проверяем введённое действие
            switch (action)
            {
                case "+":
                    Console.WriteLine("Result: " + calculator.CalculateSum(first, second));
                    break;
                case "-":
                    Console.WriteLine("Result: " + calculator.CalculateSubtract(first, second));
                    break;
                case "*":
                    Console.WriteLine("Result: " + calculator.CalculateMultiple(first, second));
                    break;
                case "/":
                    Console.WriteLine("Result: " + calculator.CalculateDivide(first, second));
                    break;
                default:
                    Console.WriteLine("Некорректное действие!");
                    break;
            }
        }
    }
}