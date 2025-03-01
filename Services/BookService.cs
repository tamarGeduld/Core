using Microsoft.AspNetCore.Mvc;
using lesson2.Models;

namespace lesson2.Services;

public static class BookService 
{
    private static List<Books> list;

    static BookService()
    {
        list = new List<Books>
        {
            new Books { Id = 1, Name = "emuna",Author="hachazon Ish" },
            new Books { Id = 2, Name = "leshaer yeudi", Author="rev itzchak zilber" }
        };
    }

    public static List<Books> Get()
    {
        return list;
    }

    public static Books Get(int id)
    {
        var books = list.FirstOrDefault(p => p.Id == id);
        return books;
    }
    
    public static int Insert(Books newBook)
    {
        if (newBook == null 
            || string.IsNullOrWhiteSpace(newBook.Name))
            return -1;

        int maxId = list.Max(p => p.Id);
        newBook.Id = maxId + 1;
        list.Add(newBook);

        return newBook.Id;
    }

     
    public static bool Update(int id, Books newBook)
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

      public static bool Delete(int id)
    {
        var book = list.FirstOrDefault(p => p.Id == id);
        if (book == null)
            return false;

        var index = list.IndexOf(book);
        list.RemoveAt(index);

        return true;
    }   
   
}
