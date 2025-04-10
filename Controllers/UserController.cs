using Microsoft.AspNetCore.Mvc;
using Project.Models;
using Project.Services;
using Project.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace Project.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService userService;
    private readonly ICurrentUserService currentUserService;
    private readonly IBookService bookService;

    public UserController(IUserService userService, ICurrentUserService currentUserService, IBookService bookService)
    {
        this.userService = userService;
        this.currentUserService = currentUserService;
        this.bookService = bookService;
    }

    [HttpGet]
    [Authorize(Policy = "Admin")]
    public ActionResult<IEnumerable<User>> Get()
    {
        return userService.Get();
    }

    // [HttpGet("me")]
    // public ActionResult<User> GetMyProfile()
    // {
    //     var currentUser = currentUserService.GetCurrentUser();
    //     return currentUser != null ? Ok(currentUser) : Unauthorized();
    // }

    [HttpGet("{id}")]
    public ActionResult<User> Get(int id)
    {
        var currentUser = currentUserService.GetCurrentUser();
        if (currentUser == null)
            return Unauthorized();

        // רק אדמין או המשתמש עצמו יכולים לראות את הפרופיל
        if (currentUser.Role != "Admin" && currentUser.Id != id)
            return Forbid();

        var user = userService.Get(id);
        return user ?? (ActionResult<User>)NotFound();
    }

    [HttpGet("me")]
    public ActionResult<User> GetMyProfile()
    {
        var currentUser = currentUserService.GetCurrentUser();
        return currentUser != null ? Ok(currentUser) : Unauthorized();
    }

    [HttpPost]
    [Authorize(Policy = "Admin")]
    public ActionResult Post(User newUser)
    {
        var newId = userService.Insert(newUser);
        return newId != -1 ? CreatedAtAction(nameof(Post), new { Id = newId }) : BadRequest();
    }

    [HttpPut("{id}")]
    public ActionResult Put(int id, User newUser)
    {
        var currentUser = currentUserService.GetCurrentUser();
        if (currentUser == null)
            return Unauthorized();

        // רק אדמין או המשתמש עצמו יכולים לעדכן
        if (currentUser.Role != "Admin" && currentUser.Id != id)
            return Forbid();

        if (currentUser.Role != "Admin")
        {
            var existingUser = userService.Get(id);
            if (existingUser == null)
                return NotFound();

            // משתמש רגיל לא יכול לשנות את ה-Role
            if (newUser.Role != existingUser.Role)
            {
                return BadRequest(new { message = "אינך מורשה לשנות את סוג המשתמש (Role)." });
            }
        }



        return userService.Update(id, newUser) ? NoContent() : BadRequest();
    }


    [HttpDelete("{id}")]
    [Authorize(Policy = "Admin")]
    public ActionResult Delete(int id)
    {
        bookService.DeleteBooksByUserId(id);

        return userService.Delete(id) ? Ok() : NotFound();
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        currentUserService.ClearUser();
        return Ok(new { message = "Logged out successfully" });
    }
}
