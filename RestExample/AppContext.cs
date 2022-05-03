using Microsoft.EntityFrameworkCore;
using RestExample.Models;

namespace RestExample;

public class AppContext : DbContext
{
    public DbSet<PizzaModel> Pizzas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("Database");
    }
}