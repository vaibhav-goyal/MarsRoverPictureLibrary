using MarsRover.PictureLibrary.DTOs;
using MarsRover.PictureLibrary.Interfaces;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace MarsRover.PictureLibrary.Infra
{
    public class NASAClient : INASAClient
    {
        private readonly HttpClient _httpClient;

        private readonly string _apiKey;
        private readonly string _basePath = "mars-photos/api/v1/rovers/curiosity/photos";

        public NASAClient(HttpClient httpClient,IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["NASAAPIKey"];
        }

        public int GetPageSize()
        {
            return 25;
        }

        public async Task<List<Photo>> GetPhotosAsync(string earthDate, int page, CancellationToken cancelToken = default(CancellationToken))
        {           
            var qb = new QueryBuilder();
            qb.Add("earth_date", earthDate);
            qb.Add("page", page.ToString());
            qb.Add("api_key", _apiKey);
            var path = _basePath + qb.ToString();

            var response = await _httpClient.GetAsync(path,cancelToken);

            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsAsync<PhotosResponse>(cancelToken);

            return result.Photos;
        }
    }
}
