using Microsoft.EntityFrameworkCore;
using System;
using System.Drawing;
using System.Linq;

namespace EntityFrameworkTest
{
    class Program
    {
        static void Main()
        {
            using (var db = new BlogContext())
            {
                db.Themes.ToList();
            }

            Console.WriteLine();
        }
    }

    class BlogContext : DbContext
    {

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Theme> Themes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Theme>()
                //.SeedData(
                //new Theme { ThemeId = 1, Name = "Temadimmerda", TitleColor = Color.Azure.Name }
                //)
                ;
        }


    }

    public class Blog
    {
        public int BlogId { get; set; }
        public string BlogUrl { get; set; }
        public Theme Theme { get; set; }
    }

    public class Theme
    {
        public int ThemeId { get; set; }
        public string Name { get; set; }
        public string TitleColor { get; set; }
    }
}
