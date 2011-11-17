namespace BlogsDomainEFCodeFirst
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Without this custom attribute Code First will generate two foreign keys in Blog for person - Person_Id, Person_Id
        //[InverseProperty("CreatedBy")]
        //public List<Post> PostsWritten { get; set; }

        //[InverseProperty("UpdatedBy")]
        //public List<Post> PostsUpdated { get; set; }
    }

    public class RegularUser : Person
    {
        public String Nickname { get; set; }
    }

    public class PowerUser : Person
    {
        [Required]
        public String UserRights { get; set; }
    }
}