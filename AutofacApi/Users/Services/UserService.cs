using AutofacApi.Contract;
using AutofacApi.Users.Models;

namespace AutofacApi.Users.Services;

public class UserService
{
    private readonly IRepository<User> _repository;

    public UserService(IRepository<User> repository)
    {
        _repository = repository;
    }

    public async Task<List<User>> Get()
    {
        return await _repository.GetAll();
    }
}
