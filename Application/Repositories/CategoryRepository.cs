using Microsoft.EntityFrameworkCore;
using WarehouseAPI.Application.Models;
using WarehouseAPI.Application.Interfaces;
using WarehouseAPI.Data;

namespace WarehouseAPI.Application.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        // Se inyecta el DbContext para interactuar con la BD
        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories
                .AsNoTracking() // Acelera lecturas, no rastrea cambios
                .ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Category> CreateAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<Category?> GetByNameAsync(string name)
        {
            // AsNoTracking para lectura sin rastreo
            return await _context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Name == name);
        }

        public async Task<Category?> GetByNameAndParentIdAsync(string name, int? parentId)
        {
            return await _context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Name == name && c.ParentId == parentId);
        }
    }
}
