using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewLibrary.Model.Devices;

namespace ViewLibrary.ViewModel.Devices
{
    public abstract class DeviceBaseViewModel : ViewModelBase
    {
        private string _DeviceName;
        public string DeviceName
        {
            get
            {
                return _DeviceName;
            }
            set
            {
                if (Equals(_DeviceName, value))
                {
                    return;
                }

                _DeviceName = value;

                NotifyPropertyChanged();
            }
        }

        private DeviceType _DeviceType;
        public DeviceType DeviceType
        {
            get
            {
                return _DeviceType;
            }
            protected set
            {
                if (Equals(_DeviceType, value))
                {
                    return;
                }

                _DeviceType = value;

                NotifyPropertyChanged();
            }
        }

    }
}
