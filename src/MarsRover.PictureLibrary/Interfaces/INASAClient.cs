using MarsRover.PictureLibrary.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MarsRover.PictureLibrary.Interfaces
{
    public interface INASAClient
    {
        Task<List<Photo>> GetPhotosAsync(string earthDate, int page,CancellationToken cancelToken);

        int GetPageSize();
    }
}
