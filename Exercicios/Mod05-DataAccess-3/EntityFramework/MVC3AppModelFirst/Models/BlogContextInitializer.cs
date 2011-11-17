namespace MVC3AppModelFirst.Models
{
    using System.Data.Entity;

    using BlogsDomainEFModelFirst;

    public class BlogsContainerInitializer : DropCreateDatabaseIfModelChanges<BlogsContainer>
    {
        protected override void Seed(BlogsContainer context)
        {
            Initializer.GetBlogs().ForEach(b => context.Blogs.Add(b));
            //Initializer.GetUsers().ForEach(u => context.People.Add(u));
            base.Seed(context);
        }
    }
}