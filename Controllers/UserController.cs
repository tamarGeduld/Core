using Microsoft.AspNetCore.Mvc;
using Lesson3.Models;
using Lesson3.Services;
using Lesson3.Interfaces;

namespace Lesson3.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
     private IUserService userService;


public UserController(IUserService userService)
    {
        this.userService = userService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Users>> Get()
    {
        return userService.Get();
    }

    [HttpGet("{id}")]
    public ActionResult<Users> Get(int id)
    {
        var user = userService.Get(id);
        if (user == null)
            return NotFound();

        return user;
    }
    
    [HttpPost]
    public ActionResult Post(Users newUser)
    {
        var newId = userService.Insert(newUser);
        if (newId == -1)
        {
            return BadRequest();
        }

        return CreatedAtAction(nameof(Post), new { Id= newId});
    }

     
    [HttpPut("{id}")]
    public ActionResult Put(int id, Users newUser)
    {
        if (userService.Update(id, newUser))
        {
            return NoContent();
        }
        return BadRequest();
        
        /*var pizza = list.FirstOrDefault(p => p.Id == id);
        if (pizza == null)
            return NotFound();*/
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        if (userService.Delete(id))
            return Ok();
            
        return NotFound();
    }   
   
}
