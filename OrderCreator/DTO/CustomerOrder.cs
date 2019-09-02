using System;

// Класс, олицетворяющий заказ клиента

namespace OrderCreator
{
    public class CustomerOrder: AbstractOrder
    {
       private static int idCounter = 0;

       public CustomerOrder()
        {
            idCounter++;
            setId(idCounter);
        } 
    }
}
