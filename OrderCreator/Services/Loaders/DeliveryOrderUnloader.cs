using System;
using System.IO;
using System.Xml.Linq;
using NLog;

// Класс, отвечающий за формирование ордеров на доставку на основании заказов клиентов

namespace OrderCreator
{
    class DeliveryOrderUnloader : IOrderLoader
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
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

            logger.Info("New delivery order is created. Customer name: {0}, delivery time: {1}",
                deliveryOrder.getName(), deliveryOrder.getDeliveryTime());

            return xdoc;
        }

    }
}
