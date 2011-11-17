namespace MVC3AppModelFirst.Controllers.Controllers
{
    using System.Data;
    using System.Linq;
    using System.Web.Mvc;

    using BlogsDomainEFModelFirst;

    public class CommentsController : Controller
    {
        private BlogsContainer context = new BlogsContainer();

        //
        // GET: /Comments/

        public ViewResult Index()
        {
            return View(context.Comments.Include("Post").ToList());
        }

        //
        // GET: /Comments/Details/5

        public ViewResult Details(int id)
        {
            Comment comment = context.Comments.Single(x => x.Id == id);
            return View(comment);
        }

        //
        // GET: /Comments/Create

        public ActionResult Create()
        {
            ViewBag.PossiblePosts = context.Posts;
            return View();
        } 

        //
        // POST: /Comments/Create

        [HttpPost]
        public ActionResult Create(Comment comment)
        {
            if (ModelState.IsValid)
            {
                context.Comments.Add(comment);
                context.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.PossiblePosts = context.Posts;
            return View(comment);
        }
        
        //
        // GET: /Comments/Edit/5
 
        public ActionResult Edit(int id)
        {
            Comment comment = context.Comments.Single(x => x.Id == id);
            ViewBag.PossiblePosts = context.Posts;
            return View(comment);
        }

        //
        // POST: /Comments/Edit/5

        [HttpPost]
        public ActionResult Edit(Comment comment)
        {
            if (ModelState.IsValid)
            {
                context.Entry(comment).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PossiblePosts = context.Posts;
            return View(comment);
        }

        //
        // GET: /Comments/Delete/5
 
        public ActionResult Delete(int id)
        {
            Comment comment = context.Comments.Single(x => x.Id == id);
            return View(comment);
        }

        //
        // POST: /Comments/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Comment comment = context.Comments.Single(x => x.Id == id);
            context.Comments.Remove(comment);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}