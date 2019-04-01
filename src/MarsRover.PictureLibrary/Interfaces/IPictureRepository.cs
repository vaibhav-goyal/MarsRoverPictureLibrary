using MarsRover.PictureLibrary.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MarsRover.PictureLibrary.Interfaces
{
    public interface IPictureRepository
    {
        Task<PictureDTO> GetPictureAsync(string date, int pictureNo,CancellationToken cancelToken);   
    }
}
