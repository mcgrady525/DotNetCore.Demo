using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore.Demo.DI.Services
{
    public interface IOrderService { }


    public class OrderService : IOrderService
    {
    }

    public class OrderServiceV2 : IOrderService
    {
    }
}
