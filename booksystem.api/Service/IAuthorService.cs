using booksystem.api.DTOs.Category;
using booksystem.api.DTOs.Author;

namespace booksystem.api.Service
{
    public interface IAuthorService
    {
        Task<IEnumerable<object>> GetAuthorsAsync();
        Task<int> CreateAuthorAsync(AuthorCreateDto dto);
        Task<List<int>> BatchCreateAuthorsAsync(IEnumerable<AuthorCreateDto> dtos); // 多筆新增
        Task<bool> UpdateAuthorAsync(int id, AuthorUpdateDto dto);
        Task<bool> DeleteAuthorAsync(int id);
    }
}
