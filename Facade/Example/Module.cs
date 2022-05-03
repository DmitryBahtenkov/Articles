using System.Reflection;
using System;
namespace Facade.Example
{
    //какие-то внутренние модули
    public class Module1
    {
        public void OperationA()
        {
            Console.WriteLine("operation a");
        }
    }

    public class Module2
    {
        public void OperationB()
        {
            Console.WriteLine("operation b");
        }
    }

    public class Module3
    {
        public void OperationC()
        {
            Console.WriteLine("operation c");
        }
    }

    /// <summary>
    /// Упрощённая версия фасада, когда доступ к модулям
    /// предоставляется напрямую
    /// </summary>
    public class SimpleFacade
    {
        // модули, которые скрывает в себе фасад
        public Module1 Module1 { get; }
        public Module2 Module2 { get; }
        public Module3 Module3 { get; }


        public SimpleFacade()
        {
            // инициализация модулей
            Module1 = new Module1();
            Module2 = new Module2();
            Module3 = new Module3();
        }
    }

    /// <summary>
    /// Более строгий вариант фасада, когда мы
    /// можем использовать только методы в определённом порядке
    /// </summary>
    public class Facade
    {
        // модули, которые скрывает в себе фасад
        private Module1 _module1;
        private Module2 _module2;
        private Module3 _module3;

        public Facade()
        {
            // инициализация модулей
            _module1 = new Module1();
            _module2 = new Module2();
            _module3 = new Module3();
        }

        // первая операция, которая вызывает в нужном порядке внутренние методы
        public void FacadeOperationA()
        {
            _module1.OperationA();
            _module3.OperationC();
        }

        // вторая операция, которая вызывает в нужном порядке внутренние методы
        public void FacadeOperationB()
        {
            _module3.OperationC();
            _module2.OperationB();
            _module1.OperationA();
        }
    }
}