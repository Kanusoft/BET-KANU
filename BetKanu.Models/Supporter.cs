using BetKanu.Models.Common;
using System.ComponentModel.DataAnnotations;


namespace BetKanu.Models
{
    public sealed class Supporter
    {
        public int Id { get; set; }

        [Required, StringLength(200)]
        public string Name { get; set; } = string.Empty;

        public SupporterType Type { get; set; }

        /// <summary>
        /// 1 = Current supporters, 2 = Previous / historical supporters.
        /// </summary>
        public int Period { get; set; } = 1;

        /// <summary>
        /// Recognition year. Legacy historical records may use 0 when the year is unknown.
        /// </summary>
        public int RecognitionYear { get; set; }

        // Optional, preserves labels such as "PayPal Recurring 2026" from the old page.
        [StringLength(120)]
        public string? DisplayLabel { get; set; }

        public bool IsPublished { get; set; } = false;
        public int DisplayOrder { get; set; } = 0;
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAtUtc { get; set; }
    }
}
