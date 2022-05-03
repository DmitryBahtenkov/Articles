using System.Runtime.InteropServices.ComTypes;
using RestExample.Models;

namespace RestExample.Services;

public class PizzaService
{
    private readonly RestExample.AppContext _appContext;

    public PizzaService(AppContext appContext)
    {
        _appContext = appContext;
    }

    public PizzaModel Create(PizzaModel pizza)
    {
        // сначала ищем существующие данные
        var existing = _appContext.Pizzas.FirstOrDefault(x => x.Name == pizza.Name);
        // если пицца с таким названием уже есть - выбрасываем исключение
        if (existing is not null)
        {
            throw new Exception("Такая пицца уже существует");
        }
        
        // добавляем пиццу в нашу таблицу
        var added = _appContext.Pizzas.Add(pizza);
        // сохраняем данные
        _appContext.SaveChanges();

        // вовзращаем информацию о созданной пицце
        return added.Entity;
    }

    public PizzaModel Update(PizzaModel pizza)
    {
        // обновляем пиццу в БД. Метод Update
        // найдёт её по ID и обновит все поля 
        var updated = _appContext.Pizzas.Update(pizza);
        // сохраняем данные
        _appContext.SaveChanges();

        return updated.Entity;
    }

    public List<PizzaModel> Search(string name = null)
    {
        // если name не пустой, ищем по имени
        if (!string.IsNullOrEmpty(name))
        {
            return _appContext.Pizzas
                    // приводим данные из БД и поисковый запрос в нижний регистр для более удобного поиска
                .Where(x => x.Name.ToLower().Contains(name.ToLower()))
                .ToList();
        }
        
        // если name не передали - просто возвращаем список всех пицц
        return _appContext.Pizzas.ToList();
    }

    public PizzaModel? Get(Guid id)
    {
        return _appContext.Pizzas.Find(id);
    }

    public void Delete(Guid id)
    {
        // ищем пиццу для удаления в БД
        var pizza = _appContext.Pizzas.Find(id);

        // если пицца не найдена - ничего не делаем
        if (pizza is null)
        {
            return;
        }
        
        // удаляем пиццу из таблицы и сохраняем данные
        _appContext.Pizzas.Remove(pizza);
        _appContext.SaveChanges();
    }
}
