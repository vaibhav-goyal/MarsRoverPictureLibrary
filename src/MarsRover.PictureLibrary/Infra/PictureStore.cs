using MarsRover.PictureLibrary.Interfaces;
using MarsRover.PictureLibrary.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace MarsRover.PictureLibrary.Infra
{
    public class PictureStore : IPictureStore
    {
        private readonly ImageStoreOptions _options;
        private readonly HttpClient _httpClient;
        public PictureStore(IOptions<ImageStoreOptions> options, HttpClient httpClient)
        {
            _options = options.Value;
            _httpClient = httpClient;
        }

        public bool CheckExists(long id,string remotePath, out string localPath)
        {
            localPath = null;
            var contents = _options.FileProvider.GetDirectoryContents(_options.ImageStoragePath);
            if (contents == null)
                return false;

            var fileExtn = remotePath.Substring(remotePath.LastIndexOf("."));
            var filePath = Path.Combine(_options.ImageStoragePath, id + fileExtn);
            var fileInfo = _options.FileProvider.GetFileInfo(filePath);
            if (fileInfo != null && fileInfo.Exists)
            {
                localPath = fileInfo.PhysicalPath;
                return true;
            }
            return false;
        }

        public async Task<string> StorePictureAsync(long id, string remotePath,CancellationToken cancelToken)
        {            
            var response = await _httpClient.GetAsync(remotePath,cancelToken);
            response.EnsureSuccessStatusCode();

            var fileExtn = remotePath.Substring(remotePath.LastIndexOf("."));
            var filePath = Path.Combine(_options.ImageStoragePath, id + fileExtn);
            var fileInfo = _options.FileProvider.GetFileInfo(filePath);

            var photoBytes = await response.Content.ReadAsByteArrayAsync();            
            using(var file = System.IO.File.Create(fileInfo.PhysicalPath))
            {
                await file.WriteAsync(photoBytes, 0, photoBytes.Length, cancelToken);
            }
            fileInfo = _options.FileProvider.GetFileInfo(filePath);
            return fileInfo.PhysicalPath;
        }
    }
}
