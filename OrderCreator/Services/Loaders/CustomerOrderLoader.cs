using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using NLog;

// Класс-загрузчик заказов клиентов

namespace OrderCreator
{
    public class CustomerOrderLoader : IOrderLoader
    {
        private readonly IReporter _reporter;
        private static Logger logger = LogManager.GetCurrentClassLogger();


        public CustomerOrderLoader(IReporter reporter)
        {
            _reporter = reporter;
        }
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

            _reporter.Report("Новый заказ");
            _reporter.Report("Имя клиента: " + customerOrder.getName());
            _reporter.Report("Адрес доставки: " + customerOrder.getAddress());
            _reporter.Report("Время доставки: " + customerOrder.getDeliveryTime());
            _reporter.Report("Комментарий к заказу: " + customerOrder.getComment());
            _reporter.Report("Спецификация заказа: ");

            int i = 0;
            foreach (OrderGoods orderGoods in customerOrder.getOrderGoodsList())
            {
                i++;
                _reporter.Report("");
                _reporter.Report("#" + i);
                _reporter.Report("Товар: " + orderGoods.name);
                _reporter.Report("Цена: " + orderGoods.price);
                _reporter.Report("Количество: " + orderGoods.quantity);
                _reporter.Report("Сумма заказа: " + orderGoods.sum);
            }
            _reporter.Report("");

            logger.Info("New customer order loaded. Customer name: {0}, delivery time: {1}",
                customerOrder.getName(), customerOrder.getDeliveryTime());

            return customerOrder;
        }
    }
}
