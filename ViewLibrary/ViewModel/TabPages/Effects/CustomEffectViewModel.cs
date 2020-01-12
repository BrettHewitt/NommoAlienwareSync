using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using ViewLibrary.Commands;
using ViewLibrary.Extensions;
using ViewLibrary.Model.Effects;
using ViewLibrary.Model.Settings;

namespace ViewLibrary.ViewModel.TabPages.Effects
{
    public class CustomEffectViewModel : TempoBaseEffectViewModel
    {
        private ActionCommand _AddCommand;
        public ActionCommand AddCommand
        {
            get
            {
                return _AddCommand ?? (_AddCommand = new ActionCommand()
                {
                    ExecuteAction = AddColour,
                });
            }
        }

        private ActionCommand _RemoveCommand;
        public ActionCommand RemoveCommand
        {
            get
            {
                return _RemoveCommand ?? (_RemoveCommand = new ActionCommand()
                {
                    ExecuteAction = RemoveColour,
                    CanExecuteAction = CanRemoveColour
                });
            }
        }
        
        private Color _SelectedColourPicker;
        public Color SelectedColourPicker
        {
            get
            {
                return _SelectedColourPicker;
            }
            set
            {
                if (Equals(_SelectedColourPicker, value))
                {
                    return;
                }

                _SelectedColourPicker = value;

                NotifyPropertyChanged();
                UpdateSelectedColour();
            }
        }
        
        private ObservableCollection<ColourViewModel> _Colours;
        public ObservableCollection<ColourViewModel> Colours
        {
            get
            {
                return _Colours;
            }
            set
            {
                if (Equals(_Colours, value))
                {
                    return;
                }

                _Colours = value;

                NotifyPropertyChanged();
            }
        }

        private ColourViewModel _SelectedColour;
        public ColourViewModel SelectedColour
        {
            get
            {
                return _SelectedColour;
            }
            set
            {
                if (Equals(_SelectedColour, value))
                {
                    return;
                }

                _SelectedColour = value;

                NotifyPropertyChanged();
                RemoveCommand.RaiseCanExecuteChangedNotification();
                UpdateSelectedColourPicker();
            }
        }
        
        private CustomEffect _CustomModel;
        public CustomEffect CustomModel
        {
            get
            {
                return _CustomModel;
            }
            set
            {
                if (Equals(_CustomModel, value))
                {
                    return;
                }

                _CustomModel = value;

                NotifyPropertyChanged();
            }
        }

        public CustomEffectViewModel(CustomEffect model) : base(model)
        {
            CustomModel = model;
            Colours = new ObservableCollection<ColourViewModel>(CustomModel.Colours.Select(x => new ColourViewModel(x)));
            MinTempo = 0.00005;
            MaxTempo = 0.00015;
        }
        
        protected override void ApplyEffect()
        {
            if (!Colours.Any())
            {
                MessageBox.Show("There must be at least one colour", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            CustomModel.Colours = Colours.Select(x => x.Colour).ToArray();
            Model.StartEffect();
        }
        
        private void AddColour()
        {
            Colours.Add(new ColourViewModel(SelectedColourPicker.ToChromaColor()));
        }

        private void RemoveColour()
        {
            if (SelectedColour == null)
            {
                return;
            }

            Colours.Remove(SelectedColour);
        }

        private bool CanRemoveColour()
        {
            return SelectedColour != null;
        }

        private void UpdateSelectedColour()
        {
            if (SelectedColour != null)
            {
                SelectedColour.Colour = SelectedColourPicker.ToChromaColor();
            }
        }

        private void UpdateSelectedColourPicker()
        {
            if (SelectedColour != null)
            {
                SelectedColourPicker = SelectedColour.Colour.ToMediaColor();
            }
        }
    }

    public class ColourViewModel : ViewModelBase
    {
        private ChromaFX.Color _Colour;
        public ChromaFX.Color Colour
        {
            get
            {
                return _Colour;
            }
            set
            {
                if (Equals(_Colour, value))
                {
                    return;
                }

                _Colour = value;

                NotifyPropertyChanged();
            }
        }

        public ColourViewModel(ChromaFX.Color colour)
        {
            Colour = colour;
        }
    }
}
