using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DotNetCore.Demo.Http.Clients
{
    public class TypedClient
    {
        HttpClient _client;

        public TypedClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<string> Get()
        {
            return await _client.GetStringAsync("/api/WeatherForecast/Get2");
        }

    }
}
