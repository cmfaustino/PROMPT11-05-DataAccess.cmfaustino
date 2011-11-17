namespace BlogsDomainEFCodeFirst
{
    using System;
    using System.Collections.Generic;

    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime DateCreated { get; set; }
        public string Content { get; set; }
        
        public int? BlogId { get; set; }

        //[ForeignKey("BlogId")]
        public virtual Blog Blog { get; set; }

        // Unconventional foreign key to Blog
        //public int FKBlogId { get; set; }

        //public Person CreatedBy { get; set; }
        //public Person UpdatedBy { get; set; }
        
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Tag> Tags { get; set; }

        //public string Dummy { get; set; }
    }
}