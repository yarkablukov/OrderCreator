using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCreator
{
    public interface IReporter
    {
        void Report (string message);
    }
}
