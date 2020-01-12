using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ViewLibrary.ViewModel
{
    public abstract class WindowViewModelBase : ViewModelBase
    {
        private bool _Close;
        private ImageSource _Icon;
        private WindowExitResult _ExitResult = WindowExitResult.Notset;

        public bool Close
        {
            get
            {
                return _Close;
            }
            set
            {
                if (Equals(_Close, value))
                {
                    return;
                }

                _Close = value;

                NotifyPropertyChanged();
            }
        }

        public ImageSource Icon
        {
            get
            {
                return _Icon;
            }
            set
            {
                if (Equals(_Icon, value))
                {
                    return;
                }

                _Icon = value;

                NotifyPropertyChanged();
            }
        }

        public WindowExitResult ExitResult
        {
            get
            {
                return _ExitResult;
            }
            protected set
            {
                _ExitResult = value;
            }
        }

        protected void CloseWindow()
        {
            Close = true;
        }
    }

    public enum WindowExitResult
    {
        Notset,
        Ok,
        Cancel,
    }
}
