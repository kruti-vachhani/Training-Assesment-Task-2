using System.ComponentModel.DataAnnotations;

namespace UserManagement.Models.ViewModels;

public class CommentViewModel
{
    public int Id { get; set; }
    public int BlogId { get; set; }
    public int UserId { get; set; }
    public string UserName{get; set;}

    [Required(ErrorMessage ="Comment is Required")]
    public string Comment { get; set; }
}
