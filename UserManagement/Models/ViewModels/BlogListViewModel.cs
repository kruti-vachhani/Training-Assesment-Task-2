namespace UserManagement.Models.ViewModels;

public class BlogListViewModel
{
    public int BlogId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime PostedAt { get; set; }
    public List<string> Tags { get; set; }
}
