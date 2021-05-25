using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewLibrary.Model.Effects;

namespace ViewLibrary.ViewModel.TabPages.Effects
{
    public abstract class TempoBaseEffectViewModel : EffectBaseViewModel
    {
        private double _MinTempo;
        public double MinTempo
        {
            get
            {
                return _MinTempo;
            }
            set
            {
                if (Equals(_MinTempo, value))
                {
                    return;
                }

                _MinTempo = value;

                NotifyPropertyChanged();
            }
        }

        private double _MaxTempo;
        public double MaxTempo
        {
            get
            {
                return _MaxTempo;
            }
            set
            {
                if (Equals(_MaxTempo, value))
                {
                    return;
                }

                _MaxTempo = value;

                NotifyPropertyChanged();
            }
        }

        public double Tempo
        {
            get
            {
                return TempoModel.Tempo;
            }
            set
            {
                if (Equals(TempoModel.Tempo, value))
                {
                    return;
                }

                TempoModel.Tempo = value;

                NotifyPropertyChanged();
            }
        }

        private TempoBaseEffect _TempoModel;
        public TempoBaseEffect TempoModel
        {
            get
            {
                return _TempoModel;
            }
            set
            {
                if (Equals(_TempoModel, value))
                {
                    return;
                }

                _TempoModel = value;

                NotifyPropertyChanged();
            }
        }

        protected TempoBaseEffectViewModel(TempoBaseEffect model) : base(model)
        {
            TempoModel = model;
        }
    }
}
