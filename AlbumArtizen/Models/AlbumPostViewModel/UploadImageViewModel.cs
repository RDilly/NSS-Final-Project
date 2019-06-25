using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumArtizen.Models.AlbumPostViewModel
{
    public class UploadImageViewModel
    {
        public AlbumPost AlbumPost { get; set; }


        public IFormFile ImageFile { get; set; }


    }
}
