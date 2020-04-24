using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore.Demo.DI.Services
{
    public interface IGenericService<T> { }


    public class GenericService<T> : IGenericService<T>
    {
        public T Data { get; set; }

        public GenericService(T data)
        {
            this.Data = data;
        }

    }
}
