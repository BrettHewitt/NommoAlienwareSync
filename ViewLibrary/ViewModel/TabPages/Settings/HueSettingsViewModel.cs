using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using ViewLibrary.Commands;
using ViewLibrary.Helpers;
using ViewLibrary.Model.Hue;
using ViewLibrary.Model.Settings;

namespace ViewLibrary.ViewModel.TabPages.Settings
{
    public class HueSettingsViewModel : SettingsBaseViewModel
    {
        private ActionCommand _GenerateEncryptionKeyCommand;
        public ActionCommand GenerateEncryptionKeyCommand => 
            _GenerateEncryptionKeyCommand ?? (_GenerateEncryptionKeyCommand = new ActionCommand()
            {
                ExecuteAction = GenerateEncryptionKey
            });

        private ActionCommand _ConnectToBridgeCommand;
        public ActionCommand ConnectToBridgeCommand =>
            _ConnectToBridgeCommand ?? (_ConnectToBridgeCommand = new ActionCommand()
            {
                ExecuteAction = ConnectToBridge,
            });

        private string _EncryptionKey;
        public string EncryptionKey
        {
            get
            {
                return _EncryptionKey;
            }
            set
            {
                if (Equals(_EncryptionKey, value))
                {
                    return;
                }

                _EncryptionKey = value;

                NotifyPropertyChanged();
            }
        }

        private bool _EnableHue;
        public bool EnableHue
        {
            get
            {
                return _EnableHue;
            }
            set
            {
                if (Equals(_EnableHue, value))
                {
                    return;
                }

                _EnableHue = value;

                NotifyPropertyChanged();
            }
        }

        private bool _WaitingForLinkPush;
        public bool WaitingForLinkPush
        {
            get
            {
                return _WaitingForLinkPush;
            }
            set
            {
                if (Equals(_WaitingForLinkPush, value))
                {
                    return;
                }

                _WaitingForLinkPush = value;

                NotifyPropertyChanged();
            }
        }
        
        private string _LinkStatus;
        public string LinkStatus
        {
            get
            {
                return _LinkStatus;
            }
            set
            {
                if (Equals(_LinkStatus, value))
                {
                    return;
                }

                _LinkStatus = value;

                NotifyPropertyChanged();
            }
        }


        private Dispatcher Dispatcher
        {
            get;
        }

        public HueSettingsViewModel(Dispatcher dispatcher) : base("Hue Settings")
        {
            Dispatcher = dispatcher;
        }

        protected override string SetDescription()
        {
            return "Phillips Hue Settings";
        }

        protected override void LoadSettings()
        {
            GlobalSettings settings = SettingsManager.GetSettings();
            EncryptionKey = settings.HueSettings.Key;
            EnableHue = settings.HueSettings.EnableHue;
        }

        public override void SaveSettings()
        {
            GlobalSettings settings = SettingsManager.GetSettings();
            settings.HueSettings.Key = EncryptionKey;
            settings.HueSettings.EnableHue = EnableHue;
            SettingsManager.SaveSettings(settings);
        }

        private void GenerateEncryptionKey()
        {
            EncryptionKey = StringHelper.GenerateRandomString(32);
        }

        private void ConnectToBridge()
        {
            if (string.IsNullOrWhiteSpace(EncryptionKey))
            {
                MessageBox.Show("Please enter or generate an Encryption Key");
                return;
            }

            SaveSettings();

            WaitingForLinkPush = true;
            LinkStatus = "Connecting...";
            HueFX.Instance.InitHueEdk(EncryptionKey);
            HueFX.Instance.PushLinkRequested += Instance_PushLinkRequested;
            HueFX.Instance.PushLinkReceived += Instance_PushLinkReceived;
            HueFX.Instance.PushLinkFailed += Instance_PushLinkFailed;
            HueFX.Instance.UserProcedureFinished += Instance_UserProcedureFinished;
            HueFX.Instance.BridgeConnected += Instance_BridgeConnected;
            HueFX.Instance.Connect();
        }

        private void Instance_PushLinkFailed(object sender, EventArgs e)
        {
            HueFX.Instance.PushLinkReceived -= Instance_PushLinkReceived;
            HueFX.Instance.PushLinkFailed -= Instance_PushLinkFailed;

            Dispatcher.Invoke(() => { LinkStatus = "Authorization Failed"; });
        }

        private void Instance_PushLinkReceived(object sender, EventArgs e)
        {
            HueFX.Instance.PushLinkReceived -= Instance_PushLinkReceived;
            HueFX.Instance.PushLinkFailed -= Instance_PushLinkFailed;

            Dispatcher.Invoke(() => { LinkStatus = "Authorized..."; });
        }

        private void Instance_BridgeConnected(object sender, EventArgs e)
        {
            HueFX.Instance.UserProcedureFinished -= Instance_UserProcedureFinished;
            HueFX.Instance.BridgeConnected -= Instance_BridgeConnected;

            Dispatcher.Invoke(() =>
            {
                WaitingForLinkPush = false;
                MessageBox.Show("Connected To Philips Hue Bridge");
            });
        }

        private void Instance_UserProcedureFinished(object sender, EventArgs e)
        {
            HueFX.Instance.UserProcedureFinished -= Instance_UserProcedureFinished;
            HueFX.Instance.BridgeConnected -= Instance_BridgeConnected;

            Dispatcher.Invoke(() =>
            {
                WaitingForLinkPush = false;
                MessageBox.Show("Unable to connect to Philips Hue Bridge");
            });
        }

        private void Instance_PushLinkRequested(object sender, EventArgs e)
        {
            HueFX.Instance.PushLinkRequested -= Instance_PushLinkRequested;

            Dispatcher.Invoke(() => { LinkStatus = "Please Press Link button on your Phillips Hue Bridge"; });
        }
    }
}
