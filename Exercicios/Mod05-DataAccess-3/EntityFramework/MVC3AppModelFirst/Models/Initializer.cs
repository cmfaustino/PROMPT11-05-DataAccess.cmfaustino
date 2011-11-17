namespace MVC3AppModelFirst.Models
{
    using System.Collections.Generic;

    using BlogsDomainEFModelFirst;

    public static class Initializer
    {
        public static List<Blog> GetBlogs()
        {
            //return new List<Blog>
            //    {
            //        new Blog
            //            {
            //                BloggerName = "Julie",
            //                Title = "My Code First Blog",
            //                DateCreated = System.DateTime.Now,
            //                Posts =
            //                    new List<Post>
            //                        {
            //                            new Post
            //                                {
            //                                    Title = "ForeignKeyAttribute Annotation",
            //                                    DateCreated = new System.DateTime(2011, 3, 15),
            //                                    Content = "Mark navigation property with ForeignKey"
            //                                },
            //                            new Post
            //                                {
            //                                    Title = "Working with the ChangeTracker",
            //                                    DateCreated = System.DateTime.Now,
            //                                    Content =
            //                                        "You can use db.Entry to get to state for a single entry or"
            //                                        + "db.ChangeTracker.Entries to work with all of the tracked entries."
            //                                }
            //                        }
            //            },
            //        new Blog
            //            {
            //                BloggerName = "Ingemaar",
            //                Title = "My Life as a Blog",
            //                DateCreated = System.DateTime.Now.AddDays(1)
            //            },
            //        new Blog
            //            {
            //                BloggerName = "Sampson",
            //                Title = "Tweeting for Dogs",
            //                DateCreated = System.DateTime.Now.AddDays(2)
            //            }
            //    };
            return null;
        }

        //public static List<Person> GetUsers()
        //{
        //    return new List<Person>
        //        {
        //            new PowerUser { Name = "PowerUser1", UserRights = "all" },
        //            new PowerUser { Name = "PowerUser2", UserRights = "some" },
        //            new RegularUser { Name = "RegularUser1", Nickname = "mokey" },
        //            new RegularUser { Name = "RegularUser1", Nickname = "clown" }
        //        };
        //}
    }
}