using booksystem.api.Data;
using booksystem.api.DTOs.Books;
using booksystem.api.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
namespace booksystem.api.Service
{
    public class BookService : IBookService
    {
        private readonly ApplicationDbContext _context;
        public BookService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<BookResponseDto>> GetAllBooksAsync(string? keyword, decimal? minPrice, decimal? maxPrice, string? categoryName)
        {
            var query = _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .AsQueryable();
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(b => b.Name.Contains(keyword) || b.Author!.Name.Contains(keyword));
            }

            if (minPrice.HasValue)
            {
                query = query.Where(b => b.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(b => b.Price <= maxPrice.Value);
            }

            if (!string.IsNullOrWhiteSpace(categoryName))
            {
                query = query.Where(b => b.Category != null && b.Category.Name == categoryName);
            }
            var books = await query.Select(b => new BookResponseDto
            {
                Id = b.Id,
                Name = b.Name,
                ISBN = b.ISBN,
                Price = b.Price,
                PublishDate = b.PublishDate,
                AuthorName = b.Author!.Name,
                CategoryName = b.Category != null ? b.Category.Name : "沒有分類"
            }).ToListAsync();
            
            return books;
        }
        public async Task<BookResponseDto?> GetBookByIdAsync(int id)
        {
            var book = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .FirstOrDefaultAsync(b=> b.Id==id);
            if (book  == null) return null; 
            return new BookResponseDto
            {
                Id = book.Id,
                Name = book.Name,
                ISBN = book.ISBN,
                Price = book.Price,
                PublishDate = book.PublishDate,
                AuthorName = book.Author!.Name,
                CategoryName = book.Category != null ? book.Category.Name : "沒有分類"
            };
        }

        public async Task <IEnumerable<BookResponseDto>> CreateAllBookAsync(IEnumerable<CreateBookDto> dto)
        {
            if (dto == null || !dto.Any()) { return Enumerable.Empty<BookResponseDto>(); }

            var newbooks = dto.Select(d => new Book
            {
                Name = d.Name,
                ISBN = d.ISBN,
                Price = d.Price,
                PublishDate = d.PublishDate,
                CategoryId = d.CategoryId,
                AuthorId = d.AuthorId,  
            }).ToList();
        _context.AddRange(newbooks);
        await _context.SaveChangesAsync();
            var newIds = newbooks.Select(b => b.Id).ToList();
            return await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Where(b => newIds.Contains(b.Id))
                .Select(b => new BookResponseDto
                {
                    Id = b.Id,
                    Name = b.Name,
                    ISBN = b.ISBN,
                    Price = b.Price,
                    PublishDate = b.PublishDate,
                    AuthorName = b.Author!.Name,
                    CategoryName = b.Category != null ? b.Category.Name : "沒有分類"
                }).ToListAsync();
        }
        
        public async Task<BookResponseDto> CreateBookAsync (CreateBookDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            // 1. 在真正寫入書籍之前，先去資料庫查查看這個作者 ID 到底存不存在？
            var authorExists = await _context.Authors.AnyAsync(a => a.Id == dto.AuthorId);

            // 2. 如果不存在（例如前端亂傳一個 999），我們就溫柔地擋下來，不讓它去撞 SQL Server 的牆
            if (!authorExists)
            {
                throw new ArgumentException("新增失敗：找不到該作者");
            }
            ;
           
            var newbook =  new Book
            {
                Name = dto.Name,
                ISBN = dto.ISBN,
                Price = dto.Price,
                PublishDate = dto.PublishDate,
                CategoryId = dto.CategoryId,
                AuthorId = dto.AuthorId,
            };
             _context.Add(newbook);
            await _context.SaveChangesAsync();
            var createdBook = await _context.Books
            .Include(b => b.Author)
            .Include(b => b.Category)
            .FirstAsync(b => b.Id == newbook.Id);
            var response = new BookResponseDto
            {
                Id = createdBook.Id,
                Name = createdBook.Name,
                ISBN= createdBook.ISBN,
                Price = createdBook.Price,
                PublishDate = createdBook.PublishDate,
                AuthorName = createdBook.Author!.Name,
                CategoryName = createdBook.Category != null ? createdBook.Category.Name : "無分類"

            };
            return response;
        }
        public async Task<bool> UpdateBookAsync(int id, UpdateBookDto dto)
        {
            var existingBook = await _context.Books.FindAsync(id);

            if (existingBook == null) {return false;}

            existingBook.Name = dto.Name;
            existingBook.ISBN = dto.ISBN;
            existingBook.Price= dto.Price;
            existingBook.PublishDate = dto.PublishDate;
            existingBook.AuthorId = dto.AuthorId;
            existingBook.CategoryId = dto.CategoryId;

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteBookAsync(int id)
        {
            var affectedRows = await _context.Books.Where(b => b.Id == id).ExecuteDeleteAsync();
            return affectedRows>0;
        }

    }
    }

