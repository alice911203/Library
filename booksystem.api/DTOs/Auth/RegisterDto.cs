using System.ComponentModel.DataAnnotations;

namespace booksystem.api.DTOs.Auth
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "請輸入帳號")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "帳號長度需介於 3 到 50 個字元之間")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "請輸入密碼")]
        [MinLength(6, ErrorMessage = "密碼長度至少需要 6 碼")]
        public string Password { get; set; } = string.Empty;
    }
}
