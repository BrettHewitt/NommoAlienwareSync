using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using huestream;
using Action = System.Action;
using Color = System.Drawing.Color;

namespace ViewLibrary.Model.Hue
{
    public class HueFX : IFeedbackMessageHandler
    {
        private static HueFX _Instance;
        public static HueFX Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new HueFX();
                }

                return _Instance;
            }
        }

        private Bridge Client
        {
            get;
            set;
        }

        private AreaEffect AreaEffect
        {
            get;
            set;
        }

        private IHueStream HueStream
        {
            get;
            set;
        }

        public event EventHandler PushLinkRequested;
        public event EventHandler PushLinkReceived;
        public event EventHandler PushLinkFailed;
        public event EventHandler BridgeConnected;
        public event EventHandler UserProcedureFinished;

        ~HueFX()
        {
            HueStream?.ShutDown();
        }

        public void InitHueEdk(string encryptionKey)
        {
            if (Client != null)
            {
                return;
            }

            Client = new Bridge(new BridgeSettings());
            Config config = new Config("dataDyneSync", "PC", new PersistenceEncryptionKey(encryptionKey));
            config.SetStreamingMode(StreamingMode.STREAMING_MODE_DTLS);
            HueStream = new HueStream(config);
            
            HueStream.RegisterFeedbackHandler(this);

            IArea area = new Area(-1, 1, 1, -1, "All");
            AreaEffect = new AreaEffect();
            AreaEffect.AddArea(area);
            HueStream.AddEffect(AreaEffect);
            AreaEffect.Enable();
        }

        public override void NewFeedbackMessage(FeedbackMessage message)
        {
            //Handle messages
            var bridge = message.GetBridge();
            if (bridge != null)
            {
                Client = bridge;
            }

            var id = message.GetId();
            
            if (id == FeedbackMessage.Id.ID_PRESS_PUSH_LINK)
            {
                PushLinkRequested?.Invoke(this, EventArgs.Empty);
                return;
            }

            if (id == FeedbackMessage.Id.ID_FINISH_AUTHORIZING_FAILED)
            {
                PushLinkFailed?.Invoke(this, EventArgs.Empty);
                return;
            }

            if (id == FeedbackMessage.Id.ID_FINISH_AUTHORIZING_AUTHORIZED)
            {
                PushLinkReceived?.Invoke(this, EventArgs.Empty);
                return;
            }

            if (id == FeedbackMessage.Id.ID_USERPROCEDURE_FINISHED)
            {
                if (HueStream.IsBridgeStreaming())
                {
                    BridgeConnected?.Invoke(this, EventArgs.Empty);
                    return;
                }

                UserProcedureFinished?.Invoke(this, EventArgs.Empty);
                return;
            }
        }

        public void Register(string ip)
        {
            HueStream.ConnectBridgeManualIpAsync(ip);
        }

        public void Connect()
        {
            HueStream.ConnectBridgeAsync();
        }

        public void Start()
        {
            HueStream.Start();
        }

        public void ShutDown()
        {
            Thread.Sleep(500);
            FadeOut();
            Thread.Sleep(500);
            HueStream.Stop();
            HueStream?.ShutDown();
            HueStream = null;
        }

        public void SetColour(Color color)
        {
            var hueColor = new huestream.Color();
            hueColor.SetR(color.R / 255d);
            hueColor.SetG(color.G / 255d);
            hueColor.SetB(color.B / 255d);

            AreaEffect.SetColorAnimation(new ConstantAnimation(hueColor.R), new ConstantAnimation(hueColor.G), new ConstantAnimation(hueColor.B));
        }

        public void SetColour(double r, double g, double b)
        {
            var hueColor = new huestream.Color();
            hueColor.SetR(r);
            hueColor.SetG(g);
            hueColor.SetB(b);

            AreaEffect.SetColorAnimation(new ConstantAnimation(hueColor.R), new ConstantAnimation(hueColor.G), new ConstantAnimation(hueColor.B));
        }

        public void FadeOut()
        {
            var hueColor = new huestream.Color();
            hueColor.SetR(0);
            hueColor.SetG(0);
            hueColor.SetB(0);

            AreaEffect.SetColorAnimation(new TweenAnimation(0, 500, TweenType.Linear),
                new TweenAnimation(0, 500, TweenType.Linear), new TweenAnimation(0, 500, TweenType.Linear));
        }
    }
}
