

using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace BetKanu.Models
{
    public class BKRBundle
    {
        [JsonPropertyName("ImageURL")]
        public string? ImageURL { get; set; }

        [JsonPropertyName("TextURL")]
        public string? TextURL { get; set; }

        [JsonPropertyName("TextLanguage")]
        public string? TextLanguage { get; set; }

        [JsonPropertyName("AudioURL")]
        public string? AudioURL { get; set; }

        [JsonPropertyName("VideoURL")]
        public string? VideoURL { get; set; }

        [JsonPropertyName("ExternalVideo")]
        public string? ExternalVideo { get; set; }

        [JsonPropertyName("InternalVideo")]
        public string? InternalVideo { get; set; }

        [JsonPropertyName("ExternalURL")]
        public string? ExternalURL { get; set; }

        [JsonPropertyName("ExternalURLName")]
        public string? ExternalURLName { get; set; }

        [JsonPropertyName("ExternalVideoName")]
        public string? ExternalVideoName { get; set; }

        [JsonPropertyName("NewPageNo")]
        public int NewPageNo { get; set; }

        [JsonPropertyName("NewSecNo")]
        public int NewSecNo { get; set; }

        [JsonPropertyName("Book")]
        public Book? Book { get; set; }

        [JsonPropertyName("IsFirst")]
        public bool IsFirst { get; set; }

        [JsonPropertyName("IsLast")]
        public bool IsLast { get; set; }
    }
}
