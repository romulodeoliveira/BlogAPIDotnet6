using BlogAPIDotnet6.Models;

namespace BlogAPIDotnet6.Repositories.Interfaces;

public interface ICommentRepository
{
    CommentModel GetCommentById(Guid id);
    List<CommentModel> GetAllCommentsForPublication(Guid id);
    List<CommentModel> GetAllCommentsForUser(string username);
    CommentModel AddComment(CommentModel comment);
    CommentModel UpdateComment(CommentModel comment);
    bool DeleteComment(Guid id);
}