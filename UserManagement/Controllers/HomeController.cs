using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Models;
using UserManagement.Models.ViewModels;

namespace UserManagement.Controllers;

[Authorize(Roles = "User")]
public class HomeController : Controller
{
    private readonly UserManagementDbContext _context;

    public HomeController(UserManagementDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        string? token = Request.Cookies["AuthToken"];

        if (string.IsNullOrEmpty(token))
        {
            return RedirectToAction("Index", "Auth");
        }

        IQueryable<Blogs> blogListQuery = _context.Blogs.Where(b => !b.IsDeleted);

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

    public IActionResult OpenCreateBlogModal()
    {
        BlogViewModel blogViewModel = new BlogViewModel();
        return PartialView("_AddEditBlogPartialView", blogViewModel);
    }

    [HttpPost]
    public IActionResult CreateBlog(BlogViewModel blogViewModel)
    {
        Blogs? blogs = new Blogs
        {
            Title = blogViewModel.Title,
            Content = blogViewModel.Content,
            Tags = blogViewModel.Tags
        };

        if (blogs != null)
        {
            _context.Blogs.Add(blogs);
            _context.SaveChanges();

            return Json(new { success = true, message = "Blog Created Successfully." });
        }

        return Json(new { success = false, message = "Error-Blog can't created." });
    }


    public IActionResult Logout()
    {
        int userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
        Response.Cookies.Delete("AuthToken");
        TempData["success"] = "Logout successful!";

        return RedirectToAction("Index", "Auth");
    }

}
