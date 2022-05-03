using System;
using System.Linq;

namespace Facade.Shop
{
    public class OrderFacade
    {
        // набор сервисов, которые надо "обернуть в фасад"
        private WarehouseService _warehouse;
        private PaymentService _payment;
        private DeliveryService _delivery;
        private EmailService _email;

        public OrderFacade()
        {
            // инициализируем зависимости в конструкторе класса
            _warehouse = new WarehouseService();
            _payment = new PaymentService();
            _delivery = new DeliveryService();
            _email = new EmailService();
        }

        public void CreateOfflineOrder(Client client, Product[] products)
        {
            // проверяем наличие товаров, и если их нет завершаем обработку заказа
            if (!_warehouse.CheckProducts(products))
            {
                Console.WriteLine("Товаров нет на складе");
                return;
            }

            // получаем общую цену всех продуктов
            var cost = products.Sum(x => x.Cost);
            
            // генерируем счёт
            _payment.CreateCheque(cost);
            
            // создаём доставку
            _delivery.CreateDelivery(client.Address, products);
            
            _email.SendEmail(client.Email, $"Ваш счёт: {cost} руб.");
            _email.SendEmail(client.Email, "Ваш заказ  создан");
        }

        public void CreateOnlineOrder(Client client, Product[] products)
        {
            // проверяем наличие товаров, и если их нет завершаем обработку заказа
            if (!_warehouse.CheckProducts(products))
            {
                Console.WriteLine("Товаров нет на складе");
                return;
            }

            // получаем общую цену всех продуктов
            var cost = products.Sum(x => x.Cost);
            
            // выполняем платёж 
            _payment.ExecutePayment(client.CardNumber, client.CardDate, client.Cvc, cost);
            
            // создаём доставку
            _delivery.CreateDelivery(client.Address, products);
            
            _email.SendEmail(client.Email, "Ваш заказ  создан");
        }
    }
}