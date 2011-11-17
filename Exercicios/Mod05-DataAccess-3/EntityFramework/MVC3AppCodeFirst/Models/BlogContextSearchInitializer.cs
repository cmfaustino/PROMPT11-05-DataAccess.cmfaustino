namespace MVC3AppCodeFirst.Models
{
    using System.Data.Entity;

    using BlogsDomainEFCodeFirst;

    public class BlogContextSearchInitializer : DropCreateDatabaseIfModelChanges<BlogContextSearch>
    {
        protected override void Seed(BlogContextSearch context)
        {
            Initializer.GetBlogs().ForEach(b => context.Blogs.Add(b));
            base.Seed(context);
        }
    }
}