using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AwesomeSauceApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AwesomeSauceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        [ModelBinder(typeof(AwesomeModelBinder))]
        public class EmotionalPhoto
        {
            public byte[] Contents { get; set; }
            public EmotionScoresDto Scores { get; set; }
        }

        public IActionResult Post(EmotionalPhoto item)
        {
            return Ok();
        }
    }
}