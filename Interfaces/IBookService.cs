using Microsoft.AspNetCore.Mvc;
using Lesson3.Models;

namespace Lesson3.Interfaces;

public interface IBookService 
{
    List<Books> Get();

    Books Get(int id);
    
    int Insert(Books newBook);
     
    bool Update(int id, Books newBook);

    bool Delete(int id);
   
}