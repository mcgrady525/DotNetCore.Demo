using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore.Demo.DI.Services
{
    public interface IOrderService { }


    public class OrderService : IOrderService, IDisposable
    {
        public void Dispose()
        {
            Console.WriteLine(string.Format("OrderService disposabled,hashcode:{0}", this.GetHashCode()));
        }

    }

    public class OrderServiceV2 : IOrderService, IDisposable
    {
        public void Dispose()
        {
            Console.WriteLine(string.Format("OrderServiceV2 disposabled,hashcode:{0}", this.GetHashCode()));
        }
    }
}
