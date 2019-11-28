using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogEngine.Core.Contracts;
using BlogEngine.Core.Models;

namespace BlogEngine.Core.Services
{
    public class BlogService : IBlogService
    {
        private readonly IBlogRepository _blogRepository;


        public BlogService(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        /// <summary>
        /// Method to get all the posts by status
        /// </summary>
        /// <param name="status">Status to be searched</param>
        /// <returns>List of posts</returns>
        public async Task<IEnumerable<IPost>> GetPostsByStatus(Status status)
        {
            var posts = await _blogRepository.GetPosts();
            return posts?.Where(x => x.Status == status);
        }

        /// <summary>
        ///  Method to get all the posts by writer id
        /// </summary>
        /// <param name="writerId">Writer id to be searched</param>
        /// <returns>List of posts</returns>
        public async Task<IEnumerable<IPost>> GetPostsByWriter(string writerId)
        {
            var posts = await _blogRepository.GetPosts();
            return posts?.Where(x => x.WriterId == writerId);
        }

        /// <summary>
        /// Method to create or update a post
        /// </summary>
        /// <param name="post">Post to be created or updated</param>
        /// <returns>true if it was succeeded otherwise false</returns>
        public bool CreateUpdatePost(Post post, string? approverId = null)
        {
            bool result;
            if (post.Id == 0)
            {
                post.CreatedDate = DateTime.Now;
                if (post.Status == Status.Published)
                {
                    post.ApprovalDate = DateTime.Now;
                    post.ApproverId = approverId;
                }
                result = _blogRepository.CreatePost(post);
            }
            else
            {
                var existingPost = _blogRepository.GetPostById(post.Id);
                existingPost.Title = post.Title;
                existingPost.Content = post.Content;
                existingPost.Status = post.Status;
                existingPost.WriterId = post.WriterId;
                if (post.Status == Status.Published && existingPost.Status != Status.Published)
                {
                    existingPost.ApprovalDate = DateTime.Now;
                    existingPost.ApproverId = approverId;
                }
                else
                {
                    existingPost.ApproverId = post.ApproverId;
                    existingPost.ApprovalDate = post.ApprovalDate;
                }

                result = _blogRepository.UpdatePost(existingPost);
            }

            return result;
        }

        /// <summary>
        /// Method to update the status of a post
        /// </summary>
        /// <param name="postId">postId to be updated</param>
        /// <param name="status">status to be setted</param>
        /// <returns>true if it was succeeded otherwise false</returns>
        public bool UpdateStatePost(int postId, Status status, string? approverId = null)
        {
            var existingPost = _blogRepository.GetPostById(postId);
            existingPost.Status = status;
            if (status == Status.Published)
            {
                existingPost.ApprovalDate = DateTime.Now;
                existingPost.ApproverId = approverId;
            }

            return _blogRepository.UpdatePost(existingPost);
        }

        /// <summary>
        /// Method to create a comment
        /// </summary>
        /// <param name="comment">Comment to be created</param>
        /// <returns>true if it was succeeded otherwise false</returns>
        public bool CreateComment(Comment comment)
        {
            comment.CreatedDate = DateTime.Now;

            return _blogRepository.CreateComment(comment);
        }
    }
}
