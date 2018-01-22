using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace data_seeding
{
    public class Program
    {

        static void Main()
        {

            using (var db = new BloggingContext())
            {
                   
            }
        }
        public class BloggingContext : DbContext
        {


            public DbSet<Blog> Blogs { get; set; }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder
                    .UseSqlServer(@"Server=(localdb)\\mssqllocaldb;Database=aspnet-Mymva-53bc9b9d-9d6a-45d4-8429-2a2761773502;Trusted_Connection=True;MultipleActiveResultSets=true");
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder
                    .Entity<Blog>()
                    .SeedData(
                        new Blog { BlogId = 1, BlogUrl = "workaround.space" },
                        new Blog { BlogId = 1, BlogUrl = "pensieroantisociale.it" }
                    );
            }

            public class Blog
            {
                public int BlogId { get; set; }
                public string BlogUrl { get; set; }
            }

        }


    }
