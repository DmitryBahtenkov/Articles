using Autofac;
using AutofacApi.Contract;
using AutofacApi.Posts.DataAccess;
using AutofacApi.Posts.Models;
using AutofacApi.Posts.Services;

namespace AutofacApi.Posts;

public class PostsModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<PostRepository>().As<IRepository<Post>>().InstancePerLifetimeScope();
        builder.RegisterType<PostService>().InstancePerLifetimeScope();
    }
}
