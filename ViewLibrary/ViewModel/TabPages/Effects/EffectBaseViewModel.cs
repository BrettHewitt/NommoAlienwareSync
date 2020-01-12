using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewLibrary.Commands;
using ViewLibrary.Model.Effects;
using ViewLibrary.Model.Settings;

namespace ViewLibrary.ViewModel.TabPages.Effects
{
    public abstract class EffectBaseViewModel : ViewModelBase
    {
        private EffectBase _Model;
        public EffectBase Model
        {
            get
            {
                return _Model;
            }
            set
            {
                if (Equals(_Model, value))
                {
                    return;
                }

                _Model = value;

                NotifyPropertyChanged();
            }
        }

        public string EffectName
        {
            get
            {
                return Model.EffectName;
            }
        }

        public string Description
        {
            get
            {
                return Model.Description;
            }
        }

        private GlobalSettings _Settings;
        protected GlobalSettings Settings
        {
            get
            {
                return _Settings;
            }
            set
            {
                if (Equals(_Settings, value))
                {
                    return;
                }

                _Settings = value;

                NotifyPropertyChanged();
            }
        }
        
        protected EffectBaseViewModel(EffectBase model)
        {
            Model = model;
        }

        public void StartEffect()
        {
            ApplyEffect();
        }

        public void StopEffect()
        {
            Model.StopEffect();
        }

        protected abstract void ApplyEffect();
    }
}
