using System;

namespace Builder.Examples.Classic
{
    public class Product
    {
        public string Property1 { get; set; }
        public string Property2 { get; set; }
        public string Property3 { get; set; }
        public string Property4 { get; set; }
        public string Property5 { get; set; }
    }
    
    public abstract class Builder
    {
        // объект, который мы будем строить
        protected Product product;

        protected Builder()
        {
            // инициализируем объект в конструкторе
            product = new Product();
        }

        // методы для построения разных частей объекта
        public abstract void BuildProp1();
        public abstract void BuildProp2();
        public abstract void BuildProp3();
        public abstract void BuildProp4();
        public abstract void BuildProp5();

        // метод получения построенного продукта
        public Product GetProduct()
        {
            return product;
        }
    }

    public class ConcreteBuilder : Builder
    {
        public override void BuildProp1()
        {
            product.Property1 = "property 1 value";
        }

        public override void BuildProp2()
        {
            product.Property2 = "property 2 value";
        }

        public override void BuildProp3()
        {
            product.Property3 = "property 3 value";
        }

        public override void BuildProp4()
        {
            product.Property4 = "property 4 value";
        }

        public override void BuildProp5()
        {
            product.Property5 = "property 5 value";
        }
    }

    public class Director
    {
        private readonly Builder _builder;

        public Director(Builder builder)
        {
            _builder = builder;
        }

        public void Construct()
        {
            // выполняем различные этапы построения
            _builder.BuildProp1();
            _builder.BuildProp3();
            _builder.BuildProp5();

            var product = _builder.GetProduct();
            Console.WriteLine($"Продукт построен. Свойство 1: {product.Property1}, Свойство 3: {product.Property3}, Свойство 5: {product.Property5}");
        }
    }
}