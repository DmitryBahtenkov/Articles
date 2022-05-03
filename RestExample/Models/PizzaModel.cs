namespace RestExample.Models;

public class PizzaModel
{
    /// <summary>
    /// Уникальный идентификатор
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Цена пиццы
    /// </summary>
    public decimal Price { get; set; }
    /// <summary>
    /// Название пиццы
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Описание пиццы
    /// </summary>
    public string Description { get; set; }
}