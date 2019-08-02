using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;

// Класс, формирующий отчеты

namespace OrderCreator
{
    class StatisticsMonitor
    {
        public readonly int totalOrdersAmountReport = 1;
        public readonly int totalOrdersSumReport = 2;
        public readonly int deliveryScheduleReport = 3;

        List<XDocument> statisticsData;

        public StatisticsMonitor(int reportType, List<XDocument> statisticsDocuments)
        {
            statisticsData = statisticsDocuments;
            if (reportType == totalOrdersAmountReport)
            {
                int totalOrdersAmount = CountOrdersAmount();
                Console.WriteLine("\nОбщее количество заказов: {0} \n", totalOrdersAmount);
            }
            if (reportType == totalOrdersSumReport)
            {
                float totalOrdersSum = CountOrdersSum();
                Console.WriteLine("\nОбщая сумма всех заказов: {0} \n", totalOrdersSum);
            }
            if (reportType == deliveryScheduleReport)
            {
                Dictionary<String, String> deliverySchedule = DeliveryShedule();
                foreach (var delivery in deliverySchedule.OrderBy(delivery => delivery.Value))
                {
                    Console.WriteLine("\nВремя доставки: {0}, Клиент: {1}\n", delivery.Value, delivery.Key);
                }
            }
        }

        public int CountOrdersAmount()
        {
            int ordersAmount = 0;
            foreach (XDocument xDoc in statisticsData)
            {
                ordersAmount++;
            }
            return ordersAmount;
        }

        public float CountOrdersSum()
        {
            float ordersSum = 0;
            foreach (XDocument xDoc in statisticsData)
            {
                foreach (XElement orderGoodsElement in
                xDoc.Element("customer_order").Element("goods").Elements("good"))
                {
                    ordersSum += Convert.ToSingle(orderGoodsElement.Element("good_sum").Value);
                }
            }
            return ordersSum;
        }

        public Dictionary<String, String> DeliveryShedule()
        {
            Dictionary<String, String> shedule = new Dictionary<String, String>();
            foreach (XDocument xDoc in statisticsData)
            {
                shedule.Add
                    (
                xDoc.Element("customer_order").Element("customer_name").Value,
                xDoc.Element("customer_order").Element("delivery_time").Value
                    );
            }
            return shedule;
        }
    }

}
