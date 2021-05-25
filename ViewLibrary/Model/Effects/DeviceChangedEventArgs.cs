using System;

namespace ViewLibrary.Model.Effects
{
    public class DeviceChangedEventArgs : EventArgs
    {
        public bool HasLightFX
        {
            get;
            set;
        }

        public bool HasChroma
        {
            get;
            set;
        }

        public bool HasHue
        {
            get;
            set;
        }

        public Guid ChromaGuid
        {
            get;
            set;
        }

        //public ChromaFX.Devices.DeviceInfo? ChromaSpeaker
        //{
        //    get;
        //    set;
        //}
    }

    public delegate void DeviceChangedHandler(object sender, DeviceChangedEventArgs args);
}
