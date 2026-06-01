namespace booksystem.api.DTOs
{
    public class ErrorResponseDto
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? Details { get; set; } // 只有在開發環境才把詳細錯誤印出來
    }
}