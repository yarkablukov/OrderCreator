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
        private readonly IReporter reporter;

        List<XDocument> statisticsData;

        public StatisticsMonitor(int reportType, List<XDocument> statisticsDocuments, IReporter reporter)
        {
            this.reporter = reporter;
            statisticsData = statisticsDocuments;
            if (reportType == totalOrdersAmountReport)
            {
                int totalOrdersAmount = CountOrdersAmount();
                reporter.Report("\nОбщее количество заказов: \n" + totalOrdersAmount);
            }
            if (reportType == totalOrdersSumReport)
            {
                float totalOrdersSum = CountOrdersSum();
                reporter.Report("\nОбщая сумма всех заказов: \n" + totalOrdersSum);
            }
            if (reportType == deliveryScheduleReport)
            {
                Dictionary<String, String> deliverySchedule = DeliveryShedule();
                foreach (var delivery in deliverySchedule.OrderBy(delivery => delivery.Value))
                {
                    reporter.Report("\nВремя доставки: " + delivery.Value + ", Клиент: " + delivery.Key + "\n");
                }
            }
        }

        public static void ShowStatistics (List<XDocument> documentslist, out bool isActive, IReporter reporter)
        {
            isActive = true;
            reporter.Report("Выберите вариант отчета по заказам: \n" +
                    "1. Общее количество заказов \n" +
                    "2. Общая сумма всех заказов \n" +
                    "3. График доставок на дату \n" +
                    "4. Выход из приложения");

            int statisticsReportType = Int32.Parse(Console.ReadLine());

            if (statisticsReportType >= 1 && statisticsReportType <= 3)
            {
                StatisticsMonitor statisticsMonitor = new StatisticsMonitor(statisticsReportType, documentslist, reporter);
            }
            else if (statisticsReportType == 4)
            {
                isActive = false;
            }
            else { reporter.Report("Некорректный ввод, попробуйте еще раз"); }
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
