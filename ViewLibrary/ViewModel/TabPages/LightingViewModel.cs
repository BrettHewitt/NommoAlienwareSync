using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewLibrary.Commands;
using ViewLibrary.Model.Effects;
using ViewLibrary.Model.Settings;
using ViewLibrary.ViewModel.TabPages.Effects;

namespace ViewLibrary.ViewModel.TabPages
{
    public class LightingViewModel : ViewModelBase
    {
        private ActionCommand _ApplyCommand;
        public ActionCommand ApplyCommand
        {
            get
            {
                return _ApplyCommand ?? (_ApplyCommand = new ActionCommand()
                {
                    ExecuteAction = StartEffect,
                    CanExecuteAction = () => SelectedEffect != null,
                });
            }
        }

        private ActionCommand _StopCommand;
        public ActionCommand StopCommand
        {
            get
            {
                return _StopCommand ?? (_StopCommand = new ActionCommand()
                {
                    ExecuteAction = () => StopEffects(true),
                });
            }
        }

        private ObservableCollection<EffectBaseViewModel> _Effects;
        public ObservableCollection<EffectBaseViewModel> Effects
        {
            get
            {
                return _Effects;
            }
            set
            {
                if (Equals(_Effects, value))
                {
                    return;
                }

                _Effects = value;

                NotifyPropertyChanged();
            }
        }

        private EffectBaseViewModel _SelectedEffect;
        public EffectBaseViewModel SelectedEffect
        {
            get
            {
                return _SelectedEffect;
            }
            set
            {
                if (Equals(_SelectedEffect, value))
                {
                    return;
                }

                _SelectedEffect = value;

                NotifyPropertyChanged();
                ApplyCommand.RaiseCanExecuteChangedNotification();
            }
        }

        public LightingViewModel()
        {
            LoadEffects();
        }

        private void LoadEffects()
        {
            ObservableCollection<EffectBaseViewModel> effects = new ObservableCollection<EffectBaseViewModel>();
            effects.Add(new SpectrumViewModel(new Spectrum()));
            effects.Add(new ScreenCaptureViewModel(new ScreenCapture()));
            effects.Add(new CustomEffectViewModel(new CustomEffect()));
            Effects = effects;

            LoadAndApplyDefaultEffect();
        }

        private void LoadAndApplyDefaultEffect()
        {
            GlobalSettings settings = SettingsManager.GetSettings();
            string selectedEffect = settings.SelectedEffect;
            if (string.IsNullOrWhiteSpace(selectedEffect))
            {
                SelectedEffect = Effects.First();
            }
            else
            {
                EffectBaseViewModel effect = Effects.FirstOrDefault(x => x.EffectName == selectedEffect);
                if (effect != null)
                {
                    SelectedEffect = effect;

                    if (settings.ProgramSettings.GeneralSettings.StartEffectOnStartup)
                    {
                        SelectedEffect.StartEffect();
                    }
                }
                else
                {
                    SelectedEffect = Effects.First();
                }
            }
        }

        private void StartEffect()
        {
            StopEffects(false);

            GlobalSettings settings = SettingsManager.GetSettings();
            settings.SelectedEffect = SelectedEffect.EffectName;
            SettingsManager.SaveSettings(settings);

            if (SelectedEffect != null)
            {
                SelectedEffect.StartEffect();
            }
        }

        private void StopEffects(bool save)
        {
            EffectManager.Instance.StopEffect();
            
            if (save)
            {
                GlobalSettings settings = SettingsManager.GetSettings();
                settings.SelectedEffect = string.Empty;
                SettingsManager.SaveSettings(settings);
            }
        }
    }
}
