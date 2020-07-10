using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DotNetCore.Demo.Cookie.Models;
using Microsoft.AspNetCore.Http;

namespace DotNetCore.Demo.Cookie.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly string CookieKey = "UserName";
        private readonly string CookieValue = "zhangsan";


        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Add()
        {
            HttpContext.Response.Cookies.Append(CookieKey, CookieValue, new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddMinutes(1)
            });

            return Content("cookie写入成功");
        }

        public IActionResult Get()
        {
            string result;
            var flag = HttpContext.Request.Cookies.TryGetValue(CookieKey, out result);

            return Content(string.Format("欢迎你：{0}", result));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
