using BlogEngine.Core.Contracts;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace BlogEngine.Core.Models
{
    public class Post: IPost
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public Status Status { get; set; }
        public string WriterId { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string? ApproverId { get; set; }
        public DateTimeOffset? ApprovalDate { get; set; }
        public virtual ICollection<Comment>? Comments { get; set; }
    }
}
