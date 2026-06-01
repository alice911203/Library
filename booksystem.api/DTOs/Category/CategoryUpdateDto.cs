using System.ComponentModel.DataAnnotations;

namespace booksystem.api.DTOs.Category
{
    public class CategoryUpdateDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
