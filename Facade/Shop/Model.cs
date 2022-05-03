using System;

namespace Facade.Shop
{
    public class Product
    {
        public string Name { get; set; }
        public int Cost { get; set; } 
    }

    public class Client
    {
        public string Email { get; set; }
        public string CardNumber { get; set; }
        public DateTime CardDate { get; set; }
        public int Cvc { get; set; }
        public string Address { get; set; }
    }
}