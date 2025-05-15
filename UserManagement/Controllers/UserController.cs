using System.Security.Claims;
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

        Blogs? blog = _context.Blogs.FirstOrDefault(d => d.Id == blogId && !d.IsDeleted);

        int userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

        IQueryable<Comments> commentQuery = _context.Comments.OrderBy(c => c.Id).Where(c => c.BlodId == blogId && c.PersonId == userId);

        List<CommentViewModel>? commentList = commentQuery
       .Select(c => new CommentViewModel
       {
           BlogId = c.Id,
           UserName = c.Persons.Firstname + " " + c.Persons.Lastname,
           Comment = c.Comment
       }).ToList();

        BlogListViewModel? blogListViewModel = new BlogListViewModel
        {
            BlogId = blog.Id,
            Title = blog.Title,
            Content = blog.Content,
            Tags = blog.Tags,
            PostedAt = blog.CreatedAt,
            Comments = commentList
        };

        return View(blogListViewModel);
    }

    public IActionResult OpenCommentModal()
    {
        CommentViewModel? commentViewModel = new CommentViewModel();
        return PartialView("_AddCommentPartialView", commentViewModel);
    }

    public IActionResult AddComment(CommentViewModel commentViewModel)
    {
        int userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

        Persons? user = _context.Persons.FirstOrDefault(u => u.Id == userId);

        Comments? comment = new Comments
        {
            Comment = commentViewModel.Comment.Trim(),
            PersonId = userId,
            BlodId = commentViewModel.BlogId,
            Name = user.Firstname + " " + user.Lastname
        };

        if (comment != null)
        {
            _context.Comments.Add(comment);
            _context.SaveChanges();

            return Json(new { success = true, message = "Comment Created Successfully." });
        }

        return Json(new { success = false, message = "Error in Adding Comment." });
    }
}
