using System.ComponentModel.DataAnnotations;

namespace booksystem.api.Models
{
    public class Author
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;
        public string Introduction { get; set; } = string.Empty;

        // 導覽屬性 (Navigation Property)：一個作者可以有很多本書
        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
