using System;

namespace Facade.Shop
{
    public class WarehouseService
    {
        public bool CheckProducts(Product[] products)
        {
            // тут должна быть логика проверки товаров на складе
            Console.WriteLine("Все продукты в наличии");
            return true;
        }
    }

    public class PaymentService
    {
        public void ExecutePayment(string number, DateTime date, int cvc, int cost)
        {
            // логика взаимодействия с платёжным сервисом
            Console.WriteLine($"Выполнен платёж на сумму {cost} руб.");
        }

        public void CreateCheque(int cost)
        {
            // логика генерации чека на офлайн оплату
            Console.WriteLine($"Чек для заказа на сумму {cost} руб.");
        }
    }

    public class DeliveryService
    {
        public void CreateDelivery(string address, Product[] products)
        {
            // логика создания заявки на доставку выбранных продуктов
            Console.WriteLine("Заявка на доставку успешно зарегистрирована");
        }
    }

    public class EmailService
    {
        public void SendEmail(string email, string text)
        {
            //логика отправки письма
            Console.WriteLine($"Сообщение отправлено на почту {email}");
            Console.WriteLine(text);
        }
    }
}