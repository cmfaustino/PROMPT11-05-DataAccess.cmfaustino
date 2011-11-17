namespace BlogsDomainEFCodeFirst.EFConfiguration
{
    using System.Data.Entity.ModelConfiguration;

    public class BlogConfiguration : EntityTypeConfiguration<Blog>
    {
        public BlogConfiguration()
        {
            ToTable("Blogs");
            //HasKey(b => b.Id);
        }
    }
}