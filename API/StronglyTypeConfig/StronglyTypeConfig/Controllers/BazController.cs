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
    public class BazController : ControllerBase
    {
        private readonly AwesomeOptions.BazOptions bazOptions;


        public BazController(IOptions<AwesomeOptions.BazOptions> awesomeOptions)
        {
            this.bazOptions = awesomeOptions.Value;
        }

        [HttpGet]
        public IActionResult GetConfigs()
        {
            return Ok(bazOptions);
        }
    }
}