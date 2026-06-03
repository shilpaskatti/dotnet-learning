using LearningBasics.Models;
using Microsoft.EntityFrameworkCore;

namespace LearningBasics.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) :DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
