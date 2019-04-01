using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MarsRover.PictureLibrary.Exceptions;
using MarsRover.PictureLibrary.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MarsRover.PictureLibrary.Controllers
{
    [Route("api/pictures")]
    [ApiController]
    public class PicturesController : ControllerBase
    {
        private readonly IPictureRepository _pictureRepository;
        private readonly ILogger<PicturesController> _logger;
        private readonly IHostingEnvironment _hostingEnvironment;

        public PicturesController(IPictureRepository pictureRepository,ILogger<PicturesController> logger,IHostingEnvironment hostingEnvironment)
        {
            _pictureRepository = pictureRepository;
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
        }

        
        [HttpGet]
        public async Task<IActionResult> GetPictures([FromQuery]string date,[FromQuery]int pictureNo = 1, CancellationToken cancelToken = default(CancellationToken))
        {
            try
            {
                var result = await _pictureRepository.GetPictureAsync(date, pictureNo, cancelToken);
                if (result == null)
                {
                    var notFoundMessage = $"Picture no : {pictureNo} for date : {date} not found";
                    _logger.LogWarning(notFoundMessage);
                    return NotFound(notFoundMessage);
                }
                result.Path = GetVirtualPath(result.Path);
                return Ok(result);
            }
            catch (DomainException de)
            {
                _logger.LogWarning(de.Message);
                return BadRequest(de.Message);
            }
        }

        private string GetVirtualPath(string path)
        {
            var root = _hostingEnvironment.ContentRootPath;
            var relativePath =  Path.GetRelativePath(root,path);
            return relativePath.Replace("\\", "/").Insert(0, "/");            
        }
    }
}
