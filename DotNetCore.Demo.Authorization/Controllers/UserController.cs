using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DotNetCore.Demo.Authorization.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace DotNetCore.Demo.Authorization.Controllers
{
    public class UserController : Controller
    {
        private static List<User> _users = new List<User>()
        {
            new User {  Id=1, Name="admin", Password="admin", Email="admin@gmail.com", PhoneNumber="18800000001", Roles= new List<string>{"admin" } },
            new User {  Id=2, Name="zhangsan", Password="zhangsan", Email="zhangsan@gmail.com", PhoneNumber="18800000002", Roles= new List<string>{ "ops" }  },
            new User {  Id=3, Name="lisi", Password="lisi", Email="lisi@gmail.com", PhoneNumber="18800000003", Roles= new List<string>{ "developer" }  },
            new User {  Id=4, Name="wangwu", Password="wangwu", Email="wangwu@gmail.com", PhoneNumber="18800000004", Roles= new List<string>{ "ops", "developer" }  }
        };


        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login(string userName, string password)
        {
            //校验用户名和密码
            var user = _users.FirstOrDefault(p => p.Name == userName && p.Password == password);
            if (user == null)
            {
                return Content("用户名或密码错误!");
            }

            //将身份信息写入cookie
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Sid, user.Id.ToString()));
            identity.AddClaim(new Claim(ClaimTypes.Name, user.Name));
            foreach (var role in user.Roles)//多个角色
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, role)); 
            }
            var userPrincipal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                userPrincipal,
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    AllowRefresh = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(20)
                });

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login", "User");
        }
    }
}