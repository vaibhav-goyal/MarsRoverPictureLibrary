using MarsRover.PictureLibrary.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
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
        private readonly string _folderPath = "client-app/public/assets/photos";
        private readonly IFileProvider _fileProvider;
        private readonly HttpClient _httpClient;
        private readonly IHostingEnvironment _hostingEnvironment;

        public PictureStore(IFileProvider fileProvider, HttpClient httpClient,IHostingEnvironment hostingEnvironment)
        {
            _fileProvider = fileProvider;
            _httpClient = httpClient;
        }

        public bool CheckExists(long id, out string localPath)
        {
            localPath = null;
            var contents = _fileProvider.GetDirectoryContents(_folderPath);
            if (contents == null)
                return false;

            var files = contents.ToList();
            if (files?.Count == 0)
                return false;

            var file = files.First(f => f.Name.StartsWith(id.ToString()));
            if (file?.Exists == true)
            {

                localPath = file.PhysicalPath;
                return true;
            }
            return false;

        }

        public async Task<string> StorePictureAsync(long id, string remotePath,CancellationToken cancelToken)
        {            
            var response = await _httpClient.GetAsync(remotePath,cancelToken);
            response.EnsureSuccessStatusCode();

            var fileExtn = remotePath.Substring(remotePath.LastIndexOf("."));
            var filePath = _folderPath + "/" + id + fileExtn;
            var fileInfo = _fileProvider.GetFileInfo(filePath);

            var photoBytes = await response.Content.ReadAsByteArrayAsync();            
            using(var file = System.IO.File.Create(fileInfo.PhysicalPath))
            {
                await file.WriteAsync(photoBytes, 0, photoBytes.Length, cancelToken);
            }
            fileInfo = _fileProvider.GetFileInfo(filePath);
            return fileInfo.PhysicalPath;
        }
    }
}
