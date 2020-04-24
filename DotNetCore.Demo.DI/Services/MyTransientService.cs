using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore.Demo.DI.Services
{
    public interface IMyTransientService { }


    public class MyTransientService: IMyTransientService
    {
    }
}
