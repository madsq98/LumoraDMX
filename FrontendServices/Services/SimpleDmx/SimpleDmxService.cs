using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontendServices.Services.SimpleDmx
{
    public class SimpleDmxService : ServiceBase
    {
        public async Task<bool> SetDmxChannel(int channel, int value)
        {
            var data = new SetDmxType
            {
                Channel = channel,
                Value = value
            };

            var httpRequest = CreatePostRequest("SimpleDmx/setChannel", data);
            var response = await SendAsync(httpRequest);

            Console.WriteLine($"Sent {channel}:{value} - result: {response.StatusCode}");

            return response.IsSuccessStatusCode;
        }
    }

    public class SetDmxType
    {
        public int Channel { get; set; }
        public int Value { get; set; }
    }
}
