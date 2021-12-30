using System;
using APIWebRESTful.Models;
using Microsoft.EntityFrameworkCore;


namespace APIWebRESTful.Data
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options) { }

        public DbSet<Hero> Heroes { get; set; } = null!;
    }
}
