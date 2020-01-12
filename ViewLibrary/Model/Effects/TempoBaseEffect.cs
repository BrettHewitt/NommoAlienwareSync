using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewLibrary.Model.Effects
{
    public abstract class TempoBaseEffect : EffectBase
    {
        private readonly object _TempoLock = new object();
        private double _Tempo;
        public double Tempo
        {
            get
            {
                lock (_TempoLock)
                {
                    return _Tempo;
                }
            }
            set
            {
                lock (_TempoLock)
                {
                    _Tempo = value;
                }
            }
        }

        protected TempoBaseEffect(string effectName) : base(effectName)
        {

        }
    }
}
