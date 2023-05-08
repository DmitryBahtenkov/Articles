using AutofacApi.Contract;
using AutofacApi.Posts.Models;
using AutofacApi.Users.Models;

namespace AutofacApi.Posts.DataAccess;

public class PostRepository : IRepository<Post>
{
    public Task<Post> Create(Post model)
    {
        throw new NotImplementedException();
    }

    public Task Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Task<List<Post>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<Post> GetById(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Post> Update(Post model)
    {
        throw new NotImplementedException();
    }
}
