using System.ComponentModel.DataAnnotations;

namespace booksystem.api.DTOs.Category
{
   
        public class CategoryCreateDto
        {
            [Required]
            public string Name { get; set; } = string.Empty;
        }
}

