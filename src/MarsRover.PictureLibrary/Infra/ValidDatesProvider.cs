using MarsRover.PictureLibrary.Interfaces;
using MarsRover.PictureLibrary.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MarsRover.PictureLibrary.Infra
{
    public class ValidDatesProvider : IValidDatesProvider
    {
        private List<string> _validDates;

        public ValidDatesProvider(IOptions<DateFileStoreOptions> options)
        {
            _validDates = new List<string>();
            var filePath = options.Value.FilePath;
            var fileInfo = options.Value.FileProvider.GetFileInfo(filePath);
            var culture = CultureInfo.CreateSpecificCulture("en-US");
            var styles = DateTimeStyles.AllowWhiteSpaces;
            var format = "yyyy-MM-dd";
            if (fileInfo.Exists)
            {
                using (var stream = fileInfo.CreateReadStream())
                {
                    using (StreamReader sr = new StreamReader(stream))
                    {
                        string dateString;
                        while ((dateString = sr.ReadLine()) != null)
                        {
                            DateTime date;
                            if (DateTime.TryParse(dateString, culture, styles, out date))
                            {
                                _validDates.Add(date.ToString(format,culture));
                            }
                        }

                    }
                }
            }
            
        }

        public ReadOnlyCollection<string> GetValidDates()
        {
            return _validDates.AsReadOnly();
        }
    }
}
