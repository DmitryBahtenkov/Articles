using System;

namespace FactoryMethod.Classic.Example
{
    abstract class Product
    {
        //тут классы-наследники будут хранить своё имя
        public abstract string Name { get; }
    }

    class ConcreteProductA : Product
    {
        public override string Name => nameof(ConcreteProductA);
    }

    class ConcreteProductB : Product
    {
        public override string Name => nameof(ConcreteProductB);
    }

    abstract class Creator
    {
        public void Operation()
        {
            var product = FactoryMethod();
            // какая-то логика с экземпляром класса Product
            Console.WriteLine(product.Name);
        }

        public abstract Product FactoryMethod();
    }
    
    class ConcreteCreatorA : Creator
    {
        public override Product FactoryMethod()
        {
            return new ConcreteProductA();
        }
    }
    
    class ConcreteCreatorB : Creator
    {
        public override Product FactoryMethod()
        {
            return new ConcreteProductB();
        }
    }
}