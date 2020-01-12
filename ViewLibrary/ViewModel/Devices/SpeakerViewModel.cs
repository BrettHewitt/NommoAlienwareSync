using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewLibrary.ViewModel.Devices
{
    public class SpeakerViewModel : DeviceBaseViewModel
    {
        private ChromaFX.Devices.DeviceInfo _Speaker;
        public ChromaFX.Devices.DeviceInfo Speaker
        {
            get
            {
                return _Speaker;
            }
            set
            {
                if (Equals(_Speaker, value))
                {
                    return;
                }

                _Speaker = value;

                NotifyPropertyChanged();
            }
        }

        public SpeakerViewModel(ChromaFX.Devices.DeviceInfo speaker)
        {
            DeviceType = Model.Devices.DeviceType.Speaker;
            Speaker = speaker;
        }
    }
}
