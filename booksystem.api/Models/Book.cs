using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace booksystem.api.Models
{
    public class Book
    {
        [Key] // 強制宣告：無論下面變數叫什麼名字，它就是這張表的主鍵！
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public string ISBN { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        [Range(0, double.MaxValue, ErrorMessage = "價格不可為負")]
        public decimal Price { get; set; }
        public DateOnly PublishDate { get; set; }
        public int AuthorId { get; set; }
        public Author? Author { get; set; } // 導覽屬性，交給 EF Core 去 JOIN

        // 2. 分類關聯 (加上 int? 代表允許為 Null，實現「無分類」狀態)
        public int? CategoryId { get; set; }
        public Category? Category { get; set; } // 導覽屬性
    }
}
