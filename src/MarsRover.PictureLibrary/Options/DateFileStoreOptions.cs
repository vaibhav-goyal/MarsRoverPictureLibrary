using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarsRover.PictureLibrary.Options
{
    public class DateFileStoreOptions
    {
        public string FilePath { get; set; }
        public IFileProvider FileProvider { get; set; }
    }
}
