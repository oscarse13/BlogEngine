using BlogEngine.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlogEngine.Core.Contracts
{
    public interface IBlogRepository
    {
        Post GetPostById(int id);
        Task<IEnumerable<IPost>> GetPosts();
        bool CreatePost(Post post);
        bool UpdatePost(Post post);
        bool CreateComment(Comment comment);
    }
}
