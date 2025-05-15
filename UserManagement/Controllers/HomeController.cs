using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Models;
using UserManagement.Models.ViewModels;

namespace UserManagement.Controllers;

[Authorize]
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

        string? role = HttpContext.Items["Role"]?.ToString();

        if (role == "User")
        {
            return RedirectToAction("Index", "User");
        }
        else
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

    [Authorize(Roles = "Admin")]
    public IActionResult OpenCreateBlogModal(int blogId)
    {
        BlogViewModel blogViewModel = new BlogViewModel();

        if (blogId != 0)
        {
            Blogs? blog = _context.Blogs.FirstOrDefault(d => d.Id == blogId);

            string blogTags = string.Join(",", blog.Tags.Select(p => p));

            blogViewModel = new BlogViewModel
            {
                BlogId = blog.Id,
                Title = blog.Title,
                Content = blog.Content,
                Tags = blogTags
            };
        }

        return PartialView("_AddEditBlogPartialView", blogViewModel);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public IActionResult SaveBlog(BlogViewModel blogViewModel)
    {
        if (blogViewModel.BlogId == 0)
        {
            int userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

            List<string>? tags = blogViewModel.Tags.ToString().Split(',').Select(t => t.Trim()).ToList();

            Blogs? blog = new Blogs
            {
                Title = blogViewModel.Title.Trim(),
                Content = blogViewModel.Content.Trim(),
                Tags = tags,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = userId
            };

            if (blog != null)
            {
                _context.Blogs.Add(blog);
                _context.SaveChanges();

                return Json(new { success = true, message = "Blog Created Successfully." });
            }
        }
        else
        {
            Blogs? blog = _context.Blogs.FirstOrDefault(b => b.Id == blogViewModel.BlogId);

            if (blog == null)
            {
                return Json(new { success = false, message = "Blog cannot found" });
            }

            List<string>? tags = blogViewModel.Tags.ToString().Split(',').Select(t => t.Trim()).ToList();

            blog.Title = blogViewModel.Title;
            blog.Content = blogViewModel.Content;
            blog.Tags = tags;

            _context.Blogs.Update(blog);
            _context.SaveChanges();

            return Json(new { success = true, message = "Blog Updated Successfully." });
        }

        return Json(new { success = false, message = "Error-Blog can't created." });
    }


    [Authorize(Roles = "Admin")]
    [HttpGet]
    public IActionResult RetriveBlogList()
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

        return PartialView("_BlogListPartialView", blogList);
    }


    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult DeleteBlog(int blogId)
    {
        Blogs? blog = _context.Blogs.FirstOrDefault(d => d.Id == blogId);
        if (blog == null)
        {
            return Json(new { success = false, message = "Blog not found" });
        }

        blog.IsDeleted = true;
        _context.SaveChanges();

        return Json(new { success = true, message = "Blog Deleted Successfully." });
    }


    public IActionResult Logout()
    {
        int userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
        Response.Cookies.Delete("AuthToken");
        TempData["success"] = "Logout successful!";

        return RedirectToAction("Index", "Auth");
    }

}
