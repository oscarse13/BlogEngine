using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogEngine.Core.Contracts;
using BlogEngine.Core.Models;
using BlogEngine.Core.Services;
using Moq;
using NUnit.Framework;

namespace BlogEngine.Test.Core.Services
{
    public class BlogServiceTest
    {
        [Test]
        public async Task SuccessGettingPostsByStatus()
        {
            //Arrange
            var posts = GetFakeDataPosts();
            var blogRepository = new Mock<IBlogRepository>();
            blogRepository.Setup(repo => repo.GetPosts()).ReturnsAsync(posts);

            var expectedListResponse = new List<Post>
            {
                new Post {  Id = 2, Title = "Post 2", Content = "This is the second post", WriterId = "UserId", Status = Status.Pending},
                new Post {  Id = 3, Title = "Post 3", Content = "This is the third post", WriterId = "UserId", Status = Status.Pending}
            };

            var blogService = new BlogService(blogRepository.Object);

            //Act
            var responseListOfPosts = await blogService.GetPostsByStatus(Status.Pending);

            //Assert

            Assert.AreEqual(responseListOfPosts.Count(), expectedListResponse.Count);
            Assert.AreEqual(responseListOfPosts.FirstOrDefault().Id, expectedListResponse.FirstOrDefault().Id);
            Assert.AreEqual(responseListOfPosts.FirstOrDefault().Title, expectedListResponse.FirstOrDefault().Title);
            Assert.AreEqual(responseListOfPosts.LastOrDefault().Id, expectedListResponse.LastOrDefault().Id);
            Assert.AreEqual(responseListOfPosts.LastOrDefault().Title, expectedListResponse.LastOrDefault().Title);
        }

        [Test]
        public async Task SuccessGettingPostsByWriter()
        {
            //Arrange
            var posts = GetFakeDataPosts();
            var blogRepository = new Mock<IBlogRepository>();
            blogRepository.Setup(repo => repo.GetPosts()).ReturnsAsync(posts);

            var expectedListResponse = new List<Post>
            {
                new Post {  Id = 1, Title = "Post 1", Content = "This is the first post", WriterId = "UserId", Status = Status.Created },
                new Post {  Id = 2, Title = "Post 2", Content = "This is the second post", WriterId = "UserId", Status = Status.Pending},
                new Post {  Id = 3, Title = "Post 3", Content = "This is the third post", WriterId = "UserId", Status = Status.Pending}
            };

            var blogService = new BlogService(blogRepository.Object);

            //Act
            var responseListOfPosts = await blogService.GetPostsByWriter("UserId");

            //Assert

            Assert.AreEqual(responseListOfPosts.Count(), expectedListResponse.Count);
            Assert.AreEqual(responseListOfPosts.FirstOrDefault().Id, expectedListResponse.FirstOrDefault().Id);
            Assert.AreEqual(responseListOfPosts.FirstOrDefault().Title, expectedListResponse.FirstOrDefault().Title);
            Assert.AreEqual(responseListOfPosts.LastOrDefault().Id, expectedListResponse.LastOrDefault().Id);
            Assert.AreEqual(responseListOfPosts.LastOrDefault().Title, expectedListResponse.LastOrDefault().Title);
        }

        [Test]
        public Task SuccessCreatingPostCreateUpdatePost()
        {
            //Arrange
            var blogRepository = new Mock<IBlogRepository>();
            var post = new Post { Id = 0, Title = "Post 100", Content = "This is the new post", WriterId = "UserId", Status = Status.Created };
            blogRepository.Setup(repo => repo.CreatePost(It.IsAny<Post>())).Returns(true);

            var blogService = new BlogService(blogRepository.Object);

            //Act
            var response =  blogService.CreateUpdatePost(post);

            //Assert

            Assert.IsTrue(response);
        }

        [Test]
        public Task FailedCreatingPostCreateUpdatePost()
        {
            //Arrange
            var blogRepository = new Mock<IBlogRepository>();
            var post = new Post { Id = 0, Title = "Post 100", Content = "This is the new post", WriterId = "UserId", Status = Status.Created };
            blogRepository.Setup(repo => repo.CreatePost(It.IsAny<Post>())).Returns(false);

            var blogService = new BlogService(blogRepository.Object);

            //Act
            var response = blogService.CreateUpdatePost(post);

            //Assert

            Assert.IsFalse(response);
        }

        [Test]
        public Task SuccessUpdatingPostCreateUpdatePost()
        {
            //Arrange
            var posts = GetFakeDataPosts();
            var blogRepository = new Mock<IBlogRepository>();
            var post = new Post { Id = 1, Title = "Post 100", Content = "This is the edited post", WriterId = "UserId", Status = Status.Created };
            blogRepository.Setup(repo => repo.GetPostById(It.IsAny<int>())).Returns(posts.First());
            blogRepository.Setup(repo => repo.UpdatePost(It.IsAny<Post>())).Returns(true);
           

            var blogService = new BlogService(blogRepository.Object);

            //Act
            var response = blogService.CreateUpdatePost(post);

            //Assert

            Assert.IsTrue(response);
        }

        [Test]
        public Task FailedUpdatingPostCreateUpdatePost()
        {
            //Arrange
            var posts = GetFakeDataPosts();
            var blogRepository = new Mock<IBlogRepository>();
            var post = new Post { Id = 1, Title = "Post 100", Content = "This is the edited post", WriterId = "UserId", Status = Status.Created };
            blogRepository.Setup(repo => repo.GetPostById(It.IsAny<int>())).Returns(posts.First());
            blogRepository.Setup(repo => repo.UpdatePost(It.IsAny<Post>())).Returns(false);
           

            var blogService = new BlogService(blogRepository.Object);

            //Act
            var response = blogService.CreateUpdatePost(post);

            //Assert

            Assert.IsFalse(response);
        }

        [Test]
        public Task SuccessUpdatingStatePost()
        {
            //Arrange
            var posts = GetFakeDataPosts();
            var blogRepository = new Mock<IBlogRepository>();
            blogRepository.Setup(repo => repo.GetPostById(It.IsAny<int>())).Returns(posts.First());
            blogRepository.Setup(repo => repo.UpdatePost(It.IsAny<Post>())).Returns(true);


            var blogService = new BlogService(blogRepository.Object);

            //Act
            var response = blogService.UpdateStatePost(1, Status.Pending);

            //Assert

            Assert.IsTrue(response);
        }

        [Test]
        public Task FailedUpdatingStatePost()
        {
            //Arrange
            var posts = GetFakeDataPosts();
            var blogRepository = new Mock<IBlogRepository>();
            blogRepository.Setup(repo => repo.GetPostById(It.IsAny<int>())).Returns(posts.First());
            blogRepository.Setup(repo => repo.UpdatePost(It.IsAny<Post>())).Returns(false);


            var blogService = new BlogService(blogRepository.Object);

            //Act
            var response = blogService.UpdateStatePost(1, Status.Pending);

            //Assert

            Assert.IsFalse(response);
        }

        [Test]
        public Task SuccessCreatingComment()
        {
            //Arrange
            var blogRepository = new Mock<IBlogRepository>();
            var comment = new Comment { Id = 0, Name = "Oscar", Content = "This is the new comment" };
            blogRepository.Setup(repo => repo.CreateComment(It.IsAny<Comment>())).Returns(true);

            var blogService = new BlogService(blogRepository.Object);

            //Act
            var response = blogService.CreateComment(comment);

            //Assert

            Assert.IsTrue(response);
        }

        [Test]
        public Task FailedCreatingComment()
        {
            //Arrange
            var blogRepository = new Mock<IBlogRepository>();
            var comment = new Comment { Id = 0, Name = "Oscar", Content = "This is the new comment" };
            blogRepository.Setup(repo => repo.CreateComment(It.IsAny<Comment>())).Returns(false);

            var blogService = new BlogService(blogRepository.Object);

            //Act
            var response = blogService.CreateComment(comment);

            //Assert

            Assert.IsFalse(response);
        }


        private static List<Post> GetFakeDataPosts()
        {
            return new List<Post>
            {
                new Post {  Id = 1, Title = "Post 1", Content = "This is the first post", WriterId = "UserId", Status = Status.Created },
                new Post {  Id = 2, Title = "Post 2", Content = "This is the second post", WriterId = "UserId", Status = Status.Pending},
                new Post {  Id = 3, Title = "Post 3", Content = "This is the third post", WriterId = "UserId", Status = Status.Pending}
            };
        }
    }
}