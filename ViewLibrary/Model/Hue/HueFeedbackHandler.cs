using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using huestream;

namespace ViewLibrary.Model.Hue
{
    public class HueFeedbackHandler : IFeedbackMessageHandler
    {
        public event EventHandler<FeedbackMessage> MessageReceived;

        public override void NewFeedbackMessage(FeedbackMessage message)
        {
            MessageReceived?.Invoke(this, message);
        }
    }
}
