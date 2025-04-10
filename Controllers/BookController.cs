using Microsoft.AspNetCore.Mvc;
using Project.Models;
using Project.Services;
using Project.Interfaces;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;


namespace Project.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class BookController : ControllerBase
{
    private readonly IBookService bookService;
    private readonly ICurrentUserService currentUserService;

    public BookController(IBookService bookService, ICurrentUserService currentUserService)
    {
        this.bookService = bookService;
        this.currentUserService = currentUserService;
    }
    [HttpGet]
    public ActionResult<IEnumerable<Book>> Get()
    {
        var currentUser = currentUserService.GetCurrentUser();
        if (currentUser == null) return Unauthorized();

        var books = bookService.Get();

        // אם המשתמש הוא לא אדמין – סנן רק את הספרים שלו
        if (currentUser.Role != "Admin")
        {
            books = books.Where(b => b.UserId == currentUser.Id.ToString()).ToList();
        }

        return Ok(books);
    }

    [HttpGet("{id}")]
    public ActionResult<Book> Get(int id)
    {
        var currentUser = currentUserService.GetCurrentUser();
        if (currentUser == null) return Unauthorized();

        var book = bookService.Get(id);
        if (book == null)
            return NotFound();

        if (currentUser.Role != "Admin" && book.UserId != currentUser.Id.ToString())
            return Forbid(); // לא רשאי

        return Ok(book);
    }

    [HttpPost]
    public ActionResult Post(BookDto newBookDto)
    {
        var currentUser = currentUserService.GetCurrentUser();
        if (currentUser == null)
            return Unauthorized();

        var book = new Book
        {
            Id = newBookDto.Id,
            Name = newBookDto.Name,
            Author = newBookDto.Author,
            UserId = currentUser.Id.ToString()
        };

        var newId = bookService.Insert(book);
        if (newId == -1)
            return BadRequest("Failed to insert book.");

        return CreatedAtAction(nameof(Post), new { Id = newId });
    }

   
    [HttpPut("{id}")]
    public ActionResult Put(int id, BookDto updatedBookDto)
    {
        var currentUser = currentUserService.GetCurrentUser();
        if (currentUser == null)
            return Unauthorized();

        var existingBook = bookService.Get(id);
        if (existingBook == null)
            return NotFound();

        // רק אם המשתמש אינו אדמין - ודא שהוא הבעלים של הספר
        if (currentUser.Role != "Admin" && existingBook.UserId != currentUser.Id.ToString())
            return Forbid();

        // עדכון רק של השדות הרלוונטיים, בלי לשנות את הבעלות
        existingBook.Name = updatedBookDto.Name;
        existingBook.Author = updatedBookDto.Author;

        if (bookService.Update(id, existingBook))
            return NoContent();

        return BadRequest("Update failed");
    }


    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        var currentUser = currentUserService.GetCurrentUser();
        if (currentUser == null) return Unauthorized();

        var book = bookService.Get(id);
        if (book == null) return NotFound();

        if (currentUser.Role != "Admin" && book.UserId != currentUser.Id.ToString())
            return Forbid();

        return bookService.Delete(id) ? Ok() : BadRequest();
    }
}
