using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC3AppCodeFirst.Controllers
{
    using BlogsDomainEFCodeFirst;

    public class BlogController : Controller
    {
        private BlogContext _context = new BlogContext();

        //
        // GET: /Blog/

        public ActionResult Index()
        {
            using (var db = new BlogContext())
            {
                return View(_context.Blogs.ToList());
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Blog newBlog)
        {
            try
            {
                using (_context)
                {
                    _context.Blogs.Add(newBlog);
                    _context.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        public ActionResult Edit(int id)
        {
            using (_context)
            {
                return View(_context.Blogs.Find(id));
            }
        }

        [HttpPost]
        public ActionResult Edit(int id, Blog blog)
        {
            try
            {
                _context.Entry(blog).State = System.Data.EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult EditwithConcurrencyCheck(int id, Blog blog, string originalName)
        {
            try
            {
                _context.Entry(blog).State = System.Data.EntityState.Modified;

                // Concurrency Check with Blogger name. Change the Edit Get view to send original Blogger name as an hidden Field
                _context.Entry(blog).Property(b => b.BloggerName).OriginalValue = originalName;


                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        public ActionResult Delete(int id)
        {
            using (_context)
            {
                return View(_context.Blogs.Find(id));
            }
        }
        [HttpPost]
        public ActionResult Delete(int id, Blog blog)
        {
            try
            {
                using (_context)
                {
                    _context.Entry(blog).State = System.Data.EntityState.Deleted;
                    _context.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
