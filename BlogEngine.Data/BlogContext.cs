using System;
using BlogEngine.Core.Contracts;
using BlogEngine.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogEngine.Data
{
    public class BlogContext : DbContext
    {
        public BlogContext(DbContextOptions<BlogContext> options) : base(options)
        { }

        public DbSet<Post> Post { get; set; }

        public DbSet<Comment> Comment { get; set; }

    }
}
