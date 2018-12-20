using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using OmniaTransferLogDev.Models;
using OmniaTransferLogDev.Security;

namespace OmniaTransferLogDev.Controllers
{
    public class LogController : ApiController
    {
        // GET: api/Log
        public IHttpActionResult Get()
        {
            Dictionary<string, string> values = new Dictionary<string, string>
            {
                { "Key1", "Value1" },
                {"Key2", "Value2" }
            };
            var json = JsonConvert.SerializeObject(values);
            return Ok(values);
        }

        // GET: api/Log/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Log
        public IHttpActionResult Post(LogItem item)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    return Ok(item);
                }
                catch(Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
           
        }

        // PUT: api/Log/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Log/5
        public void Delete(int id)
        {
        }
    }
}
