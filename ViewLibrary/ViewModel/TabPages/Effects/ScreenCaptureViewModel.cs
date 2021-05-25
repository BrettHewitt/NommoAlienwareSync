using ScreenInformation;
using System.Collections.ObjectModel;
using System.Windows;
using ViewLibrary.Model.Effects;
using ViewLibrary.ViewModel.Monitors;

namespace ViewLibrary.ViewModel.TabPages.Effects
{
    public class ScreenCaptureViewModel : EffectBaseViewModel
    {
        private ObservableCollection<DisplayViewModel> _Monitors;
        public ObservableCollection<DisplayViewModel> Monitors
        {
            get
            {
                return _Monitors;
            }
            set
            {
                if (Equals(_Monitors, value))
                {
                    return;
                }

                _Monitors = value;

                NotifyPropertyChanged();
            }
        }
        
        public Rect MonitorRect
        {
            get
            {
                return ScreenCaptureModel.MonitorRect;
            }
            set
            {
                if (Equals(ScreenCaptureModel.MonitorRect, value))
                {
                    return;
                }

                ScreenCaptureModel.MonitorRect = value;
                
                NotifyPropertyChanged();
            }
        }
        
        private bool _IsError;
        public bool IsError
        {
            get
            {
                return _IsError;
            }
            set
            {
                if (Equals(_IsError, value))
                {
                    return;
                }

                _IsError = value;

                NotifyPropertyChanged();
            }
        }

        private ScreenCapture _ScreenCaptureModel;
        public ScreenCapture ScreenCaptureModel
        {
            get
            {
                return _ScreenCaptureModel;
            }
            set
            {
                if (Equals(_ScreenCaptureModel, value))
                {
                    return;
                }

                _ScreenCaptureModel = value;

                NotifyPropertyChanged();
            }
        }

        public ScreenCaptureViewModel(ScreenCapture model) : base(model)
        {
            DisplaySource[] detailedMonitors = ScreenInformation.ScreenManager.GetDetailedMonitors();
            ObservableCollection<DisplayViewModel> monitors = new ObservableCollection<DisplayViewModel>();
            foreach (var monitor in detailedMonitors)
            {
                monitors.Add(new DisplayViewModel(monitor));
            }

            Monitors = monitors;
            ScreenCaptureModel = model;
        }
        
        protected override void ApplyEffect()
        {
            if (IsError)
            {
                MessageBox.Show("The screen capture dimensions extend beyound the bounds of the monitor. Please enter correct dimensions", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Model.StartEffect();
        }
    }
}
