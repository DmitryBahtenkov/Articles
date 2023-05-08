using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutofacApi.Helpers;
using AutofacApi.Posts;
using AutofacApi.Users;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>((containerBuilder) =>
{
    containerBuilder.RegisterAssemblyModules(Assembly.GetExecutingAssembly());
    containerBuilder.Register<IDateTimeService>(context =>
    {
        if (builder.Environment.IsEnvironment("TEST"))
        {
            return new StaticDateTimeService(new DateTime(2011, 11, 11));
        }

        return new DateTimeService();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();