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
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DateCreated { get; set; }

        [Required]
        public int AlbumPostId { get; set; }

        [Required]
        public string UserId { get; set; }


        public ApplicationUser User { get; set; }



        [Required]
        [StringLength(55)]
        public string CommentBody { get; set; }
               

    }
}
