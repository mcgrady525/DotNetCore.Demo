using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DotNetCore.Demo.Authorization.Models
{
    public class ProfileInfo
    {
        public string Name { get; set; }

        public string AuthenticationType { get; set; }

        public List<Claim> Claims { get; set; }
    }
}
