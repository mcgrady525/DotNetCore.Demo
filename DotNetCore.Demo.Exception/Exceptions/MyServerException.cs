using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore.Demo.Exception.Exceptions
{
    public class MyServerException : System.Exception, IKnownException
    {
        public MyServerException(string message, int errorCode, params object[] errorData) : base(message)
        {
            this.ErrorCode = errorCode;
            this.ErrorData = errorData;
        }

        public int ErrorCode { get; private set; }
        public object[] ErrorData { get; private set; }
    }
}
