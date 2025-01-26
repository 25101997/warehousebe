using WarehouseAPI.Application.Models;

namespace WarehouseAPI.Application.Interfaces
{
    public interface ICategoryRepository
    {
        // Devuelve todas las categorías en modo asíncrono
        Task<IEnumerable<Category>> GetAllAsync();

        // Devuelve una categoría por ID, o null si no existe
        Task<Category?> GetByIdAsync(int id);

        // Crea una nueva categoría y la persiste en BD
        Task<Category> CreateAsync(Category category);

        // Actualiza una categoría existente
        //Task<Category> UpdateAsync(Category category);

        // Elimina una categoría por ID, devuelve true si existía, false si no
        //Task<bool> DeleteAsync(int id);

        // Devuelve una categoría por nombre
        Task<Category?> GetByNameAsync(string name);

        //Devuelve una categoría por Name y ParentId
        Task<Category?> GetByNameAndParentIdAsync(string name, int? parentId);
    }
}
