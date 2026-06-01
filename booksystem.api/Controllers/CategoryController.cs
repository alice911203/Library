using booksystem.api.DTOs.Category;
using booksystem.api.Service;
using Microsoft.AspNetCore.Mvc;


namespace booksystem.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var Categories = await _categoryService.GetCategoriesAsync();
            return Ok(Categories);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CategoryCreateDto dto)
        {
            var id = await _categoryService.CreateCategoryAsync(dto);
            return Ok(new { Message = "類別新增成功", Id = id });
        }

        // 💡 開放給前端打的多筆新增端點
        [HttpPost("Batch")]
        public async Task<IActionResult> BatchCreateCategories([FromBody] IEnumerable<CategoryCreateDto> dtos)
        {
            if (dtos == null || !dtos.Any())
            {
                return BadRequest("請提供要新增的類別資料");
            }

            var ids = await _categoryService.BatchCreateCategoriesAsync(dtos);
            return Ok(new { Message = $"成功新增 {ids.Count} 個類別", Ids = ids });
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryUpdateDto dto)
        {
            var UpdateCategory = await _categoryService.UpdateCategoryAsync(id, dto);
            if (UpdateCategory == false) return NotFound("找不到要更新的類別");
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
           
                var DeleteCategory = await _categoryService.DeleteCategoryAsync(id);
                if (!DeleteCategory) return NotFound("找不到要刪除的類別");
                return NoContent();
            }
    }
}
