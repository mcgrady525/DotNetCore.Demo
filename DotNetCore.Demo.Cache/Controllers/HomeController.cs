using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DotNetCore.Demo.Cache.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.Distributed;

namespace DotNetCore.Demo.Cache.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /// <summary>
        /// 测试进程内缓存
        /// </summary>
        /// <param name="memoryCache"></param>
        /// <returns></returns>
        public IActionResult TestMemoryCache([FromServices]IMemoryCache memoryCache)
        {
            var cacheKey = "key1";
            var cacheValue = memoryCache.Get(cacheKey);
            if (cacheValue == null)
            {
                cacheValue = DateTime.Now;
                memoryCache.Set(cacheKey, cacheValue, DateTimeOffset.Now.AddMinutes(10));
            }

            return Content(string.Format("key:{0}, value:{1}", cacheKey, cacheValue.ToString()));
        }

        /// <summary>
        /// 测试分布式缓存（基于redis）
        /// </summary>
        /// <param name="cache"></param>
        /// <returns></returns>
        public IActionResult TestRedisCache([FromServices]IDistributedCache cache)
        {
            var cacheKey = "key2";
            var cacheValue = cache.GetString(cacheKey);
            if (cacheValue == null)
            {
                cacheValue = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                cache.SetString(cacheKey, cacheValue, new DistributedCacheEntryOptions {AbsoluteExpiration= DateTimeOffset.Now.AddMinutes(10) });
            }

            return Content(string.Format("key:{0}, value:{1}", cacheKey, cacheValue.ToString()));
        }

    }
}
