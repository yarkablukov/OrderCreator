using System;

// Класс, олицетворяющий ордер на выгрузку

namespace OrderCreator
{
    class DeliveryOrder : AbstractOrder
    {
        private static int idCounter = 0;

        public DeliveryOrder()
        {
            idCounter++;
            setId(idCounter);
        }
    }
}
