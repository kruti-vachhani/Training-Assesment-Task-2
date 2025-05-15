namespace UserManagement.Models;

public class Persons
{
    public int Id { get; set; }

    public string Firstname { get; set; }

    public string Lastname { get; set; }

    public string Username { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public string Role { get; set; }

      public ICollection<Blogs> Blogs { get; set; }
      public ICollection<Comments> Comments { get; set; }
      

}
