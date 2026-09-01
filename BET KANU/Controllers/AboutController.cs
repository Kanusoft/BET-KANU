using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using BetKanu.Models.ViewModels;

namespace BET_KANU.Controllers
{
    public class AboutController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Team()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult BETKANU()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Partners()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult PartnerDetails(string id)
        {
            var partnerModel = GetPartnerData(id);
            return View(partnerModel);
        }

        public ActionResult Contributors()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult OurCenter()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Gallery()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Volunteers()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult SpecialThanks()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Supporters()
        {
            ViewBag.Message = "Supporters";

            return View();
        }

        public ActionResult Support()
        {
            return View("Supporters");
        }

        public ActionResult AkkadSaadi()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult GabiChabo()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult MariaKale()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult DilamaMalki()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult RamiaAhe()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult JackAbdelMassih()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult YousipToma()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        private PartnerDetailViewModel GetPartnerData(string id)
        {
            id = (id ?? "").ToLower().Trim();

            if (id == "afa" || id == "assyrianfoundationofamerica" || id.Contains("assyrian foundation"))
            {
                return new PartnerDetailViewModel
                {
                    Id = "afa",
                    Name = "Assyrian Foundation of America",
                    Category = "Culture & Language Sponsor",
                    LogoUrl = "/img/partners/Assyrian Foundation of America.jpg",
                    WebsiteUrl = "https://assyrianfoundation.org/",
                    Description = "The Assyrian Foundation of America, founded in 1964, is a non-profit organization dedicated to the promotion and preservation of Assyrian heritage, language, and culture worldwide through educational grants, publishing support, and community development.",
                    Photos = new List<string>
                    {
                        "/img/partners/Assyrian Foundation of America.jpg",
                        "/img/partners/AANF.jpg"
                    },
                    JointWorks = new List<JointWorkItem>
                    {
                        new JointWorkItem
                        {
                            Title = "Syriac Educational Media & Apps",
                            Category = "Software & Apps",
                            ImageUrl = "/img/partners/Assyrian Foundation of America.jpg",
                            TargetUrl = "/products/Index",
                            Description = "Collaborative initiative producing interactive educational applications and digital media tools for children."
                        },
                        new JointWorkItem
                        {
                            Title = "Syriac Children Songs & Animation",
                            Category = "Studio Production",
                            ImageUrl = "/img/partners/AANF.jpg",
                            TargetUrl = "/Studio/Index",
                            Description = "Production of animated songs and educational cartoon episodes dedicated to Syriac language learners."
                        }
                    }
                };
            }

            if (id == "shlama" || id.Contains("shlama"))
            {
                return new PartnerDetailViewModel
                {
                    Id = "shlama",
                    Name = "Shlama Foundation",
                    Category = "Community & Humanitarian Partner",
                    LogoUrl = "/img/partners/Shlama Foundation.jpg",
                    WebsiteUrl = "https://www.shlama.org/",
                    Description = "Shlama Foundation connects the diaspora with Assyrian communities in the homeland through impactful projects, education, and community initiatives.",
                    Photos = new List<string> { "/img/partners/Shlama Foundation.jpg" },
                    JointWorks = new List<JointWorkItem>
                    {
                        new JointWorkItem
                        {
                            Title = "Homeland Language Materials",
                            Category = "Educational Resources",
                            ImageUrl = "/img/partners/Shlama Foundation.jpg",
                            TargetUrl = "/products/Index",
                            Description = "Distribution of Syriac books and digital tools to schools in Assyrian villages."
                        }
                    }
                };
            }

            if (id == "capni" || id == "cap" || id.Contains("capni") || id.Contains("christian aid"))
            {
                return new PartnerDetailViewModel
                {
                    Id = "capni",
                    Name = "CAPNI (Christian Aid Program)",
                    Category = "Educational & Cultural Partner",
                    LogoUrl = "/img/partners/capni2.jpg",
                    WebsiteUrl = "https://capni-iraq.org/",
                    Description = "CAPNI is a key non-governmental organization working to empower communities through education, heritage protection, and youth programs.",
                    Photos = new List<string> { "/img/partners/capni2.jpg" },
                    JointWorks = new List<JointWorkItem>
                    {
                        new JointWorkItem
                        {
                            Title = "Culture & Language Workshops",
                            Category = "Community Projects",
                            ImageUrl = "/img/partners/capni2.jpg",
                            TargetUrl = "/about/BETKANU",
                            Description = "Co-organized Syriac learning workshops and educational media distributions."
                        }
                    }
                };
            }

            if (id == "ajm" || id.Contains("ajm"))
            {
                return new PartnerDetailViewModel
                {
                    Id = "ajm",
                    Name = "AJM - Assyrischer Jugendverband Mitteleuropa e.V.",
                    Category = "Youth & Cultural Partner",
                    LogoUrl = "/img/partners/ajm.jpg",
                    WebsiteUrl = "http://www.ajmev.org/",
                    Description = "Assyrischer Jugendverband Mitteleuropa (AJM) engages young Assyrians across Europe in cultural projects, youth camps, and language revitalization.",
                    Photos = new List<string> { "/img/partners/ajm.jpg" },
                    JointWorks = new List<JointWorkItem>()
                };
            }

            if (id == "etuti" || id.Contains("etuti"))
            {
                return new PartnerDetailViewModel
                {
                    Id = "etuti",
                    Name = "Etuti Institute",
                    Category = "Educational Partner",
                    LogoUrl = "/img/partners/Etuti.jpg",
                    WebsiteUrl = "https://www.etuti.org/",
                    Description = "Etuti Institute focuses on language research, educational tools, and youth leadership.",
                    Photos = new List<string> { "/img/partners/Etuti.jpg" },
                    JointWorks = new List<JointWorkItem>()
                };
            }

            if (id == "naby" || id == "nfafc" || id.Contains("naby"))
            {
                return new PartnerDetailViewModel
                {
                    Id = "naby",
                    Name = "Naby Frye Assyrian Fund for Culture",
                    Category = "Cultural & Educational Fund",
                    LogoUrl = "/img/partners/Naby.jpg",
                    WebsiteUrl = "https://www.nabyfryeculturefund.org/",
                    Description = "The Naby Frye Assyrian Fund for Culture supports arts, language preservation, literature, and educational initiatives for the Assyrian community.",
                    Photos = new List<string> { "/img/partners/Naby.jpg" },
                    JointWorks = new List<JointWorkItem>()
                };
            }

            if (id == "sos" || id.Contains("sos"))
            {
                return new PartnerDetailViewModel
                {
                    Id = "sos",
                    Name = "SOS 1915 (Save Our Souls)",
                    Category = "Humanitarian & Cultural Partner",
                    LogoUrl = "/img/partners/SOS.jpg",
                    WebsiteUrl = "https://1915.de/",
                    Description = "SOS 1915 is dedicated to historical awareness, cultural preservation, and supporting education and humanitarian initiatives.",
                    Photos = new List<string> { "/img/partners/SOS.jpg" },
                    JointWorks = new List<JointWorkItem>()
                };
            }

            if (id == "syriac-culture" || id.Contains("syriac") || id.Contains("culture"))
            {
                return new PartnerDetailViewModel
                {
                    Id = "syriac-culture",
                    Name = "General Directorate of Syriac Culture and Arts",
                    Category = "Governmental Cultural Body",
                    LogoUrl = "/img/partners/Syriac-heritage-museum_logo-.png",
                    WebsiteUrl = "#",
                    Description = "The General Directorate of Syriac Culture and Arts works on safeguarding Syriac heritage, literature, theater, and arts.",
                    Photos = new List<string> { "/img/partners/Syriac-heritage-museum_logo-.png" },
                    JointWorks = new List<JointWorkItem>()
                };
            }

            if (id == "attratv" || id.Contains("attra"))
            {
                return new PartnerDetailViewModel
                {
                    Id = "attratv",
                    Name = "ATTRA TV",
                    Category = "Media & Broadcast Partner",
                    LogoUrl = "/img/partners/AttraTV.png",
                    WebsiteUrl = "#",
                    Description = "ATTRA TV is a broadcast media network highlighting Assyrian culture, news, educational programming, and language media.",
                    Photos = new List<string> { "/img/partners/AttraTV.png" },
                    JointWorks = new List<JointWorkItem>()
                };
            }

            if (id == "assyrians-without-borders" || id.Contains("borders"))
            {
                return new PartnerDetailViewModel
                {
                    Id = "assyrians-without-borders",
                    Name = "Assyrians Without Borders",
                    Category = "Humanitarian & Community Partner",
                    LogoUrl = "/img/partners/Assyrians Without Borders.png",
                    WebsiteUrl = "#",
                    Description = "Assyrians Without Borders supports humanitarian aid, youth education, and community infrastructure in the homeland.",
                    Photos = new List<string> { "/img/partners/Assyrians Without Borders.png" },
                    JointWorks = new List<JointWorkItem>()
                };
            }

            if (id == "tony-kalogerakos" || id.Contains("tony"))
            {
                return new PartnerDetailViewModel
                {
                    Id = "tony-kalogerakos",
                    Name = "Tony S. Kalogerakos",
                    Category = "Benefactor & Contributor",
                    LogoUrl = "/img/partners/Tony S. Kalogerakos.jpg",
                    WebsiteUrl = "#",
                    Description = "Tony S. Kalogerakos is a prominent supporter and contributor to BET KANU's language preservation and educational media initiatives.",
                    Photos = new List<string> { "/img/partners/Tony S. Kalogerakos.jpg" },
                    JointWorks = new List<JointWorkItem>()
                };
            }

            if (id == "ecews" || id.Contains("ecews"))
            {
                return new PartnerDetailViewModel
                {
                    Id = "ecews",
                    Name = "ECEWS",
                    Category = "Educational Contributor",
                    LogoUrl = "/img/partners/ecews.jpg",
                    WebsiteUrl = "#",
                    Description = "ECEWS is an educational partner supporting language learning and early childhood educational media.",
                    Photos = new List<string> { "/img/partners/ecews.jpg" },
                    JointWorks = new List<JointWorkItem>()
                };
            }

            if (id == "mor-afrem" || id.Contains("afrem"))
            {
                return new PartnerDetailViewModel
                {
                    Id = "mor-afrem",
                    Name = "MOR AFREM",
                    Category = "Cultural & Religious Contributor",
                    LogoUrl = "/img/partners/morafrem.jpg",
                    WebsiteUrl = "#",
                    Description = "MOR AFREM institution supports Syriac language publishing, heritage archiving, and youth education.",
                    Photos = new List<string> { "/img/partners/morafrem.jpg" },
                    JointWorks = new List<JointWorkItem>()
                };
            }

            if (id == "icrc" || id.Contains("icrc"))
            {
                return new PartnerDetailViewModel
                {
                    Id = "icrc",
                    Name = "ICRC",
                    Category = "International Contributor",
                    LogoUrl = "/img/partners/ICRC.jpg",
                    WebsiteUrl = "#",
                    Description = "The International Committee of the Red Cross (ICRC) supports humanitarian initiatives and educational projects across communities.",
                    Photos = new List<string> { "/img/partners/ICRC.jpg" },
                    JointWorks = new List<JointWorkItem>()
                };
            }

            if (id == "aaam" || id.Contains("aaam"))
            {
                return new PartnerDetailViewModel
                {
                    Id = "aaam",
                    Name = "AAAM",
                    Category = "Community Contributor",
                    LogoUrl = "/img/partners/aaam.jpg",
                    WebsiteUrl = "#",
                    Description = "AAAM is a community organization supporting Assyrian culture, heritage, and youth projects.",
                    Photos = new List<string> { "/img/partners/aaam.jpg" },
                    JointWorks = new List<JointWorkItem>()
                };
            }

            if (id == "dolabani" || id.Contains("dolabani"))
            {
                return new PartnerDetailViewModel
                {
                    Id = "dolabani",
                    Name = "DOLABANI",
                    Category = "Cultural Contributor",
                    LogoUrl = "/img/partners/dolabani.jpg",
                    WebsiteUrl = "#",
                    Description = "DOLABANI foundation supports Syriac literary heritage, research, and educational resources.",
                    Photos = new List<string> { "/img/partners/dolabani.jpg" },
                    JointWorks = new List<JointWorkItem>()
                };
            }

            if (id == "elias-hanna" || id.Contains("elias"))
            {
                return new PartnerDetailViewModel
                {
                    Id = "elias-hanna",
                    Name = "ELIAS HANNA",
                    Category = "Individual Contributor",
                    LogoUrl = "/img/partners/eliashanna.jpg",
                    WebsiteUrl = "#",
                    Description = "Elias Hanna is a dedicated contributor supporting BET KANU's media, music, and educational productions.",
                    Photos = new List<string> { "/img/partners/eliashanna.jpg" },
                    JointWorks = new List<JointWorkItem>()
                };
            }

            if (id == "oeuvre-dorient" || id.Contains("orient"))
            {
                return new PartnerDetailViewModel
                {
                    Id = "oeuvre-dorient",
                    Name = "OEUVRE - DORIENT",
                    Category = "International Cultural Partner",
                    LogoUrl = "/img/partners/Oeuvre-dOrient.jpg",
                    WebsiteUrl = "#",
                    Description = "L'Œuvre d'Orient supports Eastern Christian communities, heritage preservation, and language education.",
                    Photos = new List<string> { "/img/partners/Oeuvre-dOrient.jpg" },
                    JointWorks = new List<JointWorkItem>()
                };
            }

            // Fallback for any other partner/contributor
            string formattedTitle = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(id.Replace("-", " ").Replace("_", " "));
            return new PartnerDetailViewModel
            {
                Id = id,
                Name = string.IsNullOrWhiteSpace(formattedTitle) ? "Valued Contributor" : formattedTitle,
                Category = "Strategic Partner & Contributor",
                LogoUrl = "/img/partners/Assyrian Foundation of America.jpg",
                WebsiteUrl = "#",
                Description = "BET KANU is proud to collaborate with organizations, institutions, and contributors worldwide to preserve, advance, and teach the Syriac language.",
                Photos = new List<string> { "/img/partners/Assyrian Foundation of America.jpg" },
                JointWorks = new List<JointWorkItem>()
            };
        }
    }
}
