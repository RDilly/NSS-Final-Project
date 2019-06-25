using System;
using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumArtizen.Models
{
    public class AlbumPost
    {
        [Key]
        public int AlbumPostId { get; set; }

        [Required]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }


        [Required]
        [DataType(DataType.Date)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public System.DateTime DatePosted { get; set; }

        [Required]
        [StringLength(35)]
        public string Title { get; set; }

        [DisplayName("Upload Image")]
        public string ImagePath { get; set; }

    }
}
