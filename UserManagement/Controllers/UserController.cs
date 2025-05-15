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
            Title=b.Title,
            PostedAt = b.CreatedAt
        }).ToList();

        return View(blogList);
    }

    [HttpGet]
    public IActionResult BlogDetails(int blogId)
    {
         if (blogId == 0)
        {
            return RedirectToAction("Index", "User");
        }

        Blogs? blog = _context.Blogs.FirstOrDefault(d => d.Id == blogId);

        string blogTags = string.Join(",", blog.Tags.Select(p => p));

        BlogListViewModel? blogListViewModel = new BlogListViewModel
        {
            BlogId = blog.Id,
            Title = blog.Title,
            Content = blog.Content,
            Tags = blog.Tags,
            PostedAt=blog.CreatedAt
        };

        return View(blogListViewModel);
    }
}
