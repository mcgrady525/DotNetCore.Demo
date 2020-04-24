using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore.Demo.DI.Services
{
    public class MyInterceptor: Castle.DynamicProxy.IInterceptor
    {
        public void Intercept(Castle.DynamicProxy.IInvocation invocation)
        {
            Console.WriteLine(string.Format("Intercept before,method:{0}", invocation.Method.Name));

            invocation.Proceed();//具体方法执行

            Console.WriteLine(string.Format("Intercept after,method:{0}", invocation.Method.Name));
        }
    }
}
