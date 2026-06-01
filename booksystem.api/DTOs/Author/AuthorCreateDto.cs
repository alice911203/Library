using System.ComponentModel.DataAnnotations;

namespace booksystem.api.DTOs.Author
{
        public class AuthorCreateDto
        {
            [Required]
            public string Name { get; set; } = string.Empty;
            public string Introduction { get; set; } = string.Empty;
        }

       
}

