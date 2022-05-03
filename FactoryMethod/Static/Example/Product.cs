using System;

namespace FactoryMethod.Static.Example
{
    public abstract class Product
    {}

    public class ConcreteProductA : Product
    {}

    public class ConcreteProductB : Product
    {}

    public static class Factory
    {
        public static Product CreateProduct(string arg)
        {
            if(arg == "A")
            {
                return new ConcreteProductA();
            }                
            if(arg == "B")
            {
                return new ConcreteProductB();    
            }
            else
            {
                throw new Exception("некорректный тип продукта");
            }
        }
    }
}