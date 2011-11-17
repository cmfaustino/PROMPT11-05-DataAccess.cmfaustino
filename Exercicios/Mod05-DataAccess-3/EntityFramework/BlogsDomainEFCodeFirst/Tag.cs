namespace BlogsDomainEFCodeFirst
{
    using System.Collections.Generic;

    public class Tag
    {
        public int TagId { get; set; }
        public string Name { get; set; }
        public ICollection<Post> Posts { get; set; }
    }
}