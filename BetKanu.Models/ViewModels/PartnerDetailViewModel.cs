using System.Collections.Generic;

namespace BetKanu.Models.ViewModels
{
    public class JointWorkItem
    {
        public string Title { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string TargetUrl { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class PartnerDetailViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string LogoUrl { get; set; } = string.Empty;
        public string WebsiteUrl { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string> Photos { get; set; } = new List<string>();
        public List<JointWorkItem> JointWorks { get; set; } = new List<JointWorkItem>();
    }
}
