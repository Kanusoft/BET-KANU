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
        public string Subtitle { get; init; } = string.Empty;
        public IReadOnlyList<SupporterCardViewModel> Cards { get; init; } = Array.Empty<SupporterCardViewModel>();
        public string ColumnClass { get; set; } = "col-lg-4 col-md-6 col-sm-12";

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

    /// <summary>
    /// One public tab per recognition year for supporters in the "Current" period.
    /// </summary>
    public sealed class SupporterYearTabViewModel
    {
        public int Year { get; init; }
        public bool IsCurrentYear { get; init; }
        public bool IsUpcoming { get; init; }
        public string Key => $"year-{Year}";
        public IReadOnlyList<SupporterSectionViewModel> Sections { get; init; } = Array.Empty<SupporterSectionViewModel>();
    }

    public sealed class SupportersPageViewModel
    {
        public const string PreviousTabKey = "previous";

        public int CurrentYear { get; init; }
        public IReadOnlyList<SupporterYearTabViewModel> YearTabs { get; init; } = Array.Empty<SupporterYearTabViewModel>();
        public IReadOnlyList<SupporterSectionViewModel> HistoricalSections { get; init; } = Array.Empty<SupporterSectionViewModel>();
        public string DefaultTabKey { get; init; } = PreviousTabKey;

        public static SupportersPageViewModel FromEntities(IReadOnlyCollection<Supporter> items, int year)
        {
            var current = items.Where(x => x.Period == (int)SupporterPeriod.Current).ToList();
            var previous = items.Where(x => x.Period == (int)SupporterPeriod.Previous).ToList();

            // Every recognition year used by a current supporter gets its own tab (newest first).
            var years = current.Where(x => x.RecognitionYear > 0)
                .Select(x => x.RecognitionYear)
                .Distinct()
                .OrderByDescending(x => x)
                .ToList();
            if (years.Count == 0) years.Add(year);

            // Open the calendar year's tab by default; otherwise the newest year.
            int defaultYear = years.Contains(year) ? year : years[0];

            var tabs = years.Select(tabYear =>
            {
                // Current supporters without a known year (legacy, stored as 0) show in the default tab.
                var tabItems = current.Where(x => x.RecognitionYear == tabYear
                    || (x.RecognitionYear <= 0 && tabYear == defaultYear));

                return new SupporterYearTabViewModel
                {
                    Year = tabYear,
                    IsCurrentYear = tabYear == year,
                    IsUpcoming = tabYear > year,
                    Sections = BuildSections(tabItems, historical: false, $"year-{tabYear}", $"{tabYear} Supporters")
                };
            }).ToList();

            return new SupportersPageViewModel
            {
                CurrentYear = year,
                YearTabs = tabs,
                HistoricalSections = BuildSections(previous, historical: true, "historic", "All Previous Years"),
                DefaultTabKey = $"year-{defaultYear}"
            };
        }

        private static IReadOnlyList<SupporterSectionViewModel> BuildSections(
            IEnumerable<Supporter> items, bool historical, string keyPrefix, string subtitle)
        {
            var list = items.ToList();
            var frequencies = new[] { SupporterType.Monthly, SupporterType.Yearly, SupporterType.OneTime };

            // Hide the Yearly column when it has no supporters, so the remaining columns fill the row.
            var sections = frequencies
                .Select(type => BuildSection(list, type, historical, keyPrefix, subtitle))
                .Where(section => section.Type != SupporterType.Yearly || section.Cards.Count > 0)
                .ToList();

            string columnClass = sections.Count == 2 ? "col-lg-6 col-md-6 col-sm-12" : "col-lg-4 col-md-6 col-sm-12";
            foreach (var section in sections) section.ColumnClass = columnClass;

            return sections;
        }

        private static SupporterSectionViewModel BuildSection(
            IEnumerable<Supporter> items, SupporterType type, bool historical, string keyPrefix, string subtitle)
        {
            var matches = items.Where(x => x.Type == type)
                .OrderBy(x => x.DisplayOrder)
                .ThenBy(x => x.Name);

            return new SupporterSectionViewModel
            {
                Key = $"{keyPrefix}-{type.ToString().ToLowerInvariant()}",
                Type = type,
                Historical = historical,
                Subtitle = subtitle,
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
