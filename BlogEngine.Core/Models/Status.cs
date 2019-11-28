using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace BlogEngine.Core.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Status
    {
        [EnumMember(Value = "Created")]
        Created,
        [EnumMember(Value = "Pending")]
        Pending,
        [EnumMember(Value = "Published")]
        Published,
        [EnumMember(Value = "Rejected")]
        Rejected,
        [EnumMember(Value = "Deleted")]
        Deleted
    }
}
