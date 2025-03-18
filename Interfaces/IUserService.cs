using Microsoft.AspNetCore.Mvc;
using Lesson3.Models;

namespace Lesson3.Interfaces;

public interface IUserService 
{
    List<Users> Get();

    Users Get(int id);
    
    int Insert(Users newUser);
     
    bool Update(int id, Users newUser);

    bool Delete(int id);
   
}