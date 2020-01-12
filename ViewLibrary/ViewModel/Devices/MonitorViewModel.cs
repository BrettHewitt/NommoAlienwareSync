using AlienFXWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewLibrary.ViewModel.Devices
{
    public class MonitorViewModel : DeviceBaseViewModel
    {
        private MonitorDetails _Monitor;
        public MonitorDetails Monitor
        {
            get
            {
                return _Monitor;
            }
            set
            {
                if (Equals(_Monitor, value))
                {
                    return;
                }

                _Monitor = value;

                NotifyPropertyChanged();
            }
        }


        public MonitorViewModel(MonitorDetails monitor)
        {
            DeviceType = Model.Devices.DeviceType.Monitor;
            Monitor = monitor;
            DeviceName = Monitor.DeviceDescription;
        }
    }
}
