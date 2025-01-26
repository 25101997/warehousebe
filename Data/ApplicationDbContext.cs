using WarehouseAPI.Application.Models;
using Microsoft.EntityFrameworkCore;

namespace WarehouseAPI.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public required DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
               
        }
        
    }
}