using System;

namespace StrategyLogger
{
    public class Calculator
    {
        private readonly ILogger _logger;
        
        public Calculator(ILogger logger)
        {
            _logger = logger;
        }

        public int CalculateSum(int a, int b)
        {
            _logger.LogInfo($"{a} + {b} = {a+b}");
            return a + b;
        }

        public int CalculateSubtract(int a, int b)
        {
            _logger.LogInfo($"{a} - {b} = {a-b}");
            return a - b;
        }

        public int CalculateMultiple(int a, int b)
        {
            _logger.LogInfo($"{a} * {b} = {a*b}");
            return a * b;
        }
        
        public int CalculateDivide(int a, int b)
        {
            if (b == 0)
            {
                _logger.LogError($"Divide {a} by zero!!!");
                return 0;
            }
            
            _logger.LogInfo($"{a} / {b} = {a/b}");
            return a / b;
        }
    }
}