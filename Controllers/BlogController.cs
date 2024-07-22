using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC4.Models;

namespace MVC4.Controllers
{
    public class BlogController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BlogController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }


        Blog_CRUDContext Blog_CRUDContext = new Blog_CRUDContext();
        public IActionResult Index(string? tin)
        {
            List<Blog> blogs = new List<Blog>();

            blogs = Blog_CRUDContext.Blogs.Include(b => b.Category).Where(b => (String.IsNullOrEmpty(tin)) || b.Title.Contains(tin)).ToList();
            Console.WriteLine(tin);

            ViewBag.BlogList = blogs;
            ViewBag.tin = tin;
            return View("BlogList");
        }

        public IActionResult Detail(int id)
        {
            var b = Blog_CRUDContext.Blogs.Where(b => b.BlogId == id).SingleOrDefault();
            var location = b.Location;
            var larr = b.Location.Split(",");

            Console.WriteLine(b.Status);

            ViewBag.Blog = b;
            return View("BlogDetail");
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, string title, string shortDescription, string content, IFormFile image, string[] location, bool publicStatus, int categoryId, DateTime datepublic)
        {
            var blog = Blog_CRUDContext.Blogs.Find(id);
            if (blog == null)
            {
                return NotFound();
            }
            Console.WriteLine(publicStatus);
            // Cập nhật các thuộc tính của blog
            blog.Title = title;
            blog.CategoryId = categoryId;
            blog.Status = publicStatus;
            blog.Description = content;
            blog.ShortDescription = shortDescription;
            blog.PublicDate = datepublic;
            blog.Location = string.Join(", ", location);
            Console.WriteLine(image);
            if (image != null && image.Length > 0)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Img", image.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }
                blog.Img = "/Img/" + image.FileName;
            }

            Blog_CRUDContext.Entry(blog).State = EntityState.Modified;

            // Lưu thay đổi vào cơ sở dữ liệu
            Blog_CRUDContext.SaveChanges();

            return Detail(id);
        }

        public IActionResult Delete(int id)
        {
            var b = Blog_CRUDContext.Blogs.Where(b=>b.BlogId==id).SingleOrDefault();
            if (b != null)
            {
                Blog_CRUDContext.Remove(b);
                Blog_CRUDContext.SaveChanges();
            }

            if (b != null)
            {
                b.Status = false;
                Blog_CRUDContext.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Create()
        {
            return View("BlogCreate");
        }

        [HttpPost]
        public async Task<IActionResult> CreateBlog(string title, string shortDescription, string content, IFormFile image, string[] location, bool publicStatus, int categoryId, DateTime datepublic)
        {
            var blog = new Blog();
            if (blog == null)
            {
                return NotFound();
            }
            Console.WriteLine(publicStatus);

            blog.Title = title;
            blog.CategoryId = categoryId;
            blog.Status = publicStatus;
            blog.Description = content;
            blog.ShortDescription = shortDescription;
            blog.PublicDate = datepublic;
            blog.Location = string.Join(", ", location);
            Console.WriteLine(image);
            if (image != null && image.Length > 0)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Img", image.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }
                blog.Img = "/Img/" + image.FileName;
            }
            Blog_CRUDContext.Add(blog);


            Blog_CRUDContext.SaveChanges();

            return Detail(blog.BlogId);
        }

    }
}
