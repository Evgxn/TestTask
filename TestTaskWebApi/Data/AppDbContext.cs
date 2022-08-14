using Microsoft.EntityFrameworkCore;
using TestTaskWebApi.Models;

namespace TestTaskWebApi.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Card> Cards { get; set; }
    }
}
