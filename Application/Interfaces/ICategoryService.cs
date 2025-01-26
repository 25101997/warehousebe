using WarehouseAPI.Application.Models;

namespace WarehouseAPI.Application.Interfaces
{
    public interface ICategoryService
    {
        // Retorna todas las categorías
        Task<IEnumerable<Category>> GetAllAsync();

        // Retorna una categoría por ID
        Task<Category?> GetByIdAsync(int id);

        // Crea una categoría nueva
        Task<Category> CreateAsync(Category category);
    }
}
