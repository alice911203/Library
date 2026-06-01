using System.ComponentModel.DataAnnotations;

namespace booksystem.api.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        // 導覽屬性：一個分類底下有很多本書
        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
