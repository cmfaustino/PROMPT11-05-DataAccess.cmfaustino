namespace MVC3AppCodeFirst.Models
{
    using System.Data.Entity;

    using BlogsDomainEFCodeFirst;

    public class BlogContextInitializer : DropCreateDatabaseIfModelChanges<BlogContext>
    {
        protected override void Seed(BlogContext context)
        {
            Initializer.GetBlogs().ForEach(b => context.Blogs.Add(b));
            Initializer.GetUsers().ForEach(u => context.People.Add(u));
            base.Seed(context);
        }
    }
}