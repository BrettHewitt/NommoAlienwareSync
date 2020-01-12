using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ViewLibrary.ViewModel.Devices;

namespace ViewLibrary.TemplateSelectors
{
    public class DeviceToImageSelector : DataTemplateSelector
    {
        public DataTemplate NommoTemplate 
        { 
            get; 
            set; 
        }

        public DataTemplate NommoProTemplate 
        { 
            get; 
            set; 
        }

        public DataTemplate AW3420DWTemplate 
        { 
            get; 
            set; 
        }

        public DataTemplate AW3418DWTemplate
        {
            get;
            set;
        }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            SpeakerViewModel speaker = item as SpeakerViewModel;
            if (speaker != null)
            {
                if (speaker.DeviceName.Contains("Pro"))
                {
                    return NommoProTemplate;
                }
                else
                {
                    return NommoTemplate;
                }
            }

            MonitorViewModel monitor = item as MonitorViewModel;
            if (monitor != null)
            {
                if (monitor.DeviceName == "AW3420DW")
                {
                    return AW3420DWTemplate;
                }
                else
                {
                    return AW3418DWTemplate;
                }
            }

            return null; ;
        }
    }
}
