using booksystem.api.Data;
using booksystem.api.DTOs.Auth;
using booksystem.api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace booksystem.api.Service
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        // 注入 IConfiguration，為了去 appsettings.json 拿 JWT 的設定檔
        public AuthService(IConfiguration configuration, ApplicationDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }
        public async Task<string> RegisterAsync(RegisterDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.Username == dto.Username)) 
            {
                return "帳號已存在，請直接登入";
            }
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            var newUser = new User
            {
                Username = dto.Username,
                PasswordHash = passwordHash,
                Role = "User" // 預設新註冊的人都是普通會員
            };
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return "註冊成功";
        }
        public async Task<string?> LoginAsync(LoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
            if (user== null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            {
                return null;
            }
   

            // 1. 準備 Payload 聲明 (Claims)：決定手環上面要寫什麼資訊
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()), // 使用者的 ID (Subject)
                new Claim(JwtRegisteredClaimNames.Name, user.Username), // 使用者名稱
                new Claim(ClaimTypes.Role, user.Role) // 使用者權限角色
            };

            // 2. 取出我們藏好的 Secret Key，並轉成對稱式加密金鑰
            var signKey = _configuration["Jwt:Key"] ?? throw new Exception("Jwt:Key is missing");
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signKey));

            // 3. 準備數位簽章的憑證 (使用剛剛的金鑰 + HMAC-SHA256 演算法)
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // 4. 設定 JWT 的詳細規格 (發行者、觀眾、過期時間、簽章)
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                Subject = new ClaimsIdentity(claims), // 把剛剛的 Claims 塞進來
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"])), // 設定過期時間
                SigningCredentials = credentials
            };

            // 5. 呼叫微軟底層的 Token 處理器，真正產生出這串字串
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            // 將物件轉換成最終前端看得到的 eyJ... 字串
            return tokenHandler.WriteToken(securityToken);
        }
    }
}