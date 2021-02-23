using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace IdentityServer
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime ScaringSince { get; set; }
        [Range(0,999)]
        public int Tentacles { get; set; }
        public string FirstName { get; set; }
        public string FamilyName { get; set; }
    }
}
