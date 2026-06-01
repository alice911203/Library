
using booksystem.api.Data;
using booksystem.api.Models;
using Microsoft.EntityFrameworkCore;
using booksystem.api.DTOs.Category;

namespace booksystem.api.Service
{
    public class CategoryService:ICategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<object>> GetCategoriesAsync()
        {
            return await _context.Categories
                .Select(a => new { a.Id, a.Name})
                .ToListAsync();
        }

        public async Task<int> CreateCategoryAsync(CategoryCreateDto dto)
        {
            var Category = new Category { Name = dto.Name };
            _context.Categories.Add(Category);
            await _context.SaveChangesAsync();
            return Category.Id;
        }


        public async Task<List<int>> BatchCreateCategoriesAsync(IEnumerable<CategoryCreateDto> dtos)
        {
            // 將傳進來的 DTO 陣列，轉換成 Author 實體陣列
            var Categories = dtos.Select(dto => new Category
            {
                Name = dto.Name,
            }).ToList();

            // 使用 AddRange 一次性加入追蹤
            _context.Categories.AddRange(Categories);

            // 一次性寫入資料庫
            await _context.SaveChangesAsync();

            // 回傳所有成功建立的 ID 陣列
            return Categories.Select(a => a.Id).ToList();
        }
        public async Task<bool> UpdateCategoryAsync(int id, CategoryUpdateDto dto)
        {
            var Category =await  _context.Categories.FindAsync(id);
            if (Category == null) return false;
            Category.Name = dto.Name;
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var affectedRows = await _context.Categories.Where(c => c.Id==id).ExecuteDeleteAsync(); 
            
            return affectedRows>0;
        }
    }
}
