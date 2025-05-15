using System.ComponentModel.DataAnnotations.Schema;

namespace UserManagement.Models;

public class Blogs
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public List<string> Tags { get; set; }

    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; }

    [ForeignKey("Persons")]
    public int CreatedBy { get; set; }


    public Persons Persons { get; set; }

    public ICollection<Comments> Comments { get; set; }
}
