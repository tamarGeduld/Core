namespace Project.Models;

public class Book
{
    public int Id { get; set; }
    public string Name { get; set; }

    public string Author { get; set; }
    public string UserId { get; set; }

}
public class BookDto
{
    public int Id { get; set; }
    public string Name { get; set; }

    public string Author { get; set; }
}

