namespace Builder.Examples.Construct
{
    public class Product
    {
        // наборы свойств для использования
        public string Property1 { get; set; }
        public string Property2 { get; set; }
        public string Property3 { get; set; }
        public string Property4 { get; set; }
        public string Property5 { get; set; }
        
        // конструктор для первого случая

        public Product(string property1, string property2, string property3)
        {
            Property1 = property1;
            Property2 = property2;
            Property3 = property3;
        }

        // конструктор для второго случая
        public Product(string property4, string property5)
        {
            Property4 = property4;
            Property5 = property5;
        }

        // конструктор для третьего случая
        public Product(string property1, string property2, string property3, string property4, string property5)
        {
            Property1 = property1;
            Property2 = property2;
            Property3 = property3;
            Property4 = property4;
            Property5 = property5;
        }
    }

    public class Product2
    {
        // наборы свойств для использования
        public string Property1 { get; set; }
        public string Property2 { get; set; }
        public string Property3 { get; set; }
        public string Property4 { get; set; }
        public string Property5 { get; set; }

        public Product2(string property1 = null,
            string property2 = null,
            string property3 = null,
            string property4 = null,
            string property5 = null)
        {
            Property1 = property1;
            Property2 = property2;
            Property3 = property3;
            Property4 = property4;
            Property5 = property5;
        }
    }
}