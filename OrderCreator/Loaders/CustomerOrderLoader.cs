using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

// Класс-загрузчик заказов клиентов

namespace OrderCreator
{
    public class CustomerOrderLoader : IOrderLoader
    {
        public CustomerOrder LoadCustomerOrder(XDocument customerOrderDocument)
        {
            // изменение культурных настроек для корректной обработки float типов
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            CustomerOrder customerOrder = new CustomerOrder();
            List<OrderGoods> orderGoodsList = new List<OrderGoods>();
            customerOrder.setOrderGoodsList(orderGoodsList);

            customerOrder.setName(customerOrderDocument.Element("customer_order").Element("customer_name").Value);
            customerOrder.setAddress(customerOrderDocument.Element("customer_order").Element("customer_address").Value);
            customerOrder.setDeliveryTime(DateTime.Parse(
                customerOrderDocument.Element("customer_order").Element("delivery_time").Value));

            foreach (XElement orderGoodsElement in
                customerOrderDocument.Element("customer_order").Element("goods").Elements("good"))
            {
                OrderGoods orderGoods = new OrderGoods();
                orderGoods.name = orderGoodsElement.Element("good_name").Value;
                orderGoods.price = Convert.ToSingle(orderGoodsElement.Element("good_price").Value);
                orderGoods.quantity = Int32.Parse(orderGoodsElement.Element("good_quantity").Value);
                orderGoods.sum = Convert.ToSingle(orderGoodsElement.Element("good_sum").Value);
                orderGoodsList.Add(orderGoods);
            }

            Console.WriteLine("Новый заказ");
            Console.WriteLine("Имя клиента: {0}", customerOrder.getName());
            Console.WriteLine("Адрес доставки: {0}", customerOrder.getAddress());
            Console.WriteLine("Время доставки: {0}", customerOrder.getDeliveryTime());
            Console.WriteLine("Комментарий к заказу: {0}", customerOrder.getComment());
            Console.WriteLine("Спецификация заказа: ");

            int i = 0;
            foreach (OrderGoods orderGoods in customerOrder.getOrderGoodsList())
            {
                i++;
                Console.WriteLine();
                Console.WriteLine("#" + i);
                Console.WriteLine("Товар: {0}", orderGoods.name);
                Console.WriteLine("Цена: {0}", orderGoods.price);
                Console.WriteLine("Количество: {0}", orderGoods.quantity);
                Console.WriteLine("Сумма заказа: {0}", orderGoods.sum);
            }
            Console.WriteLine();

            return customerOrder;
        }

        public void Log(string fileName)
        {
            string writePath = @"C:\Users\Slava\source\repos\OrderCreator\OrderCreator\bin\Debug\netcoreapp2.1\logs\customer_orders_logs.txt";

            try
            {
                using (StreamWriter sw = new StreamWriter(writePath, true, System.Text.Encoding.Default))
                {
                    sw.WriteLine("Обработан заказ клиента из файла: {0}, время: {1}", fileName, DateTime.Now);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
