using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DynamicConfiguration.Data.Service;
using DynamicConfiguration.Data.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DynamicConfiguration.UI.Controllers
{
   
    [Route("api/[controller]")]
    public class DynamicConfigurationController : Controller
    {
     

        [HttpGet]
        public IEnumerable<DynamicConfig> Get()
        {
            var redisService = new RedisConfigurationService("localhost:6379");
            var results = redisService.GetAllConfig();
            return results;
        }
        
        [HttpGet]
        [Route("GetOne")]
        public DynamicConfig GetOne(string key)
        {
            var redisService = new RedisConfigurationService("localhost:6379");
            var result = redisService.GetOne(key);
            return result;
        }

        
        [HttpPost]
        public void Post(DynamicConfig config)
        {
            var redisService = new RedisConfigurationService("localhost:6379");
            redisService.AddConfig(config);
        }
       
        [HttpPost]
        [Route("Update")]
        public void Update(DynamicConfig config)
        {
            var redisService = new RedisConfigurationService("localhost:6379");
            redisService.UpdateConfig(config);
        }
    }
}