using BlogAPIDotnet6.Models;

namespace BlogAPIDotnet6.Repositories.Interfaces;

public interface IPostRepository
{
    PostModel GetPostById(Guid id);
    List<PostModel> GetAllPosts();
    PostModel AddPost(PostModel post);
    PostModel UpdatePost(PostModel post);
    bool DeletePost(Guid id);
}