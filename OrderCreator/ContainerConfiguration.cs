using System;
using System.Collections.Generic;
using System.Text;
using Unity;

namespace OrderCreator
{
    class ContainerConfiguration
    {
        public static UnityContainer Configure()
        {
            var container = new UnityContainer();
            container.RegisterSingleton<IReporter, Reporter>();
            return container;
        }
    }
}
