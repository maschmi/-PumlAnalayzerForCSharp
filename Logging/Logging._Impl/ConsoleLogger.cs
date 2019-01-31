using System;

namespace Logger
{
    public class ConsoleLogger : IDoLog
    {
        public void Debug(string logmessage)
        {
            Console.WriteLine("[DEBUG]\t" + logmessage);
        }

        public void Error(string logmessage)
        {
            Console.WriteLine("[ERROR]\t" + logmessage);
        }

        public void Info(string logmessage)
        {
            Console.WriteLine("[INFO]\t" + logmessage);
        }

        public void Warning(string logmessage)
        {
            Console.WriteLine("[WARN]\t" + logmessage);
        }

        public void WriteLine(string logmessage)
        {
            Console.WriteLine("[----]\t" + logmessage);
        }
    }
}