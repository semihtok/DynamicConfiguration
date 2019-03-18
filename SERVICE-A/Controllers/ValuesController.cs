using DynamicConfiguration.Core;
using Microsoft.AspNetCore.Mvc;

namespace SERVICE_A.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
          // GET api/values
        [HttpGet]
        public ActionResult<string> Get()
        {
            var configurationManager = new ConfigurationReader("SERVICE-A", "redis:6379", 15000);
            var configValue = configurationManager.GetValue<string>("SiteName");

            if (string.IsNullOrEmpty(configValue))
            {
                return NotFound();
            }
            return Ok(configValue);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}