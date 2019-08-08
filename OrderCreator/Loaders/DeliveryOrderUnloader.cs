using System;
using System.IO;
using System.Xml.Linq;

// Класс, отвечающий за формирование ордеров на доставку на основании заказов клиентов

namespace OrderCreator
{
    class DeliveryOrderUnloader : IOrderLoader
    {
        public XDocument CreateDeliveryOrders(CustomerOrder co)
        {
                DeliveryOrder deliveryOrder = new DeliveryOrder();
                deliveryOrder.setName(co.getName());
                deliveryOrder.setAddress(co.getAddress());
                deliveryOrder.setDeliveryTime(co.getDeliveryTime());
                deliveryOrder.setComment(co.getComment());
                deliveryOrder.setOrderGoodsList(co.getOrderGoodsList());

                XDocument xdoc = new XDocument(new XElement("delivery_order",
                       new XElement("customer_name", deliveryOrder.getName()),
                       new XElement("customer_address", deliveryOrder.getAddress()),
                       new XElement("delivery_time", deliveryOrder.getDeliveryTime()),
                       new XElement("goods"),
                       new XElement("customer_comment", deliveryOrder.getComment())));

                XElement goodsElement = xdoc.Element("delivery_order").Element("goods");
                foreach (OrderGoods orderGoods in co.getOrderGoodsList())
                {
                    goodsElement.Add(new XElement ("good",
                        new XElement("good_name", orderGoods.name),
                        new XElement("good_price", orderGoods.price),
                        new XElement("good_quantity", orderGoods.quantity),
                        new XElement("good_sum", orderGoods.sum))
                        );
                }
            return xdoc;
        }

        public void Log(string fileName)
        {
            string writePath = @"C:\Users\Slava\source\repos\OrderCreator\OrderCreator\bin\Debug\netcoreapp2.1\logs\delivery_orders_logs.txt";

            try
            {
                using (StreamWriter sw = new StreamWriter(writePath, true, System.Text.Encoding.Default))
                {
                    sw.WriteLine("Создан ордер на отгрузку в файле: {0}, время: {1}", fileName, DateTime.Now);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
