using booksystem.api.Data;
using booksystem.api.Models;
using booksystem.api.DTOs.Author;
using Microsoft.EntityFrameworkCore;


namespace booksystem.api.Service
{
    public class AuthorService: IAuthorService
    {
        private readonly ApplicationDbContext _context;

        public AuthorService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<object>> GetAuthorsAsync()
        {
            return await _context.Authors
                .Select(a => new { a.Id, a.Name, a.Introduction })
                .ToListAsync();
        }

        public async Task<int> CreateAuthorAsync(AuthorCreateDto dto)
        {
            var author = new Author { Name = dto.Name, Introduction = dto.Introduction };
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();
            return author.Id;
        }

        // 💡 多筆批次新增的實作
        public async Task<List<int>> BatchCreateAuthorsAsync(IEnumerable<AuthorCreateDto> dtos)
        {
            // 將傳進來的 DTO 陣列，轉換成 Author 實體陣列
            var authors = dtos.Select(dto => new Author
            {
                Name = dto.Name,
                Introduction = dto.Introduction
            }).ToList();

            // 使用 AddRange 一次性加入追蹤
            _context.Authors.AddRange(authors);

            // 一次性寫入資料庫
            await _context.SaveChangesAsync();

            // 回傳所有成功建立的 ID 陣列
            return authors.Select(a => a.Id).ToList();
        }
    }
}
