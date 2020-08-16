using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyCaching.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Tutorials.NetCore.Redis.Introduction.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RedisController : ControllerBase
    {
        private readonly IEasyCachingProvider _cachingProvider;

        public RedisController(IEasyCachingProviderFactory cachingProviderFactory)
        {
            _cachingProvider = cachingProviderFactory.GetCachingProvider("RedisLocal"); //Same name that we set in Startup.cs
        }


        [HttpGet()]
        public IActionResult Index()
        {
            return new JsonResult("You can try with /Set and /Get method.");
        }

        [HttpGet("Set")]
        public IActionResult SetItemInQueue(string key, string value)
        {
            if (String.IsNullOrEmpty(key) || String.IsNullOrEmpty(value))
            {
                return new JsonResult("You should call this method with 'key' and 'value'");
            }
            _cachingProvider.Set(key, value, TimeSpan.FromMinutes(3));
            return Ok();
        }

        [HttpGet("Get")]
        public IActionResult GetItemInQueue(string key)
        {
            if (String.IsNullOrEmpty(key))
            {
                return new JsonResult("You should call this method with 'key'");
            }
            var item = _cachingProvider.Get<string>(key);
            return new JsonResult(item.Value);
        }


        // Other necessary methods.

        // _cachingProvider.Exists(key) 
        // _cachingProvider.Remove(key);  

    }
}
