using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BetKanu.Models
{
    public class User : IdentityUser
    {
        public string?  Name { get; set; }
    }
}
