using System.Collections.Generic;
using AlbumArtizen.Models;
using AlbumArtizen.Data;

namespace Bangazon.Models.ProductViewModels
{
    public class AlbumPostListViewModel
    {
        public IEnumerable<AlbumPost> AlbumPost { get; set; }
    }
}