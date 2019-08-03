using System;
using System.IO;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Xml.Schema;

namespace OrderCreator
{
    class Program
    {
        //test commit
        static void Main(string[] args)
        {
            // Папка, в которой будут лежать заказы клиентов на загрузку
            string customerOrdersDirectory =
                @"C:\Users\Slava\source\repos\OrderCreator\OrderCreator\bin\Debug\netcoreapp2.1\customer_orders";
            // Папка, в которую будут выгружаться ордера на доставку
            string deliveryOrdersDirectory
                = @"C:\Users\Slava\source\repos\OrderCreator\OrderCreator\bin\Debug\netcoreapp2.1\delivery_orders";
            // Путь до схемы валидации заказа клиента
            string xsdSchemaPath = @"C:\Users\Slava\source\repos\OrderCreator\CutomerOrderXMLSchema.xsd";
            

            List<XDocument> statisticsDocuments = new List<XDocument>();
            DirectoryInfo customerOrdersDI = new DirectoryInfo(customerOrdersDirectory);

            // проверка на наличие заказов клиентов
            if (customerOrdersDI.GetFiles().Length == 0)
            {
                Console.WriteLine("В каталоге {0} не найдено ни одного заказа клиента", customerOrdersDirectory);
                Environment.Exit(0);
            }

            // проход по всем заказам клиентов, формирование ордеров на выгрузку
            int i = 1;
            foreach (var fi in customerOrdersDI.GetFiles())
            {
                // загрузка заказа
                Console.WriteLine(fi.Directory + @"\" + fi.Name);
                XDocument xDoc_in = XDocument.Load
                    (fi.Directory + @"\" + fi.Name);

                //валидация заказа по xsd-схеме
                XmlSchemaSet schema = new XmlSchemaSet();
                schema.Add("", xsdSchemaPath);
                xDoc_in.Validate(schema, ValidationEventHandler);

                // обработка заказа, вывод данных о заказе в консоль, логирование данных о загрузке
                CustomerOrderLoader customerOrderLoader = new CustomerOrderLoader();
                CustomerOrder customerOrder = customerOrderLoader.LoadCustomerOrder(xDoc_in);
                customerOrderLoader.Log(fi.Name);

                // создание ордера на отгрузку из загруженного заказа, сохрание документа, логирование данных
                DeliveryOrderUnloader deliveryOrderUnloader = new DeliveryOrderUnloader();
                XDocument xDoc_out = deliveryOrderUnloader.CreateDeliveryOrders(customerOrder);
                xDoc_out.Save(deliveryOrdersDirectory + @"\" + "delivery_order_" + i + ".xml");
                deliveryOrderUnloader.Log("delivery_order_" + i + ".xml");
                i++;

                // добавление документа в коллекцию для дальнейшего использования в отчетах
                statisticsDocuments.Add(xDoc_in);
            }

            // вывод разных вариантов отчетов по заказам клиентов
            bool active = true;
            while (active)
            {
                try
                {
                    Console.WriteLine("Выберите вариант отчета по заказам: \n" +
                    "1. Общее количество заказов \n" +
                    "2. Общая сумма всех заказов \n" +
                    "3. График доставок на дату \n" +
                    "4. Выход из приложения");

                    int statisticsReportType = Int32.Parse(Console.ReadLine());

                    if (statisticsReportType >= 1 && statisticsReportType <= 3)
                    {
                        StatisticsMonitor statisticsMonitor = new StatisticsMonitor(statisticsReportType, statisticsDocuments);
                    }
                    else if (statisticsReportType == 4)
                    {
                        active = false;
                    }
                    else { Console.WriteLine("Некорректный ввод, попробуйте еще раз"); }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            if (active == false)
                {
                    Environment.Exit(0);
                }
                
            }
        }
        static void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            XmlSeverityType type = XmlSeverityType.Warning;
            if (Enum.TryParse<XmlSeverityType>("Error", out type))
            {
                if (type == XmlSeverityType.Error) throw new Exception(e.Message);
            }
        }
    }
}
