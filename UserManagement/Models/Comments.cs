using System.ComponentModel.DataAnnotations.Schema;

namespace UserManagement.Models;

public class Comments
{
    public int Id { get; set; }

    [ForeignKey("Blogs")]
    public int BlodId { get; set; }

    [ForeignKey("Persons")]
    public int PersonId { get; set; }
    public string Name { get; set; }
    public string Comment { get; set; }

     public Persons Persons{ get; set; }
     public Blogs Blogs { get; set; }
}
