using Microsoft.AspNetCore.Mvc;
using Project.Models;
using Project.Services;
using Project.Interfaces;
using System.Text.Json;

namespace Project.Controllers;

[ApiController]
[Route("[controller]")]
public class BookController : ControllerBase
{
     private IBookService bookService;


public BookController(IBookService bookService)
    {
        this.bookService = bookService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Book>> Get()
    {
        return bookService.Get();
    }

    [HttpGet("{id}")]
    public ActionResult<Book> Get(int id)
    {
        var book = bookService.Get(id);
        if (book == null)
            return NotFound();

        return book;
    }
    
    [HttpPost]
    public ActionResult Post(Book newBook)
    {
        Console.WriteLine(JsonSerializer.Serialize(newBook)); // הדפסת מה שמתקבל

        var newId = bookService.Insert(newBook);
        if (newId == -1)
        {
            return BadRequest("Failed to insert book.");
        }

        return CreatedAtAction(nameof(Post), new { Id= newId});
    }

     
    [HttpPut("{id}")]
    public ActionResult Put(int id, Book newBook)
    {
        if (bookService.Update(id, newBook))
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
        if (bookService.Delete(id))
            return Ok();
            
        return NotFound();
    }   
   
}
