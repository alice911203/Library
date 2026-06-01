using booksystem.api.DTOs.Category;
using booksystem.api.Service;
using Microsoft.AspNetCore.Mvc;
using booksystem.api.DTOs.Author;

namespace booksystem.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorsController:ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorsController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAuthors()
        {
            var authors = await _authorService.GetAuthorsAsync();
            return Ok(authors);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAuthor(AuthorCreateDto dto)
        {
            var id = await _authorService.CreateAuthorAsync(dto);
            return Ok(new { Message = "作者新增成功", Id = id });
        }

        // 💡 開放給前端打的多筆新增端點
        [HttpPost("Batch")]
        public async Task<IActionResult> BatchCreateAuthors([FromBody] IEnumerable<AuthorCreateDto> dtos)
        {
            if (dtos == null || !dtos.Any())
            {
                return BadRequest("請提供要新增的作者資料");
            }

            var ids = await _authorService.BatchCreateAuthorsAsync(dtos);
            return Ok(new { Message = $"成功新增 {ids.Count} 位作者", Ids = ids });
        }
    }
}
