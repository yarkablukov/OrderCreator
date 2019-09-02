using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCreator
{
    public class Reporter : IReporter
    {
        public void Report(string message)
        {
            Console.WriteLine(message);
        }
    }
}
