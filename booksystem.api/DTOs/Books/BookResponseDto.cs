using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace booksystem.api.DTOs.Books
{
    /// <summary>
    /// 規範回傳書籍資料時，只能回傳資料庫的欄位範疇
    /// </summary>
    [Description("規範回傳書籍資料時，只能回傳資料庫的欄位範疇")]
    public class BookResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ISBN { get; set; }
        public decimal Price { get; set; }
        public DateOnly PublishDate { get; set; }
        public string AuthorName { get; set; } = string.Empty;
        public string? CategoryName { get; set; }
    }
}
