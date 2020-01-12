using ViewLibrary.Model.Effects;

namespace ViewLibrary.ViewModel.TabPages.Effects
{
    public class SpectrumViewModel : TempoBaseEffectViewModel
    {
        private Spectrum _SpectrumModel;
        public Spectrum SpectrumModel
        {
            get
            {
                return _SpectrumModel;
            }
            set
            {
                if (Equals(_SpectrumModel, value))
                {
                    return;
                }

                _SpectrumModel = value;

                NotifyPropertyChanged();
            }
        }

        public SpectrumViewModel(Spectrum model) : base(model)
        {
            SpectrumModel = model;
            MinTempo = 0.5;
            MaxTempo = 2.5;
        }
        
        protected override void ApplyEffect()
        {
            Model.StartEffect();
        }
    }
}
