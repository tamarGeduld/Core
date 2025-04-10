using Microsoft.AspNetCore.Mvc;
using Project.Services;
using Project.Models;
using Project.Interfaces;


using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using Services.TokenService;


namespace Project.Controllers;

[ApiController]
[Route("api/login")]
public class LoginController : ControllerBase
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserService _userService;

    public LoginController(ICurrentUserService currentUserService,IUserService userService)
    {
        _currentUserService = currentUserService;
        _userService = userService;
    }

    [HttpPost]
    public ActionResult Login([FromBody] LoginRequest request)
    {
        var users = _userService.Get();
        var user = users.FirstOrDefault((u) => u.Name == request.UserName && u.Password == request.Password);
        if (user == null)
            return Unauthorized();
        _currentUserService.SetUser(user.Id.ToString(), user.Name, user.Role, user.Password);

        var claims = new List<Claim> { new Claim("type", "User"), new Claim("id", user.Id.ToString()), new Claim("userName", user.Name) };
        if (user.Role == "Admin")
            claims.Add(new Claim("type", "Admin"));
        var token = TokenService.GetToken(claims);
        return Ok(new { Id = user.Id , Token = TokenService.WriteToken(token)});
    }
}
