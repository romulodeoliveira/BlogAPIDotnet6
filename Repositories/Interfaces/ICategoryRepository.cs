using BlogAPIDotnet6.DTOs.Category;
using BlogAPIDotnet6.Models;

namespace BlogAPIDotnet6.Repositories.Interfaces;

public interface ICategoryRepository
{
    CategoryModel GetCategoryById(Guid id);
    List<CategoryModel> GetAllCategories();
    (bool Success, string Message) AddCategory(CreateCategory category, string username);
    (bool Success, string Message) UpdateCategory(Guid categoryId, UpdateCategory category, string username);
    (bool Success, string Message) DeleteCategory(Guid id, string username);
}