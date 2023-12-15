using Autofac;
using Autofac.Builder;

namespace AutofacLifeTimeEvents;

public class Dependency
{
    private readonly Service _service;
    private readonly string _customParameter;

    public Dependency(string customParameter, Service service)
    {
        _service = service;
        _customParameter = customParameter;
        Console.WriteLine($"Constructor of Dependency executed with custom param '{customParameter}'");
    }

    public void Print()
    {
        Console.WriteLine("Print() method");
        Console.WriteLine(_service.GetText());
    }
}

public static class RegistrationExtensions
{
    public static IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> 
        Trace<TLimit, TActivatorData, TRegistrationStyle>(this IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> registration)
    {
        return registration.OnPreparing(e => { Console.WriteLine($"{e.Component.Activator.LimitType.Name}.preparing"); })
            .OnActivating(e => { Console.WriteLine($"{e.Component.Activator.LimitType.Name}.activating"); })
            .OnActivated(e => { Console.WriteLine($"{e.Component.Activator.LimitType.Name}.activated"); })
            .OnRelease(e => { Console.WriteLine($"{e.GetType().Name}.release"); });
    }
}