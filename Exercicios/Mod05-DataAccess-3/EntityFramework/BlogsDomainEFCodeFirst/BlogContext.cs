#define CONFIGURATION

namespace BlogsDomainEFCodeFirst
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    using BlogsDomainEFCodeFirst.EFConfiguration;

    public class BlogContextSearch : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Comment> Comments { get; set; }
    }


    public class BlogContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }


        public DbSet<Person> People { get; set; }
        public DbSet<PowerUser> PowerUsers { get; set; }
        public DbSet<RegularUser> RegularUsers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Conventions.Remove<IncludeMetadataConvention>();
            //#if CONFIGURATION

            //modelBuilder.Entity<Blog>().ToTable("InternalBlogs", "dbo");

            //modelBuilder.Entity<Blog>().HasKey(b => new { b.Id, b.Title });
            //modelBuilder.Entity<Blog>().Property(p => p.Title).HasColumnName("BlogTitle").IsMaxLength();
            //modelBuilder.Entity<Blog>().Property(p => p.DateCreated)
            //    .HasColumnName("CreatedDate")
            //    .HasColumnOrder(1)
            //    .HasColumnType("date");

            //modelBuilder.Entity<Blog>().Map(
            //    mca =>
            //    {
            //        mca.Properties(b => new { b.Id, b.Title, b.BloggerName });
            //        mca.ToTable("InternalBlogs");
            //    }).Map(
            //            mca =>
            //            {
            //                mca.Properties(b => new { b.DateCreated });
            //                mca.ToTable("BlogDetails");
            //            });


            //modelBuilder.ComplexType<BlogDetails>();

            // If Post does not have a BlogId property, this configuration is required
                // so that all posts are bound to one Blog
            modelBuilder.Entity<Post>().HasRequired(p => p.Blog);
            modelBuilder.Entity<Person>()
                    .Map<PowerUser>(m => m.Requires("Type").HasValue("PU"))
                    .Map <RegularUser>(m => m.Requires("Type").HasValue("RU"));

            // To support the unconventional foreign key FKBlogId in Post this configuration is required
            modelBuilder.Entity<Post>()
                .HasOptional(p => p.Blog)
                .WithMany(b => b.Posts)
                //.HasForeignKey(p => p.FKBlogId)
                ;

            modelBuilder.Entity<Post>()
                .HasMany(p => p.Tags)
                .WithMany(t => t.Posts)
                .Map(mc =>
                {
                    mc.ToTable("PostJoinTag");
                    mc.MapLeftKey("PostId");
                    mc.MapRightKey("TagId");
                });


            // Configuration with entity configuration
            //modelBuilder.Configurations.Add(new BlogConfiguration());
            //#endif

        }
    }
}