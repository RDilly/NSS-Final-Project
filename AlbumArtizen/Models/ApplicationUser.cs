using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace AlbumArtizen.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser() { }



        [Required]
        [Display(Name = "Displayname")]
        public string DisplayName { get; set; }
    }
}