using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.IO;
using System.Xml.Linq;
using System.Xml.Schema;


namespace OrderCreator
{
    public class OrderProcessor
    {
        private readonly DirectoryInfo customerOrdersDI;
        private readonly string xsdSchemaPath;
        private readonly string deliveryOrdersDirectory;
        private readonly IReporter reporter;
        XDocument[] statisticsDocuments;

        public OrderProcessor(DirectoryInfo customerOrdersDI, string xsdSchemaPath, string deliveryOrdersDirectory, IReporter reporter)
        {
            this.customerOrdersDI = customerOrdersDI;
            this.xsdSchemaPath = xsdSchemaPath;
            this.deliveryOrdersDirectory = deliveryOrdersDirectory;
            this.reporter = reporter;
        }
        public IEnumerable<XDocument> Process()
        {
            // проверка на наличие заказов клиентов
            if (customerOrdersDI.GetFiles().Length == 0)
            {
                reporter.Report(String.Format("В каталоге {0} не найдено ни одного заказа клиента", customerOrdersDI.FullName));
                Environment.Exit(0);
            }

            // проход по всем заказам клиентов, формирование ордеров на выгрузку
            int i = 1;
            foreach (var fi in customerOrdersDI.GetFiles())
            {
                // загрузка заказа
                XDocument xDoc_in = XDocument.Load
                    (fi.Directory + @"\" + fi.Name);

                //валидация заказа по xsd-схеме
                XmlSchemaSet schema = new XmlSchemaSet();
                schema.Add("", xsdSchemaPath);
                xDoc_in.Validate(schema, ValidationEventHandler);

                // обработка заказа, вывод данных о заказе в консоль, логирование данных о загрузке
                CustomerOrderLoader customerOrderLoader = new CustomerOrderLoader(reporter);
                CustomerOrder customerOrder = customerOrderLoader.LoadCustomerOrder(xDoc_in);

                // создание ордера на отгрузку из загруженного заказа, сохрание документа, логирование данных
                DeliveryOrderUnloader deliveryOrderUnloader = new DeliveryOrderUnloader();
                XDocument xDoc_out = deliveryOrderUnloader.CreateDeliveryOrders(customerOrder);
                xDoc_out.Save(deliveryOrdersDirectory + @"\" + "delivery_order_" + i + ".xml");
                i++;

                // добавление документа в коллекцию для дальнейшего использования в отчетах
                yield return xDoc_in;
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
