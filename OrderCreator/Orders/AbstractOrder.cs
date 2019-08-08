using System;
using System.Collections.Generic;

// Абстрактный класс "Ордер", от которого наследуются классы CustomerOrder и DeliveryOrder

namespace OrderCreator
{
    public abstract class AbstractOrder
    {
        private int id;
        private string client_name;
        private string client_address;
        private DateTime delivery_time;
        private List<OrderGoods> OrderGoodsList;
        private string comment;

        public int getId()
        {
            return id;
        }
        public void setId(int _id)
        {
            this.id = _id;
        }
        public string getName()
        {
            return client_name;
        }
        public void setName(string _name)
        {
            this.client_name = _name;
        }
        public string getAddress()
        {
            return client_address;
        }
        public void setAddress(string _address)
        {
            this.client_address = _address;
        }
        public DateTime getDeliveryTime()
        {
            return delivery_time;
        }
        public void setDeliveryTime(DateTime _deliveryTime)
        {
            this.delivery_time = _deliveryTime;
        }
        public List<OrderGoods> getOrderGoodsList()
        {
            return OrderGoodsList;
        }
        public void setOrderGoodsList(List<OrderGoods> _orderGoodsList)
        {
            this.OrderGoodsList = _orderGoodsList;
        }
        public string getComment()
        {
            return comment;
        }
        public void setComment(string _comment)
        {
            this.comment = _comment;
        }

    }

    public class OrderGoods
    {
        public string name;
        public float price;
        public int quantity;
        public float sum;
    }
}
