

namespace BetKanu.Models
{
    public class CollaborationProject
    {
        public int Id { get; set; }

        // Parent
        public int PartnerContributorId { get; set; }

        public PartnerContributor PartnerContributor
        { get; set; } = null!;

        // Project Information
        public string Title { get; set; } = string.Empty;
        public string? LogoUrl { get; set; }
        public string? ShortDescription { get; set; }
        public string? LongDescription { get; set; }
        public string? ProjectLink { get; set; }

        // Classification
        public string? ProjectType { get; set; }
        public int? BookId { get; set; }
        public int? Year { get; set; }

        // Page / Content Association
        public int? ContentPageId { get; set; }

        // Administration
        public bool IsActive { get; set; } = true;
        public int DisplayOrder { get; set; }

        public DateTime CreatedAt { get; set; }
            = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public ICollection<CollaborationMedia> Media { get; set; }
    = new List<CollaborationMedia>();
    }
}
