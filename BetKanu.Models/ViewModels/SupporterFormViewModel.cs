using BetKanu.Models.Common;
using System.ComponentModel.DataAnnotations;


namespace BetKanu.Models.ViewModels
{
    public sealed class SupporterFormViewModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Supporter name is required.")]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Choose a supporter type.")]
        [EnumDataType(typeof(SupporterType))]
        public SupporterType? Type { get; set; } = SupporterType.Monthly;

        [Required(ErrorMessage = "Choose a period.")]
        [EnumDataType(typeof(SupporterPeriod))]
        public SupporterPeriod? Period { get; set; } = SupporterPeriod.Current;

        [Range(1900, 2100, ErrorMessage = "Enter a valid recognition year.")]
        public int? RecognitionYear { get; set; } = DateTime.UtcNow.Year;

        public bool UnknownHistoricalYear { get; set; }

        [StringLength(120)]
        public string? DisplayLabel { get; set; }

        public bool IsPublished { get; set; }

        [Range(0, 100000)]
        public int DisplayOrder { get; set; }

        public static SupporterFormViewModel FromEntity(Supporter item) => new()
        {
            Id = item.Id,
            Name = item.Name,
            Type = item.Type,
            Period = Enum.IsDefined(typeof(SupporterPeriod), item.Period)
                ? (SupporterPeriod)item.Period
                : SupporterPeriod.Current,
            RecognitionYear = item.RecognitionYear > 0 ? item.RecognitionYear : null,
            UnknownHistoricalYear = item.RecognitionYear == 0,
            DisplayLabel = item.DisplayLabel,
            IsPublished = item.IsPublished,
            DisplayOrder = item.DisplayOrder
        };

        public Supporter ToEntity() => new()
        {
            Name = Name.Trim(),
            Type = Type!.Value,
            Period = (int)Period!.Value,
            RecognitionYear = UnknownHistoricalYear ? 0 : RecognitionYear!.Value,
            DisplayLabel = string.IsNullOrWhiteSpace(DisplayLabel) ? null : DisplayLabel.Trim(),
            IsPublished = IsPublished,
            DisplayOrder = DisplayOrder
        };
    }

    public sealed class SupporterAdminIndexViewModel
    {
        public string? Query { get; init; }
        public SupporterType? Type { get; init; }
        public int? Year { get; init; }
        public bool? Published { get; init; }
        public int? Period { get; init; }
        public List<Supporter> Items { get; init; } = new();
    }

    public sealed class SupporterCardViewModel
    {
        public string Name { get; init; } = string.Empty;
        public string Initials { get; init; } = string.Empty;
        public string Tag { get; init; } = string.Empty;
    }

    public sealed class SupporterSectionViewModel
    {
        public string Key { get; init; } = string.Empty;
        public SupporterType Type { get; init; }
        public bool Historical { get; init; }
        public IReadOnlyList<SupporterCardViewModel> Cards { get; init; } = Array.Empty<SupporterCardViewModel>();

        public string Title => Type switch
        {
            SupporterType.Monthly => "Monthly Supporters",
            SupporterType.Yearly => "Yearly Supporters",
            _ => "One-Time Donations"
        };
        public string Theme => Type == SupporterType.OneTime ? "orange" : "blue";
        public string Icon => Type switch
        {
            SupporterType.Monthly => "fa-refresh",
            SupporterType.Yearly => "fa-calendar-check-o",
            _ => "fa-heart"
        };
    }

    public sealed class SupportersPageViewModel
    {
        public int CurrentYear { get; init; }
        public IReadOnlyList<SupporterSectionViewModel> CurrentSections { get; init; } = Array.Empty<SupporterSectionViewModel>();
        public IReadOnlyList<SupporterSectionViewModel> HistoricalSections { get; init; } = Array.Empty<SupporterSectionViewModel>();

        public static SupportersPageViewModel FromEntities(IReadOnlyCollection<Supporter> items, int year)
        {
            var frequencies = new[] { SupporterType.Monthly, SupporterType.Yearly, SupporterType.OneTime };
            return new SupportersPageViewModel
            {
                CurrentYear = year,
                CurrentSections = frequencies.Select(type => BuildSection(items, type, historical: false)).ToList(),
                HistoricalSections = frequencies.Select(type => BuildSection(items, type, historical: true)).ToList()
            };
        }

        private static SupporterSectionViewModel BuildSection(
            IEnumerable<Supporter> items, SupporterType type, bool historical)
        {
            int period = historical ? (int)SupporterPeriod.Previous : (int)SupporterPeriod.Current;

            var matches = items.Where(x => x.Type == type && x.Period == period)
                .OrderBy(x => x.DisplayOrder)
                .ThenBy(x => x.Name);

            return new SupporterSectionViewModel
            {
                Key = $"{(historical ? "historic" : "current")}-{type.ToString().ToLowerInvariant()}",
                Type = type,
                Historical = historical,
                Cards = matches.Select(x => new SupporterCardViewModel
                {
                    Name = x.Name,
                    Initials = GetInitials(x.Name),
                    Tag = GetTag(x)
                }).ToList()
            };
        }

        private static string GetTag(Supporter item)
        {
            if (!string.IsNullOrWhiteSpace(item.DisplayLabel)) return item.DisplayLabel;
            string description = item.Type == SupporterType.OneTime
                ? "One-Time Donor"
                : $"{item.Type} Supporter";
            return item.RecognitionYear > 0
                ? $"{description} {item.RecognitionYear}"
                : description;
        }

        private static string GetInitials(string value)
        {
            var words = value.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (words.Length == 0) return "?";
            return words.Length == 1
                ? char.ToUpperInvariant(words[0][0]).ToString()
                : $"{char.ToUpperInvariant(words[0][0])}{char.ToUpperInvariant(words[^1][0])}";
        }
    }
}
