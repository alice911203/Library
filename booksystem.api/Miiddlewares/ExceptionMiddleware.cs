using booksystem.api.DTOs;
using System.Net;
using System.Text.Json;

namespace booksystem.api.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        // 1. 透過 DI 注入下一個關卡 (_next) 與日誌系統 (_logger)
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        // 2. 這是每次有 HTTP Request 進來時，一定會執行的方法
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // 放行！讓請求繼續往後面的 Controller 和 Service 走
                await _next(context);
            }
            catch (Exception ex)
            {
                // 3. 如果後面有人摔破盤子（發生錯誤），就會跳到這裡被攔截！
                _logger.LogError(ex, "系統發生未預期的例外錯誤！"); // 偷偷記錄在伺服器後台
                await HandleExceptionAsync(context, ex); // 包裝成精美 JSON 吐給前端
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // 設定回傳格式為 JSON，並且狀態碼一律給 500 (Internal Server Error)
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new ErrorResponseDto
            {
                StatusCode = context.Response.StatusCode,
                Message = "伺服器發生未預期的錯誤，請稍後再試或聯絡系統管理員。",
                Details = exception.Message // 實戰中，你可以判斷如果是正式上線環境，就把這行設為 null
            };

            // 將 C# 物件轉成 JSON 字串
            var json = JsonSerializer.Serialize(response);

            return context.Response.WriteAsync(json);
        }
    }
}