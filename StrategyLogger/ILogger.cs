using System;

namespace StrategyLogger
{
    public interface ILogger
    {
        public void LogError(string message);
        public void LogInfo(string message);
    }
}