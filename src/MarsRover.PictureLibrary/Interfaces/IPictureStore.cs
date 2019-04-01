using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MarsRover.PictureLibrary.Interfaces
{
    public interface IPictureStore
    {
        Task<string> StorePictureAsync(long id,string remotePath, CancellationToken cancelToken);

        bool CheckExists(long id,string remotePath, out string localPath);
        
    }
}
