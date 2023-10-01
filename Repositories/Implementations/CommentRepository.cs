using BlogAPIDotnet6.Data;
using BlogAPIDotnet6.Models;
using BlogAPIDotnet6.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlogAPIDotnet6.Repositories.Implementations;

public class CommentRepository : ICommentRepository
{
    private readonly DataContext _dataContext;

    public CommentRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    
    public CommentModel GetCommentById(Guid id)
    {
        return _dataContext.Comments.FirstOrDefault(c => c.Id == id);
    }

    public List<CommentModel> GetAllCommentsForPublication(Guid id)
    {
        var comments = _dataContext.Comments
            .Where(c => c.PostId == id)
            .ToList();

        return comments;
    }

    // GetAllCommentsForUser
    public List<CommentModel> GetAllCommentsForUser(string username)
    {
        var comments = _dataContext.Comments
            .Where((c => c.Username == username))
            .ToList();
        
        return comments;
    }
    public CommentModel AddComment(CommentModel comment)
    {
        _dataContext.Comments.Add(comment);
        _dataContext.SaveChanges();

        return comment;
    }

    public CommentModel UpdateComment(CommentModel comment)
    {
        _dataContext.Entry(comment).State = EntityState.Modified;
        _dataContext.SaveChanges();

        return comment;
    }

    public bool DeleteComment(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException($"O ID fornecido não é válido. {nameof(id)}");
        }

        CommentModel comment = GetCommentById(id);
        
        if (comment == null)
        {
            throw new System.Exception("Houve um erro ao excluir o comentário.");
        }

        _dataContext.Comments.Remove(comment);
        _dataContext.SaveChanges();

        return true;
    }
}