using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace StronglyTypeConfig.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AwesomeController : ControllerBase
    {
        public readonly AwesomeOptions awesomeOptions;
        public AwesomeController(IOptionsSnapshot<AwesomeOptions> awesomeOptions)
        {
            this.awesomeOptions = awesomeOptions.Value;
        }

        [HttpGet]
        public IActionResult GetAwesomeConfig()
        {
            return Ok(awesomeOptions);
        }
    }
}