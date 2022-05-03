using System;

namespace Builder.Examples
{
    public class Product
    {
        public string Property1 { get; set; }
        public string Property2 { get; set; }
        public string Property3 { get; set; }
        public string Property4 { get; set; }
        public string Property5 { get; set; }
    }
    
    public class FluentBuilder
    {
        private readonly Product _product;

        public FluentBuilder()
        {
            _product = new Product();
        }

        public FluentBuilder WithProp1(string value)
        {
            _product.Property1 = value;
            return this;
        }
        
        public FluentBuilder WithProp2(string value)
        {
            _product.Property2 = value;
            return this;
        }
        
        public FluentBuilder WithProp3(string value)
        {
            _product.Property3 = value;
            return this;
        }
        
        public FluentBuilder WithProp4(string value)
        {
            _product.Property4 = value;
            return this;
        }
        
        public FluentBuilder WithProp5(string value)
        {
            _product.Property5 = value;
            return this;
        }

        public Product Build()
        {
            return _product;
        }
    }

    public class Director
    {
        private readonly FluentBuilder _builder;

        public Director(FluentBuilder builder)
        {
            _builder = builder;
        }

        public void Construct()
        {
            // "строим" объект с помощью цепочки вызовов
            var product = _builder
                .WithProp1("Prop 1")
                .WithProp3("Prop 3")
                .WithProp5("Prop 5")
                .Build();
            
            Console.WriteLine($"Продукт построен. Свойство 1: {product.Property1}, Свойство 3: {product.Property3}, Свойство 5: {product.Property5}");
        }
    }
}