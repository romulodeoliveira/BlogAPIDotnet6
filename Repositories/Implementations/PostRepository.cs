using BlogAPIDotnet6.Data;
using BlogAPIDotnet6.Exceptions;
using BlogAPIDotnet6.Models;
using BlogAPIDotnet6.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlogAPIDotnet6.Repositories.Implementations;

public class PostRepository : IPostRepository
{
    private readonly DataContext _dataContext;

    public PostRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    
    public PostModel GetPostById(Guid id)
    {
        return _dataContext.Posts.FirstOrDefault(x => x.Id == id);
    }

    public List<PostModel> GetAllPosts()
    {
        return _dataContext.Posts.ToList();
    }

    public PostModel AddPost(PostModel post)
    {
        _dataContext.Posts.Add(post);
        _dataContext.SaveChanges();

        return post;
    }
    
    public bool IsSlugUnique(string slug)
    {
        return !_dataContext.Posts.Any(p => p.Slug == slug);
    }

    public PostModel UpdatePost(PostModel post)
    {
        _dataContext.Entry(post).State = EntityState.Modified;
        _dataContext.SaveChanges();

        return post;
    }

    public bool DeletePost(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("O ID fornecido não é válido.", nameof(id));
        }

        PostModel post = GetPostById(id);

        if (post == null)
        {
            throw new PostNotFoundException("A postagem não foi encontrada.");
        }

        try
        {
            _dataContext.Posts.Remove(post);
            _dataContext.SaveChanges();

            return true;
        }
        catch (DbUpdateException error)
        {
            throw new DataAccessException($"Ocorreu um erro ao excluir a postagem no banco de dados. {error}");
        }
    }
}