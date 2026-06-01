using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace booksystem.api.DTOs.Books
{
    /// <summary>
    /// 規範更新書籍時，只能改動資料庫的欄位範疇
    /// </summary>
    [Description("規範更新書籍時，只能改動資料庫的欄位範疇")]
    public class UpdateBookDto
    {
        public string Name { get; set; }
        public string ISBN { get; set; }
        public decimal Price { get; set; }
        public DateOnly PublishDate { get; set; }
        public int AuthorId { get; set; }
        public int? CategoryId { get; set; }
    }
}