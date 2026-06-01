using booksystem.api.DTOs.Books;
namespace booksystem.api.Service
{
    public interface IBookService
    {   
        //查詢多筆書籍
        Task<IEnumerable<BookResponseDto>> GetAllBooksAsync(string? keyword, decimal? minPrice, decimal? maxPrice, string? categoryName);
        //查詢單筆書籍
        Task<BookResponseDto?> GetBookByIdAsync(int id);
        //建立書籍
        Task<BookResponseDto> CreateBookAsync(CreateBookDto dto);
        //批次建立書籍
        Task<IEnumerable<BookResponseDto>> CreateAllBookAsync(IEnumerable<CreateBookDto> dto);
        //修改書籍資料
        Task<bool> UpdateBookAsync(int id,UpdateBookDto dto);
        //刪除書籍
        Task<bool> DeleteBookAsync(int id);

    }
}
