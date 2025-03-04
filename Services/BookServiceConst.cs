using Microsoft.AspNetCore.Mvc;
using Lesson3.Models;
using Lesson3.Interfaces;

namespace Lesson3.Services;

public  class BookServiceConst :IBookService
{
    private  List<Books> list;

    public BookServiceConst()
    {
        list = new List<Books>
        {
            new Books { Id = 1, Name = "emuna",Author="hachazon Ish" },
            new Books { Id = 2, Name = "leshaer yeudi", Author="rev itzchak zilber" }
        };
    }

    public  List<Books> Get()
    {
        return list;
    }

    public  Books Get(int id)
    {
        var books = list.FirstOrDefault(p => p.Id == id);
        return books;
    }
    
    public  int Insert(Books newBook)
    {
        if (newBook == null 
            || string.IsNullOrWhiteSpace(newBook.Name))
            return -1;

        int maxId = list.Max(p => p.Id);
        newBook.Id = maxId + 1;
        list.Add(newBook);

        return newBook.Id;
    }

     
    public  bool Update(int id, Books newBook)
    {
        if (newBook == null 
            || string.IsNullOrWhiteSpace(newBook.Name)
            || newBook.Id != id)
        {
            return false;
        }
        
        var book = list.FirstOrDefault(p => p.Id == id);
        if (book == null)
            return false;

        book.Name = newBook.Name;
        book.Author = newBook.Author;
        
        /*var index = list.IndexOf(pizza);
        list[index] = newPizza;*/

        return true;
    }

      public  bool Delete(int id)
    {
        var book = list.FirstOrDefault(p => p.Id == id);
        if (book == null)
            return false;

        var index = list.IndexOf(book);
        list.RemoveAt(index);

        return true;
    }   
   
}


public static class BookUtilities
{
    public static void AddBookConst(this IServiceCollection services)
    {
        services.AddSingleton<IBookService, BookServiceConst>();

    }
}