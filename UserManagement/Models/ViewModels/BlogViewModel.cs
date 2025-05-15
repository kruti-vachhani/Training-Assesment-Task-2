using System.ComponentModel.DataAnnotations;

namespace UserManagement.Models.ViewModels;

public class BlogViewModel
{
    public int BlogId { get; set; }

    [Required(ErrorMessage ="Title is Required")]
    public string Title { get; set; }

    [Required(ErrorMessage ="Content is Required")]
    public string Content { get; set; }

    [Required(ErrorMessage ="Tags is Required")]
    public string Tags { get; set; }
}
