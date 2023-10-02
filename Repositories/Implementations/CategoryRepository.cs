using BlogAPIDotnet6.Data;
using BlogAPIDotnet6.Models;
using BlogAPIDotnet6.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlogAPIDotnet6.Repositories.Implementations;

public class CategoryRepository : ICategoryRepository
{
    private readonly DataContext _dataContext;

    public CategoryRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    
    public CategoryModel GetCategoryById(Guid id)
    {
        return _dataContext.Categories.FirstOrDefault(c => c.Id == id);
    }

    public List<CategoryModel> GetAllCategories()
    {
        return _dataContext.Categories.ToList();
    }

    public CategoryModel AddCategory(CategoryModel category)
    {
        _dataContext.Categories.Add(category);
        _dataContext.SaveChanges();
        return category;
    }

    public CategoryModel UpdateCategory(CategoryModel category)
    {
        _dataContext.Entry(category).State = EntityState.Modified;
        _dataContext.SaveChanges();
        return category;
    }

    public bool DeleteCategory(Guid id)
    {
        CategoryModel category = GetCategoryById(id);

        if (category == null)
        {
            throw new System.Exception("Houve um erro ao excluir a categoria.");
        }

        _dataContext.Categories.Remove(category);
        _dataContext.SaveChanges();

        return true;
    }
}