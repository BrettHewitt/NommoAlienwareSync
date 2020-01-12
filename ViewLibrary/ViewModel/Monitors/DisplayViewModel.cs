using ScreenInformation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewLibrary.ViewModel.Monitors
{
    public class DisplayViewModel : ViewModelBase
    {
        private string _Name;
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                if (Equals(_Name, value))
                {
                    return;
                }

                _Name = value;

                NotifyPropertyChanged();
            }
        }

        private Rectangle _WorkingArea;
        public Rectangle WorkingArea
        {
            get
            {
                return _WorkingArea;
            }
            set
            {
                if (Equals(_WorkingArea, value))
                {
                    return;
                }

                _WorkingArea = value;

                NotifyPropertyChanged();
            }
        }

        private DisplaySource _Model;
        public DisplaySource Model
        {
            get
            {
                return _Model;
            }
            set
            {
                if (Equals(_Model, value))
                {
                    return;
                }

                _Model = value;

                NotifyPropertyChanged();
            }
        }

        public DisplayViewModel(DisplaySource model)
        {
            Model = model;
            Name = model.MonitorInformation.FriendlyName;
            WorkingArea = model.MonitorInformation.WorkArea;
        }
    }
}
