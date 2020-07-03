using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Demo.Logging.Serilog.ConsoleApp
{
    public interface IRunner
    {
        void DoAction(string name);
    }

    public class Runner: IRunner
    {
        private readonly ILogger<Runner> _logger;

        public Runner(ILogger<Runner> logger)
        {
            _logger = logger;
        }

        public void DoAction(string name)
        {
            _logger.LogTrace(string.Format("这是一条trace日志，{0}", name));
            _logger.LogDebug(string.Format("这是一条debug日志，{0}", name));
        }
    }
}
