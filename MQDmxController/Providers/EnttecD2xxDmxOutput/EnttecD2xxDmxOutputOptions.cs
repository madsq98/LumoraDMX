using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MQDmxController.Abstractions;

namespace MQDmxController.Providers.EnttecD2xxDmxOutput
{
    public class EnttecD2xxDmxOutputOptions : DmxOutputOptions
    {
        public string SerialNumber { get; set; }
    }
}
