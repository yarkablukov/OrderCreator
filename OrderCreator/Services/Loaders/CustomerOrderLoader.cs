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
            _reporter.Report(String.Format("Имя клиента: {0}", customerOrder.getName()));
            _reporter.Report(String.Format("Адрес доставки: {0}", customerOrder.getAddress()));
            _reporter.Report(String.Format("Время доставки: {0}", customerOrder.getDeliveryTime()));
            _reporter.Report(String.Format("Комментарий к заказу: {0}", customerOrder.getComment()));
            _reporter.Report(String.Format("Спецификация заказа: "));

            int i = 0;
            foreach (OrderGoods orderGoods in customerOrder.getOrderGoodsList())
            {
                i++;
                _reporter.Report("");
                _reporter.Report(String.Format("#{0}", i));
                _reporter.Report(String.Format("Товар: {0}", orderGoods.name));
                _reporter.Report(String.Format("Цена: {0}", orderGoods.price));
                _reporter.Report(String.Format("Количество: {0}", orderGoods.quantity));
                _reporter.Report(String.Format("Сумма заказа: {0}", orderGoods.sum));
            }
            _reporter.Report("");

            logger.Info("New customer order loaded. Customer name: {0}, delivery time: {1}",
                customerOrder.getName(), customerOrder.getDeliveryTime());

            return customerOrder;
        }
    }
}
