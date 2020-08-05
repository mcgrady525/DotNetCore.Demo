using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DotNetCore.Demo.Authentication.Models;
using Microsoft.AspNetCore.Authorization;

namespace DotNetCore.Demo.Authentication.Controllers
{
    [Authorize]
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

        /// <summary>
        /// 个人信息
        /// </summary>
        /// <returns></returns>
        public IActionResult Profile()
        {
            var profile = new Profile() {Claims= new Dictionary<string, string>() };
            profile.Name = HttpContext.User.Identity.Name;
            profile.AuthenticationType = HttpContext.User.Identity.AuthenticationType;

            var claims= HttpContext.User.Claims.ToList();
            foreach (var item in claims)
            {
                profile.Claims.Add(item.Type, item.Value);
            }

            return View(profile);
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
    }

    public class Profile
    { 
        public string Name { get; set; }

        public string AuthenticationType { get; set; }

        public Dictionary<string, string> Claims { get; set; }
    }
}
