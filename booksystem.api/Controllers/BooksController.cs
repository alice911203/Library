using booksystem.api.Data;
using booksystem.api.DTOs.Auth;
using booksystem.api.DTOs.Books;
using booksystem.api.Models;
using booksystem.api.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
namespace booksystem.api.Controllers {
    
    //設定API的route，裡面controller是微軟寫的動態參數，會抓此行下一行中的類別名稱，去掉Controller後當作端點
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    //設定一個類別名稱為  BooksController ，ControllerBase是一個類別，使用此功能是為了繼承父類別所有的json格式轉換、API回傳碼等功能
    {
        private readonly IBookService _bookService;
        public BooksController(IBookService bookservice)
        {
            _bookService =bookservice;
        }
        /// <summary>
        /// 取得目前圖書館的書籍資料
        /// </summary>
        /// <returns></returns>
        //這是.NET8之前的用法，swagger可以讀取summary，但到.net8之後，就是用微軟出的套件MapOpenApi()
        [EndpointSummary("取得目前圖書館的書籍資料")]
        [HttpGet]
        //定義他只能被get方法呼叫

        public async Task<ActionResult<IEnumerable<BookResponseDto>>> GetAllBook(
            [FromQuery] string? keyword,
            [FromQuery] decimal? minPrice,
            [FromQuery] decimal? maxPrice,
            [FromQuery] string? categoryName)

        {
            var books = await _bookService.GetAllBooksAsync(keyword, minPrice, maxPrice, categoryName);

            return Ok(books);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<BookResponseDto?>> GetBookbyId(int id)

        {
            var books = await _bookService.GetBookByIdAsync(id);
            if (books == null) return NotFound("此id找不到書籍");

            return Ok(books);
        }
        /// <summary>
        /// 批次匯入書籍資料
        /// 新增成功會回傳ID跟書籍資料
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [EndpointSummary("批次匯入書籍資料")]
        [EndpointDescription("新增成功會回傳ID跟書籍資料")]
        [Authorize]
        [HttpPost("Batch")]
        public async Task<IActionResult> CreateBooks(IEnumerable<CreateBookDto> dto) {
            var newbooks = await _bookService.CreateAllBookAsync(dto);
            return Ok(newbooks);
        }
        /// <summary>
        /// 匯入單筆書籍資料
        /// 新增成功會回傳ID跟書籍資料
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [EndpointSummary("匯入單筆書籍資料")]
        [EndpointDescription("新增成功會回傳ID跟書籍資料")]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateBook(CreateBookDto dto)
        {
            try
            {
                var newBook = await _bookService.CreateBookAsync(dto);
                // 狀態碼回傳201
                return CreatedAtAction(nameof(GetBookbyId), new { Id = newBook.Id }, newBook);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
            /// <summary>
            /// 修改書籍屬性
            /// 可修改書籍的名稱、ISBN、Price、PublishDate
            /// </summary>
            /// <param name="id"></param>
            /// <param name="dto"></param>
            /// <returns></returns>
            [EndpointSummary("修改書籍屬性")]
            [EndpointDescription("可修改書籍的名稱、ISBN、Price、PublishDate")]
            [Authorize(Roles = "admin")]
            [HttpPut("{id}")] // 注意這裡：路由多了一個 {id} 參數
            public async Task<IActionResult> UpdateBook(int id, UpdateBookDto dto)
            {

                var existingBook = await _bookService.UpdateBookAsync(id, dto);
                if (existingBook == false) return NotFound("此ID找不到書籍");
                return NoContent();
            }
            /// <summary>
            /// 刪除書籍資料
            /// 將書籍從圖書館中刪除
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            [EndpointSummary("刪除書籍資料")]
            [EndpointDescription("將書籍從圖書館中刪除")]
            [Authorize(Roles = "admin")]
            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteBook(int id)
            {

                var bookToDelete = await _bookService.DeleteBookAsync(id);
                if (!bookToDelete) return NotFound("此ID找不到書籍");
                return NoContent();
            }
        }
    }
    


