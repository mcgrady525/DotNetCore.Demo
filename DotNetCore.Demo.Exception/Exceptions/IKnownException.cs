using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore.Demo.Exception.Exceptions
{
    public interface IKnownException
    {
        /// <summary>
        /// 异常消息
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// 异常code
        /// </summary>
        public int ErrorCode { get; }

        /// <summary>
        /// 异常数据
        /// </summary>
        public object[] ErrorData { get; }
    }
}
