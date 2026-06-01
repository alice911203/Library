using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace booksystem.api.DTOs.Books
    
{
    /// <summary>
    /// 規範新增書籍時，只能改動資料庫的欄位範疇
    /// </summary>
    [Description("規範新增書籍時，只能改動資料庫的欄位範疇")]
    public class CreateBookDto
    {
        
        [Required(ErrorMessage = "請勿傳入空白")]
        public string Name { get; set; }
        [Required]
        [RegularExpression(@"^\d{13}$",ErrorMessage ="請輸入13碼ISBN")]
        public string ISBN { get; set; }
        [Range(0, double.MaxValue,ErrorMessage ="價格不可為負")]
        public decimal Price  { get; set; }
        public DateOnly PublishDate { get; set; }
        public int AuthorId { get; set; }
        public int? CategoryId { get; set; }
    }
}
