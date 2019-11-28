using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogEngine.Core.Contracts;
using BlogEngine.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogEngine.Data
{

    public class BlogDBRepository : IBlogRepository
    {
        /// <summary>
        /// The Database Context.
        /// </summary>
        private readonly BlogContext context;

        public BlogDBRepository(BlogContext contextObj)
        {
            context = contextObj;
        }

        /// <summary>
        ///  Method to get a post by id
        /// </summary>
        /// <param name="id">id to be searched</param>
        /// <returns>Post found</returns>
        public Post GetPostById(int id)
        {
            return context.Post.FirstOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// Method to get all the posts
        /// </summary>
        /// <returns>List of posts</returns>
        public async Task<IEnumerable<IPost>> GetPosts()
        {
            return await context.Post.Include(post => post.Comments).Where(x => x.Status != Status.Deleted).ToListAsync();
        }

        /// <summary>
        /// Method to create a post
        /// </summary>
        /// <param name="post">Post to be created</param>
        /// <returns>true if it was succeeded</returns>
        public bool CreatePost(Post post)
        {
            context.Post.Add(post);
            context.SaveChanges();
            return true;
        }

        /// <summary>
        /// Method to update a post
        /// </summary>
        /// <param name="post">Post to be updated</param>
        /// <returns>true if it was succeeded</returns>
        public bool UpdatePost(Post post)
        {
            context.Post.Update(post);
            context.SaveChanges();
            return true;
        }

        /// <summary>
        /// Method to create a comment
        /// </summary>
        /// <param name="comment">comment to be created</param>
        /// <returns>true if it was succeeded</returns>
        public bool CreateComment(Comment comment)
        {
            context.Comment.Add(comment);
            context.SaveChanges();
            return true;
        }
    }
}
