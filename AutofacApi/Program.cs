using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutofacApi.Posts;
using AutofacApi.Users;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>((containerBuilder) =>
{
    containerBuilder.RegisterAssemblyModules(Assembly.GetExecutingAssembly());
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