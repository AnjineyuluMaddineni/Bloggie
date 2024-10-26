using Bloggie.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Bloggie.Web.Data
{
    public class BloggieDbContext : DbContext
    {
        public BloggieDbContext(DbContextOptions<BloggieDbContext> options):base(options)
        { 
        }

        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<BlogPostLike> BlogPostLike { get; set; }
        public DbSet<BlogPostComment> BlogPostComment { get; set; }
        public DbSet<Log> Logs { get; set; }


        //Add-Migration AddLogsTable -Context BloggieDbContext
        //Update-Database -Context BloggieDbContext

        //Add-Migration UpdateLogModel -Context BloggieDbContext

       

    }
}
