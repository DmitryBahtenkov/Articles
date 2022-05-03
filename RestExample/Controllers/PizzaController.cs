using Microsoft.AspNetCore.Mvc;
using RestExample.Models;
using RestExample.Services;

namespace RestExample.Controllers;

[ApiController]
[Route("api/pizza")]
public class PizzaController : ControllerBase
{
    private readonly PizzaService _pizzaService;

    public PizzaController(PizzaService pizzaService)
    {
        _pizzaService = pizzaService;
    }

    [HttpPost]
    public PizzaModel Create([FromBody] PizzaModel pizza)
        => _pizzaService.Create(pizza);
    
    [HttpPut]
    public PizzaModel Update([FromBody] PizzaModel pizza)
        => _pizzaService.Update(pizza);
    
    [HttpGet("search")]
    public List<PizzaModel> Search([FromQuery] string name = null)
        => _pizzaService.Search(name);
    
    [HttpGet("{id}")]
    public PizzaModel Get(Guid id)
        => _pizzaService.Get(id);
    
    [HttpDelete("{id}")]
    public void Delete(Guid id)
        => _pizzaService.Delete(id);
}