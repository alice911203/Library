using System.ComponentModel.DataAnnotations;

namespace booksystem.api.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;
        
        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Role { get; set; } = "User"; // 預設身分為普通會員

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
