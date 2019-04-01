using MarsRover.PictureLibrary.DTOs;
using MarsRover.PictureLibrary.Exceptions;
using MarsRover.PictureLibrary.Interfaces;
using MarsRover.PictureLibrary.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MarsRover.PictureLibrary.Repository
{
    public class PictureRepository : IPictureRepository
    {
        private readonly IPictureStore _pictureStore;
        private readonly INASAClient _nasaClient;
        private readonly IValidDatesProvider _validDatesProvider;

        public PictureRepository(IPictureStore pictureStore, INASAClient nasaClient,IValidDatesProvider validDatesProvider)
        {
            _pictureStore = pictureStore;
            _nasaClient = nasaClient;
            _validDatesProvider = validDatesProvider;
        }

        public async Task<PictureDTO> GetPictureAsync(string date, int pictureNo, CancellationToken cancelToken = default(CancellationToken))
        {           
            if (string.IsNullOrWhiteSpace(date))
                throw new DomainException("Null date", new ArgumentNullException(nameof(date)));

            if(_validDatesProvider.GetValidDates().Count > 0 && !_validDatesProvider.GetValidDates().Contains(date))
                throw new DomainException("Invalid date", new ArgumentOutOfRangeException(nameof(date)));

            if (pictureNo < 1)
                throw new DomainException("Invalid pictureNo", new ArgumentOutOfRangeException(nameof(pictureNo)));

            var pageSize = _nasaClient.GetPageSize();
            var page = ((pictureNo -1)/pageSize) + 1;

            var photos = await _nasaClient.GetPhotosAsync(date, page,cancelToken);
            if (photos == null || photos.Count == 0)
                return null;

            var itemIndex = pictureNo - ((page - 1) * pageSize) - 1;

            if (itemIndex > (photos.Count - 1))
                return null;

            var photo = photos[itemIndex];
            var localPath = string.Empty;
            if(!_pictureStore.CheckExists(photo.Id,photo.Img_src,out localPath))
            {
                localPath = await _pictureStore.StorePictureAsync(photo.Id, photo.Img_src,cancelToken);
            }

            return new PictureDTO { Id = photo.Id, Path = localPath };
        }
        
    }
}
