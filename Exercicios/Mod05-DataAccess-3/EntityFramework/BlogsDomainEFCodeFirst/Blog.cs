//#define CONFIGURATION
//#define DATA_ANNOTTATIONS
#define CODE_FIRST_CONVENTIONS

namespace BlogsDomainEFCodeFirst
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

#if CONFIGURATION || CODE_FIRST_CONVENTIONS
    public class Blog
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string BloggerName { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public DateTime DateCreated { get; set; }
        public string Description { get; set; }

        public Privacy Privacy
        {
            get { return (Privacy)PrivacyId; }
            set { PrivacyId = (int)value; }
        }

        internal int PrivacyId
        {
            get;
            set; 
        }
    }

    public enum Privacy
    {
        Public,
        Private
    }

#endif



    #if DATA_ANNOTTATIONS
    [Table("InternalBlogs")]
    public class Blog
    {
        public int Id { get; set; }

        //[Key]
        //public int PrimaryTrackingKey { get; set; }

        [Required(ErrorMessage = "The Title is required")]
        //[ConcurrencyCheck]
        public string Title { get; set; }

        [MaxLength(10, ErrorMessage = "BloggerName must be 10 characters or less"), MinLength(5)]
        public string BloggerName { get; set; }
        public virtual ICollection<Post> Posts { get; set; }


        [NotMapped]
        public string BlogCode
        {
            get
            {
                return Title.Substring(0, 1) + ":" + BloggerName.Substring(0, 1);
            }
        }

        public BlogDetails BlogDetail { get; set; }

        public DateTime DateCreated { get; set; }

        [Timestamp]
        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        //[Column(TypeName = "timestamp")]
        public Byte[] TimeStamp { get; set; }
    }

    //[ComplexType]
    public class BlogDetails
    {
        public int Id { get; set; }
        public DateTime? DateCreated { get; set; }
        [MaxLength(250)]
        [Column("BlogDescription", TypeName="ntext")]
        public string Description { get; set; }
    }
#endif
}