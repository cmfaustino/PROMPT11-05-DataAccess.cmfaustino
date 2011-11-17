using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mod05_ChelasDAL.Mappers;
using Mod05_ChelasDAL.Metadata;
using Mod5_DomainModel;
using Mod5_DomainModel.DomainModelMappers;

namespace Mod05_ChelasDAL.Test
{
    [TestClass]
    public class BlogsTest
    {
        protected readonly string ConnectionStringName = "TestConnection";

        [TestInitialize()]
        public void MyTestInitialize()
        {
            var connectionString = ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = "DELETE FROM Blogs";
                cmd.ExecuteNonQuery();
            }        
        }

        [TestMethod]
        public void Can_Create_Blog_With_Mapper()
        {
            var connectionString = ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            { 
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                { 
                    var metaDataStore = new MetaDataStore();
                    metaDataStore.BuildTableInfoFor<Blog>();
                    var identityMap = new IdentityMap();
                    var hydrater = new EntityHydrater(metaDataStore, identityMap);
                    BlogMapper blogMapper = new BlogMapper(connection, transaction, metaDataStore, hydrater, identityMap);

                    Blog blog = new Blog { Name = "PROMPT", Description = "Módulo 5 - Plataformas e modelos de acesso a dados" };
                    blog = blogMapper.Insert(blog);

                    Assert.IsNotNull(blog);
                    Assert.IsTrue(blog.Id > 0);
                    Assert.AreEqual("PROMPT", blog.Name);
                    Assert.AreEqual("Módulo 5 - Plataformas e modelos de acesso a dados", blog.Description);

                    transaction.Commit();
                }
            }
        }

        [TestMethod]
        public void Can_FindAll_Blogs_With_Mapper()
        {
            // Prepare
            var connectionString = ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            { 
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                { 
                    var metaDataStore = new MetaDataStore();
                    metaDataStore.BuildTableInfoFor<Blog>();
                    var identityMap = new IdentityMap();
                    var hydrater = new EntityHydrater(metaDataStore, identityMap);
                    BlogMapper blogMapper = new BlogMapper(connection, transaction, metaDataStore, hydrater, identityMap);

                    for (int i = 0; i < 10; i++)
                    {
                        Blog blog = new Blog { Name = "PROMPT " + i, Description = "Módulo " + i + " - Plataformas e modelos de acesso a dados" };
                        blogMapper.Insert(blog);
                    }
                    transaction.Commit();
                }
            }

            // Test with whole new references
            using (var connection = new SqlConnection(connectionString))
            { 
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                { 
                    var metaDataStore = new MetaDataStore();
                    metaDataStore.BuildTableInfoFor<Blog>();
                    var identityMap = new IdentityMap();
                    var hydrater = new EntityHydrater(metaDataStore, identityMap);
                    BlogMapper blogMapper = new BlogMapper(connection, transaction, metaDataStore, hydrater, identityMap);
                    
                    var allBlogs = blogMapper.FindAll();
                    
                    Assert.AreEqual(10, allBlogs.Count());
                    for (int i = 0; i < 10; i++)
                    {
                        int blogi = i;
                        var blog = allBlogs.SingleOrDefault(b => b.Name == "PROMPT " + blogi);
                        Assert.IsNotNull(blog);
                    }

                    transaction.Commit();
                }
            }
        }
    }
}
