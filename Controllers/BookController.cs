using Microsoft.AspNetCore.Mvc;
using Lesson3.Models;
using Lesson3.Services;
using Lesson3.Interfaces;

namespace Lesson3.Controllers;

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
    public ActionResult<IEnumerable<Books>> Get()
    {
        return bookService.Get();
    }

    [HttpGet("{id}")]
    public ActionResult<Books> Get(int id)
    {
        var book = bookService.Get(id);
        if (book == null)
            return NotFound();

        return book;
    }
    
    [HttpPost]
    public ActionResult Post(Books newBook)
    {
        var newId = bookService.Insert(newBook);
        if (newId == -1)
        {
            return BadRequest();
        }

        return CreatedAtAction(nameof(Post), new { Id= newId});
    }

     
    [HttpPut("{id}")]
    public ActionResult Put(int id, Books newBook)
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
