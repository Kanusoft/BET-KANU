using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BetKanu.Models
{
    public class User 
    {
        public int Id { get; set; }
        public string?  Email { get; set; }
        public string? PassWord { get; set; }
        public bool KeepLoggedIn { get; set; }
    }
}
