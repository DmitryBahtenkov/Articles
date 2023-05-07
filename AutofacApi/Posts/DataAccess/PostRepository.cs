using AutofacApi.Contract;
using AutofacApi.Users.Models;

namespace AutofacApi.Posts.DataAccess;

public class PostRepository : IRepository<User>
{
    public Task<User> Create(User model)
    {
        throw new NotImplementedException();
    }

    public Task Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Task<List<User>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<User> GetById(int id)
    {
        throw new NotImplementedException();
    }

    public Task<User> Update(User model)
    {
        throw new NotImplementedException();
    }
}
