
using System.ComponentModel.DataAnnotations;

namespace BET_KANU.Models
{
    public class PartnershipInquiryRequest
    {
        [Required]
        [StringLength(120)]
        public string Name { get; set; }

        [Required]
        [StringLength(160)]
        public string Organization { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(254)]
        public string Email { get; set; }

        public List<string> HelpTypes { get; set; }

        [StringLength(4000)]
        public string Message { get; set; }
    }
}
