using Autofac;
using Autofac.Core.Lifetime;
using AutofacApi.Users.Models;
using AutofacApi.Users.Services;
using Microsoft.AspNetCore.Mvc;

namespace AutofacApi.Users.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<List<Models.User>> Get()
    {
        return await _userService.Get();
    }
}