using BetKanu.Models.Common;

namespace BetKanu.Models
{
    public class PartnerContributor
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string? LogoUrl { get; set; }

        public string? ShortDescription { get; set; }
        public string? LongDescription { get; set; }
        public string? WebsiteUrl { get; set; }

        public CollaborationCategory Category { get; set; }
        public string? PartnerType { get; set; }
        public int? Year { get; set; }

        public bool IsActive { get; set; } = true;
        public int DisplayOrder { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public ICollection<CollaborationProject> Projects
        { get; set; } = new List<CollaborationProject>();

        public ICollection<CollaborationMedia> Media { get; set; }
    = new List<CollaborationMedia>();
    }
}
