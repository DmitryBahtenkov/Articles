using Autofac;
using Autofac.Core.Activators.Reflection;
using AutofacLifeTimeEvents;

using var container = BuildContainer();
{
    var dep = container.Resolve<Dependency>();
    dep.Print();
}
Console.WriteLine("end application");

static IContainer BuildContainer()
{
    var builder = new ContainerBuilder();
    builder
        .RegisterType<Dependency>()
        .InstancePerLifetimeScope()
        .OnPreparing(x => x.Parameters = new []{ new NamedParameter("customParameter", "this is a custom parameter") })
        .Trace();
    
    builder
        .RegisterType<Service>()
        .InstancePerLifetimeScope()
        .Trace();
    
    return builder.Build();
}