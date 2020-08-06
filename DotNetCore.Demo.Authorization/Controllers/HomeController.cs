using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DotNetCore.Demo.Authorization.Models;
using Microsoft.AspNetCore.Authorization;

namespace DotNetCore.Demo.Authorization.Controllers
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
            //获取当前用户的身份信息
            var profileInfo = new ProfileInfo();
            profileInfo.Name = HttpContext.User.Identity.Name;
            profileInfo.AuthenticationType = HttpContext.User.Identity.AuthenticationType;
            profileInfo.Claims = HttpContext.User.Claims.ToList();

            return View(profileInfo);
        }

        [Authorize(Roles ="admin")]
        public IActionResult OnlyAdmin()
        {
            return View();
        }

        [Authorize(Roles = "admin, ops")]
        public IActionResult OnlyOps()
        {
            return View();
        }

        [Authorize(Roles = "admin, developer")]
        public IActionResult OnlyDeveloper()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return Content("对不起，你无权访问!");
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
}
