using Microsoft.AspNetCore.Mvc;
using Project.Models;

namespace Project.Interfaces;

public interface IBookService 
{
    List<Book> Get();

    Book Get(int id);
    
    int Insert(Book newBook);
     
    bool Update(int id, Book newBook);

    bool Delete(int id);
    
    void DeleteBooksByUserId(int userId);

}