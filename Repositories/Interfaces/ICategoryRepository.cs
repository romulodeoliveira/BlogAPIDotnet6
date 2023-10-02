using BlogAPIDotnet6.Models;

namespace BlogAPIDotnet6.Repositories.Interfaces;

public interface ICategoryRepository
{
    CategoryModel GetCategoryById(Guid id);
    List<CategoryModel> GetAllCategories();
    CategoryModel AddCategory(CategoryModel category);
    CategoryModel UpdateCategory(CategoryModel category);
    bool DeleteCategory(Guid id);
}