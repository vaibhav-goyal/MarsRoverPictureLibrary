using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarsRover.PictureLibrary.DTOs
{
    public class PhotosResponse
    {
        public List<Photo> Photos { get; set; }
    }

    public class Photo
    {
        public long Id { get; set; }
        public string Img_src { get; set; }
    }
}
