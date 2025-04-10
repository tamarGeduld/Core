using Project.Models; 

public interface ICurrentUserService
{
    User? GetCurrentUser();
    void SetUser(string userId, string name, string role,string password);
    void ClearUser();
}
