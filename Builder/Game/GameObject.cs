using System;
using Builder.Examples;

namespace Builder.Game
{
    public class GameObject
    {
        // можно ли взаимодействовать с элементом
        public bool IsInteractive { get; set; }

        // двигается элемент
        public bool IsMovable { get; set; }

        // событие при движении элемента
        public Action Move { get; set; }

        // видимость элемента
        public bool IsVisible { get; set; }

        // название элемента
        public string Name { get; set; }

        // событие при касании с элементом
        public Action TouchEvent { get; set; }

        // событие уничтожения элемента
        public Action DestroyEvent { get; set; }

        // расположение элемента по X
        public double CoordX { get; set; }

        // расположение элемента по Y
        public double CoordY { get; set; }

        // расположение элемента по Z
        public double CoordZ { get; set; }
    }

    public class GameObjectBuilder
    {
        private GameObject _gameObject;

        public GameObjectBuilder()
        {
            _gameObject = new GameObject();
        }

        public GameObjectBuilder IsInteractive()
        {
            _gameObject.IsInteractive = true;
            return this;
        }

        public GameObjectBuilder IsVisible()
        {
            _gameObject.IsVisible = true;
            return this;
        }

        public GameObjectBuilder SetName(string name)
        {
            _gameObject.Name = name;
            return this;
        }

        public GameObjectBuilder SetLocation(double x, double y, double z)
        {
            _gameObject.CoordX = x;
            _gameObject.CoordY = y;
            _gameObject.CoordZ = z;
            return this;
        }

        public GameObjectBuilder SetMove(Action action)
        {
            _gameObject.IsMovable = true;
            _gameObject.Move += action;
            return this;
        }

        public GameObjectBuilder AddTouchEvent(Action action)
        {
            _gameObject.TouchEvent += action;
            return this;
        }

        public GameObjectBuilder AddDestroyEvent(Action action)
        {
            _gameObject.DestroyEvent += action;
            return this;
        }

        public GameObject Build()
        {
            return _gameObject;
        }
    }

    public class Place
    {
        public void Construct()
        {
            // строим игровой объект "Кролик"
            var bunny = new GameObjectBuilder()
                .SetName("Кролик")
                .IsInteractive()
                .IsVisible()
                .SetLocation(10, 10, 10)
                .SetMove(() => Console.WriteLine("Кролик прыгает"))
                .AddDestroyEvent(() => Console.WriteLine("Кролик умер"))
                .AddTouchEvent(() => Console.WriteLine("Кролик озлобленно огрызнулся"))
                .Build();

            // строим игровой объект "чай"
            var tea = new GameObjectBuilder()
                .IsInteractive()
                .IsVisible()
                .SetLocation(110, 112, 111)
                .SetName("TESS (не реклама)")
                .AddDestroyEvent(() => Console.WriteLine("Чай выпит"))
                .AddTouchEvent(() => Console.WriteLine("Чай подобран"))
                .Build();

            Console.WriteLine("Локация загружена");
            Console.WriteLine($"Кролик расположен в координатах {bunny.CoordX} {bunny.CoordY} {bunny.CoordZ}");
            bunny.Move?.Invoke();
            bunny.TouchEvent?.Invoke();

            Console.WriteLine($"Чай расположен в координатах {tea.CoordX} {tea.CoordY} {tea.CoordZ}");
            tea.DestroyEvent?.Invoke();
        }
    }
}