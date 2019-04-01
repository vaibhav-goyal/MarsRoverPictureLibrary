using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarsRover.PictureLibrary.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MarsRover.PictureLibrary.Controllers
{
   
    [ApiController]
    public class ValidDatesController : ControllerBase
    {
        private IValidDatesProvider _validDatesProvider;

        public ValidDatesController(IValidDatesProvider validDatesProvider)
        {
            _validDatesProvider = validDatesProvider;
        }

        [Route("api/validdates")]
        [HttpGet]
        public IActionResult GetValidDates()
        {
            return Ok(_validDatesProvider.GetValidDates());
        }
    }
}