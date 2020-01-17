using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewLibrary.ViewModel.Devices
{
    public class SpeakerViewModel : DeviceBaseViewModel
    {
        public SpeakerViewModel()
        {
            DeviceType = Model.Devices.DeviceType.Speaker;
        }
    }
}
