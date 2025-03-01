using Microsoft.AspNetCore.Mvc;
using lesson2.Models;
using lesson2.Services;

namespace lesson2.Controllers;

[ApiController]
[Route("[controller]")]
public class BookController : ControllerBase
{

    [HttpGet]
    public ActionResult<IEnumerable<Books>> Get()
    {
        return BookService.Get();
    }

    [HttpGet("{id}")]
    public ActionResult<Books> Get(int id)
    {
        var book = BookService.Get(id);
        if (book == null)
            return NotFound();

        return book;
    }
    
    [HttpPost]
    public ActionResult Post(Books newBook)
    {
        var newId = BookService.Insert(newBook);
        if (newId == -1)
        {
            return BadRequest();
        }

        return CreatedAtAction(nameof(Post), new { Id= newId});
    }

     
    [HttpPut("{id}")]
    public ActionResult Put(int id, Books newBook)
    {
        if (BookService.Update(id, newBook))
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
        if (BookService.Delete(id))
            return Ok();
            
        return NotFound();
    }   
   
}
