using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontendServices.Services
{
    public abstract class ServiceBase
    {
        private HttpClient? _client;
        protected string? _backend;

        public void SetBackend(string backend)
        {
            _backend = backend;
            _client = new HttpClient()
            {
                BaseAddress = new Uri($"{backend}/api/")
            };
        }

        protected bool IsReady()
        {
            return _backend != null && _client != null;
        }

        protected async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            if(!IsReady())
            {
                throw new Exception("Backend not initialized!");
            }

            return await _client.SendAsync(request);
        }
    }
}
