using booksystem.api.DTOs.Category;

namespace booksystem.api.Service
{
    public interface ICategoryService
    {
        Task<IEnumerable<object>> GetCategoriesAsync();
        Task<int> CreateCategoryAsync(CategoryCreateDto dto);
        Task<List<int>> BatchCreateCategoriesAsync(IEnumerable<CategoryCreateDto> dtos);
        Task<bool> UpdateCategoryAsync(int id, CategoryUpdateDto dto);
        Task<bool> DeleteCategoryAsync(int id);
    }
}
