using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using FrontendServices.Services.SimpleDmx;

namespace DesktopApplication.ViewModels
{
    public partial class ChannelSliderViewModel : ObservableObject
    {
        [ObservableProperty]
        private int _channel;

        [ObservableProperty]
        private int _value;

        partial void OnValueChanged(int value)
        {
            _ = _simpleDmxService.SetDmxChannel(Channel, value);
        }

        private readonly SimpleDmxService _simpleDmxService;

        public ChannelSliderViewModel(int channel, SimpleDmxService service)
        {
            _channel = channel;
            _simpleDmxService = service;
        }
    }
}
