using Autofac;
using AutofacApi.Contract;
using AutofacApi.Users.DataAccess;
using AutofacApi.Users.Models;
using AutofacApi.Users.Services;

namespace AutofacApi.Users;

public class UserModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<UserRepository>().As<IRepository<User>>().InstancePerLifetimeScope();
        builder.RegisterType<UserService>().InstancePerLifetimeScope();
    }
}
