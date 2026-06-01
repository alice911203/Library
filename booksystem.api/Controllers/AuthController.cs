using booksystem.api.DTOs.Auth;
using booksystem.api.Service;
using Microsoft.AspNetCore.Mvc;

namespace booksystem.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController:ControllerBase
    {

            private readonly IAuthService _authService;

            public AuthController(IAuthService authService)
            {
                _authService = authService;
            }
            [EndpointSummary("使用者註冊")]
            [EndpointDescription("建立新帳號，密碼將被自動加密")]
            [HttpPost("Register")]
            public async Task<IActionResult> Register(RegisterDto dto)
            {
                var result = await _authService.RegisterAsync(dto);

                if (result == "帳號已存在，請直接登入")
                {
                    // 409 Conflict 代表與伺服器當前狀態衝突 (帳號重複)
                    return Conflict(new { Message = result });
                }

                // 200 OK
                return Ok(new { Message = result });
            }
            [EndpointSummary("使用者登入")]
            [EndpointDescription("輸入正確帳密以獲取 JWT Token")]
            [HttpPost("Login")]
            public async Task<IActionResult> Login(LoginDto dto)
            {
                var token = await _authService.LoginAsync(dto);

                if (token == null)
                {
                    // 帳密錯誤，回傳 401 未授權
                    return Unauthorized("帳號或密碼錯誤。");
                }

                // 成功登入！為了符合 JSON 格式，我們用一個匿名物件把字串包起來回傳
                return Ok(new { Token = token });
            }
        }
    }

