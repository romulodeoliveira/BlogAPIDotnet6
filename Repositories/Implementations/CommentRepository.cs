using BlogAPIDotnet6.Data;
using BlogAPIDotnet6.Models;
using BlogAPIDotnet6.Repositories.Interfaces;

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
        throw new NotImplementedException();
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
        throw new NotImplementedException();
    }

    public bool DeleteComment(Guid id)
    {
        throw new NotImplementedException();
    }
}