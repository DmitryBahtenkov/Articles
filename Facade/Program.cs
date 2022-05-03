using System;
using Facade.Shop;

namespace Facade
{
    class Program
    {
        static void Main(string[] args)
        {
            var facade = new OrderFacade();
            var client = new Client
            {
                Email = "a@a.a",
                Cvc = 124,
                Address = "ул. пушкина дом колотушкина",
                CardDate = DateTime.Now,
                CardNumber = "1234 1234 1234 1234"
            };

            var products = new[]
            {
                new Product {Name = "Продукт", Cost = 1000}
            };
            
            facade.CreateOnlineOrder(client, products);
        }
    }
}
