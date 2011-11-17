using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC3AppCodeFirst.Controllers
{
    using BlogsDomainEFCodeFirst;

    public class PostController : Controller
    {
        private BlogContext db = new BlogContext();

        //
        // GET: /Default1/

        public ViewResult Index()
        {
            //var posts = db.Posts.Include(p => p.Blog);
            var posts = db.Posts;
            return View(posts.ToList());
        }

        //
        // GET: /Default1/Details/5

        public ViewResult Details(int id)
        {
            Post post = db.Posts.Find(id);
            return View(post);
        }

        //
        // GET: /Default1/Create

        public ActionResult Create()
        {
            ViewBag.BlogId = new SelectList(db.Blogs, "Id", "Title");
            return View();
        } 

        //
        // POST: /Default1/Create

        [HttpPost]
        public ActionResult Create(Post post)
        {
            if (ModelState.IsValid)
            {
                db.Posts.Add(post);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            //ViewBag.BlogId = new SelectList(db.Blogs, "Id", "Title", post.BlogId);
            return View(post);
        }
        
        //
        // GET: /Default1/Edit/5
 
        public ActionResult Edit(int id)
        {
            Post post = db.Posts.Find(id);
            //ViewBag.BlogId = new SelectList(db.Blogs, "Id", "Title", post.BlogId);
            return View(post);
        }

        //
        // POST: /Default1/Edit/5

        [HttpPost]
        public ActionResult Edit(Post post)
        {
            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.BlogId = new SelectList(db.Blogs, "Id", "Title", post.BlogId);
            return View(post);
        }

        //
        // GET: /Default1/Delete/5
 
        public ActionResult Delete(int id)
        {
            Post post = db.Posts.Find(id);
            return View(post);
        }

        //
        // POST: /Default1/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}