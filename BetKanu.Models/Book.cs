using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BetKanu.Models
{
    public class Book
    {
        [Key]
        [JsonPropertyName("Id")]
        public int Id { get; set; }

        [JsonPropertyName("Name")]
        public string? Name { get; set; }

        [JsonPropertyName("URL")]
        public string? URL { get; set; }

        [JsonPropertyName("Dialect")]
        public string? Dialect { get; set; }

        [JsonPropertyName("ShortDescription")]
        public string? ShortDescription { get; set; }

        [JsonPropertyName("LongDescription")]
        public string? LongDescription { get; set; }

        [JsonPropertyName("IsReleased")]
        public bool IsReleased { get; set; }

        [JsonPropertyName("IsDownloadable")]
        public bool IsDownloadable { get; set; }

        [JsonPropertyName("DownloadableDocument")]
        public string? DownloadableDocument { get; set; }

        [JsonPropertyName("ProjectURL")]
        public string? ProjectURL { get; set; }

        [JsonPropertyName("BookImageURL")]
        public string? BookImageURL { get; set; }
    }
}
