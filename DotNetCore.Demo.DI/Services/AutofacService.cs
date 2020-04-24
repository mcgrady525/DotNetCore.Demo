using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore.Demo.DI.Services
{
    public interface IAutofacService
    {
        void ShowCode();
    }


    public class AutofacService : IAutofacService
    {
        public void ShowCode()
        {
            Console.WriteLine(string.Format("AutofacService.ShowCode:{0}", this.GetHashCode()));
        }
    }

    public class AutofacServiceV2 : IAutofacService
    {
        public MyNameService NameService { get; set; }

        public void ShowCode()
        {
            Console.WriteLine(string.Format("AutofacServiceV2.ShowCode:{0},NameService是否为空:{1}", this.GetHashCode(), NameService == null));
        }
    }

    public class MyNameService
    { }
}
