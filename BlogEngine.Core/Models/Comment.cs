using System;

namespace BlogEngine.Core.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int PostId { get; set; }
        public DateTimeOffset CreatedDate { get; set; }

    }
}