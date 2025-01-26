using WarehouseAPI.Application.Models;
using WarehouseAPI.Application.Interfaces;

namespace WarehouseAPI.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            // Aquí podrías aplicar lógicas de negocio,
            // validaciones, filtrados, etc.
            return await _categoryRepository.GetAllAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            // Podrías aplicar aquí validaciones adicionales
            // (por ejemplo, verificar si el ID es válido antes de llamar al repo)
            return await _categoryRepository.GetByIdAsync(id);
        }

        public async Task<Category> CreateAsync(Category category)
        {
            // 1. Validar que el "Name" no sea vacío o nulo.
            if (string.IsNullOrWhiteSpace(category.Name))
            {
                // Puedes lanzar excepción o retornar algún resultado de error.
                throw new ArgumentException("El campo 'Name' es obligatorio.");
            }

            // 2. Validar "ParentId":
            //    - Si ParentId <= 0, lo tratamos como null (categoría raíz).
            //    - Si ParentId > 0, verificamos que exista la categoría padre.
            if (category.ParentId.HasValue && category.ParentId.Value > 0)
            {
                var parentCategory = await _categoryRepository.GetByIdAsync(category.ParentId.Value);
                if (parentCategory == null)
                {
                    // Si no existe la categoría padre, puedes lanzar excepción
                    throw new InvalidOperationException("La categoría padre especificada no existe.");
                }
            }
            else
            {
                // Interpretar cualquier valor <= 0 como "sin padre"
                category.ParentId = null;
            }

            // 3. Verificar si ya existe una categoría con el mismo nombre
            var existingCategory = await _categoryRepository.GetByNameAndParentIdAsync(category.Name, category.ParentId);
            if (existingCategory != null)
            {
                throw new InvalidOperationException(
                    $"Ya existe la categoría '{category.Name}' bajo el mismo padre.");
            }

            // 4. Finalmente, crear la categoría en la BD
            return await _categoryRepository.CreateAsync(category);
        }

    }
}
