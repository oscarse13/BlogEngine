using BlogEngine.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlogEngine.Core.Contracts
{
    public interface IBlogService
    {
        Task<IEnumerable<IPost>> GetPostsByStatus(Status status);
        Task<IEnumerable<IPost>> GetPostsByWriter(string writerId);
        bool CreateUpdatePost(Post post, string? approverId = null);
        bool UpdateStatePost(int postId, Status status, string? approverId = null);
        bool CreateComment(Comment comment);
    }
}
