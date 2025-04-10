using Microsoft.AspNetCore.Http;
using Project.Models;
using System;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ISession _session;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _session = _httpContextAccessor.HttpContext?.Session 
                   ?? throw new InvalidOperationException("Session is not available");
    }

    public bool IsAuthenticated => !string.IsNullOrEmpty(UserId);
    public string UserId => _session.GetString("UserId") ?? "";
    public string Name => _session.GetString("UserName") ?? "";
    public string Role => _session.GetString("UserRole") ?? "user";
    public string Password => _session.GetString("UserPassword") ?? "user";


    public void SetUser(string userId, string name, string role,string password)
    {
        _session.SetString("UserId", userId);
        _session.SetString("UserName", name);
        _session.SetString("UserRole", role);
        _session.SetString("UserPassword", password);

    }

    public void ClearUser()
    {
        _session.Remove("UserId");
        _session.Remove("UserName");
        _session.Remove("UserRole");
        _session.Remove("UserPassword");

    }

    public User? GetCurrentUser()
    {
        if (!IsAuthenticated) return null;
        return new User
        {
            Id = int.Parse(UserId),
            Name = Name,
            Role = Role,
            Password = Password
        };
    }
}
