using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MQDmxController.Types;

namespace MQDmxController.Abstractions
{
    public interface IDmxOutput
    {
        void SendFrame(byte[] dmxData);
        void Initialize(DmxOutputOptions options);
        void Shutdown();
        List<ProviderDevice> GetDevices();
    }
}
