using System.ComponentModel.DataAnnotations;

namespace WarehouseAPI.Application.DTOs
{
    public class CategoryDto
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(25)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Description { get; set; }
        
        public int? ParentId { get; set; }
    }
}