using System;
using System.IO;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Xml.Schema;
using System.Configuration;
using NLog;
using Unity;

namespace OrderCreator
{
    class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        static void Main(string[] args)
        {
            // Папка, в которой будут лежать заказы клиентов на загрузку
            //string customerOrdersDirectory = ConfigurationManager.AppSettings["customer_orders_directory"];
            // Папка, в которую будут выгружаться ордера на доставку
            string deliveryOrdersDirectory = ConfigurationManager.AppSettings["delivery_orders_directory"];
            // Путь до схемы валидации заказа клиента
            string xsdSchemaPath = ConfigurationManager.AppSettings["xsdSchemaPath"];

            var container = ContainerConfiguration.Configure();
            

            //запуск OrderProcessor для загрузки заказов клиентов и формирования ордеров на доставку
            var statisticsDocuments = new OrderProcessor(
                new DirectoryInfo(customerOrdersDirectory),
                xsdSchemaPath,
                deliveryOrdersDirectory,
                //new Reporter()
                container.Resolve<IReporter>()
                ).Process();
                       
            // вывод разных вариантов отчетов по заказам клиентов
            bool isActive = true;
            while (isActive)
            {
                try
                {
                    StatisticsMonitor.ShowStatistics(statisticsDocuments, out isActive, container.Resolve<IReporter>());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            if (isActive == false)
                {
                    Environment.Exit(0);
                }
                
            }
        }
    }
}
