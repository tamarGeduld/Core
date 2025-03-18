using Microsoft.AspNetCore.Mvc;
using Lesson3.Models;
using Lesson3.Interfaces;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
namespace Lesson3.Services.BookServiceConstJson;

public  class UserServiceConstJson :IUserService
{
      List<Users> Users { get; }
        private static string fileName = "Users.json";
        private string filePath;

         public UserServiceConstJson(IHostEnvironment  env)
        {
            filePath = Path.Combine(env.ContentRootPath, "Data", fileName);

 if (!File.Exists(filePath))
        {
            Console.WriteLine("⚠️ JSON file not found!");
            Users = new List<Users>(); // יצירת רשימה ריקה במקרה שאין קובץ
            return;
        }
            using (var jsonFile = File.OpenText(filePath))
            {
                Users = JsonSerializer.Deserialize<List<Users>>(jsonFile.ReadToEnd(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                
                 Console.WriteLine($"Loaded JSON: {Users}");
            }
        }

        private void saveToFile()
        {
            File.WriteAllText(filePath, JsonSerializer.Serialize(Users));
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

   
        public List<Users> Get() => Users;

        public Users Get(int id) => Users.FirstOrDefault(b =>b.Id == id);

        public int Insert(Users user)
        {
            user.Id = Users.Count()+1;
            Users.Add(user);
            saveToFile();
            return user.Id;
        }

        public bool Delete(int id)
        {
            var user = Get(id);
            if (user is null)
                return false;

            Users.Remove(user);
            saveToFile();
            return true;
        }

        public bool Update(int id,Users newUser)
        {
             if (newUser == null 
            || string.IsNullOrWhiteSpace(newUser.Name)
            || newUser.Id != id)
        {
            return false;
        }
            var index = Users.FindIndex(b => b.Id == newUser.Id);
            if (index == -1)
                return false;

            Users[index] = newUser;
            saveToFile();
            return true;
        }

   

    public int Count => Users.Count();
   
}


public static class UserUtilities
{
    public static void AddUserConst(this IServiceCollection services)
    {
        services.AddSingleton<IUserService, UserServiceConstJson>();

    }
}