using BlogEngine.Core.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace BlogEngine.Core.Contracts
{
    public interface IPost
    {
        int Id { get; set; }
        string Title { get; set; }
        string Content { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        Status Status { get; set; }
        string WriterId { get; set; }
        DateTimeOffset CreatedDate { get; set; }
        string? ApproverId { get; set; }
        DateTimeOffset? ApprovalDate { get; set; }

        ICollection<Comment>? Comments { get; set; }
    }
}
