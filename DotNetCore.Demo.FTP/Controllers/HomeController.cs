using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DotNetCore.Demo.FTP.Models;
using FluentFTP;
using System.Net;
using System.Text;
using System.IO;

namespace DotNetCore.Demo.FTP.Controllers
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

        public int TestFTP1()
        {
            try
            {
                // create an FTP client
                FtpClient client = new FtpClient("119.23.66.207");

                // specify the login credentials, unless you want to use the "anonymous" user account
                client.Credentials = new NetworkCredential("uftpServerInfo", "fUser!2020(629)");

                // begin connecting to the server
                client.Connect();

                // get a list of files and directories in the "/htdocs" folder
                foreach (FtpListItem item in client.GetListing("/ServerInfo"))
                {
                    //1，下载
                    //2，读取内容并解析
                    //3，持久化

                    byte[] bytes = null;
                    var serverInfoStr = "";

                    // if this is a file
                    if (item.Type == FtpFileSystemObjectType.File)
                    {
                        client.Download(out bytes, item.FullName);

                        //bytes to string
                        if (bytes != null)
                        {
                            serverInfoStr = Encoding.UTF8.GetString(bytes);
                        }
                    }

                    //解析文本
                    if (!string.IsNullOrEmpty(serverInfoStr))
                    {

                    }

                }

                // disconnect! good bye!
                client.Disconnect();
            }
            catch (Exception ex)
            {

            }

            return 0;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /// <summary>
        /// 解析文本
        /// </summary>
        /// <returns></returns>
        public int TestFTP2()
        {
            var path = "F:\\192.168.6.61.txt";
            var result = System.IO.File.ReadAllText(path, Encoding.UTF8);
            var rows = result.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            var v = rows[1];

            return 0;
        }
    }
}
