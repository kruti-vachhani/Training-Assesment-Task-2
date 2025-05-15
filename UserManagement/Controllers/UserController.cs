using Microsoft.AspNetCore.Mvc;
using UserManagement.Models;
using UserManagement.Models.ViewModels;

namespace UserManagement.Controllers;

public class UserController : Controller
{
    private readonly UserManagementDbContext _context;

    public UserController(UserManagementDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        IQueryable<Blogs> blogListQuery = _context.Blogs.OrderBy(b => b.Id).Where(b => !b.IsDeleted);

        List<BlogListViewModel>? blogList = blogListQuery
        .Select(b => new BlogListViewModel
        {
            BlogId = b.Id,
            Title = b.Title,
            Content = b.Content,
            Tags = b.Tags,
            PostedAt = b.CreatedAt
        }).ToList();

        return View(blogList);
    }
}
