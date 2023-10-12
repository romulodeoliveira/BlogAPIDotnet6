using BlogAPIDotnet6.Data;
using BlogAPIDotnet6.DTOs.Category;
using BlogAPIDotnet6.Helper;
using BlogAPIDotnet6.Models;
using BlogAPIDotnet6.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlogAPIDotnet6.Repositories.Implementations;

public class CategoryRepository : ICategoryRepository
{
    private readonly DataContext _dataContext;
    private readonly IUserRepository _userRepository;
    
    public CategoryRepository(DataContext dataContext, IUserRepository userRepository)
    {
        _dataContext = dataContext;
        _userRepository = userRepository;
    }
    
    public CategoryModel GetCategoryById(Guid id)
    {
        return _dataContext.Categories.FirstOrDefault(c => c.Id == id);
    }

    public List<CategoryModel> GetAllCategories()
    {
        return _dataContext.Categories.ToList();
    }

    public (bool Success, string Message) AddCategory(CreateCategory request, string username)
    {
        try
        {
            var user = _userRepository.GetUserByUsername(username);
            if (user != null)
            {
                if (user.Role == Roles.Admin)
                {
                    var category = new CategoryModel();
            
                    if (!string.IsNullOrEmpty(request.Title))
                    {
                        category.Title = request.Title;
                    }
                    else
                    {
                        return (false, "O título não pode estar em branco.");
                    }
                    
                    category.Username = username;
                    category.User = user;
                
                    _dataContext.Categories.Add(category);
                    _dataContext.SaveChanges();
                    return (true, "Categoria criada com sucesso.");
                }
                else
                {
                    return (false, "Você não tem permissão para essa operação.");
                }
            }
            else
            {
                return (false, "Usuário não encontrado.");
            }
        }
        catch (Exception error)
        {
            return (false, $"Erro interno do servidor: {error.Message}");
        }
    }

    public (bool Success, string Message) UpdateCategory(Guid categoryId, UpdateCategory request, string username)
    {
        try
        {
            var user = _userRepository.GetUserByUsername(username);
            if (user != null)
            {
                if (user.Role == Roles.Admin)
                {
                    var category = GetCategoryById(categoryId);

                    if (!string.IsNullOrEmpty(request.Title))
                    {
                        category.Title = request.Title;
                    }
                    
                    category.UpdatedAt = DateTime.UtcNow;

                    category.Username = username;
                    category.User = user;
                
                    _dataContext.Entry(category).State = EntityState.Modified;
                    _dataContext.SaveChanges();
                    return (true, "Categoria atualizada com sucesso.");
                }
                else
                {
                    return (false, "Você não tem permissão para essa operação.");
                }
            }
            else
            {
                return (false, "Usuário não encontrado.");
            }
        }
        catch (Exception error)
        {
            return (false, $"Erro interno do servidor: {error.Message}");
        }
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