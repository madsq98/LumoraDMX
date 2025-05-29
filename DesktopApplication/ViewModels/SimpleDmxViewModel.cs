using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using FrontendServices.Services.SimpleDmx;

namespace DesktopApplication.ViewModels
{
    public partial class SimpleDmxViewModel : ObservableObject
    {
        private readonly SimpleDmxService _simpleDmxService;

        public ObservableCollection<PageViewModel> Pages { get; } = new();

        public SimpleDmxViewModel(SimpleDmxService simpleDmxService)
        {
            _simpleDmxService = simpleDmxService;

            for (int page = 0; page < 4; page++)
            {
                var sliders = new ObservableCollection<ChannelSliderViewModel>();
                for (int i = 0; i < 128; i++)
                {
                    int channel = page * 128 + i + 1;
                    sliders.Add(new ChannelSliderViewModel(channel, _simpleDmxService));
                }

                Pages.Add(new PageViewModel
                {
                    Header = $"Channels {page * 128 + 1}–{(page + 1) * 128}",
                    Channels = sliders
                });
            }

            _ = _simpleDmxService.SetDmxChannel(1, 0);
        }
    }

    public class PageViewModel
    {
        public string Header { get; set; } = "";
        public ObservableCollection<ChannelSliderViewModel> Channels { get; set; } = new();
    }
}
