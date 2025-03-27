using Microsoft.AspNetCore.Mvc;
using Project.Models;
using Project.Interfaces;
using System.Text.Json;
using Microsoft.Extensions.Hosting;

namespace Project.Services.BookServiceConstJson;

public  class BookServiceConstJson :IBookService
{
      List<Book> Books { get; }
        private static string fileName = "book.json";
        private string filePath;

         public BookServiceConstJson(IHostEnvironment  env)
        {
            filePath = Path.Combine(env.ContentRootPath, "Data", fileName);

 if (!File.Exists(filePath))
        {
            Console.WriteLine("⚠️ JSON file not found!");
            Books = new List<Book>(); // יצירת רשימה ריקה במקרה שאין קובץ
            return;
        }
            using (var jsonFile = File.OpenText(filePath))
            {
                Books = JsonSerializer.Deserialize<List<Book>>(jsonFile.ReadToEnd(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                
                 Console.WriteLine($"Loaded JSON: {Books}");
            }
        }

        private void saveToFile()
        {
            File.WriteAllText(filePath, JsonSerializer.Serialize(Books));
        }
    // private  List<Books> list;
    

    // public BookServiceConstJson()
    // {
    //     list = new List<Books>
    //     {
    //         new Books { Id = 1, Name = "emuna",Author="hachazon Ish" },
    //         new Books { Id = 2, Name = "leshaer yeudi", Author="rev itzchak zilber" }
    //     };
    // }

   
        public List<Book> Get() => Books;

        public Book Get(int id) => Books.FirstOrDefault(b =>b.Id == id);

        public int Insert(Book book)
        {
            book.Id = Books.Count()+1;
            Books.Add(book);
            saveToFile();
            return book.Id;
        }

        public bool Delete(int id)
        {
            var book = Get(id);
            if (book is null)
                return false;

            Books.Remove(book);
            saveToFile();
            return true;
        }

        public bool Update(int id,Book newBook)
        {
             if (newBook == null 
            || string.IsNullOrWhiteSpace(newBook.Name)
            || newBook.Id != id)
        {
            return false;
        }
            var index = Books.FindIndex(b => b.Id == newBook.Id);
            if (index == -1)
                return false;

            Books[index] = newBook;
            saveToFile();
            return true;
        }

   

    public int Count => Books.Count();
   
}


public static class BookUtilities
{
    public static void AddBookJson(this IServiceCollection services)
    {
        services.AddSingleton<IBookService, BookServiceConstJson>();

    }
}