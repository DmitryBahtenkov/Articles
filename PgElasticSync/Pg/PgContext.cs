using Microsoft.EntityFrameworkCore;
using PgElasticSync.Models;

namespace PgElasticSync.Pg;

public sealed class PgContext : DbContext
{
    public DbSet<Employee> Employees { get; set; }

    public PgContext()
    {
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(
            "Server=127.0.0.1;Port=5432;Database=read_model;User Id=read_model_owner;Password=qwerty;");
    }
}