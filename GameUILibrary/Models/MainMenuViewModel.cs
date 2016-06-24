#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using EmptyKeys.UserInterface;
using EmptyKeys.UserInterface.Input;
using EmptyKeys.UserInterface.Mvvm;

namespace GameUILibrary.Models
{
    public class MainMenuViewModel : ViewModelBase
    {
        public ICommand SinglePlayerButtonCommand { get; set; }
        public ICommand MultiPlayerButtonCommand { get; set; }
        public ICommand SettingsButtonCommand { get; set; }
        public ICommand ExitButtonCommand { get; set; }

        #region SinglePlayer

        private bool _spMenuEnabled;

        public bool SpMenuEnabled
        {
            get { return _spMenuEnabled; }
            set
            {
                if (_spMenuEnabled == value) return;
                _spMenuEnabled = value;
                MpMenuEnabled = false;
            }
        }
        public Visibility SpMenuVisibility => SpMenuEnabled ? Visibility.Visible : Visibility.Collapsed;
        public string WorldName { get; set; }
        public string GameType { get; set; }
        public bool EnableLan { get;set; }
        public ICommand CreateNewWorldCommand { get; set; }
        #endregion
        #region MultiPlayer

        private bool _mpMenuEnabled;

        public bool MpMenuEnabled
        {
            get { return _mpMenuEnabled; }
            set
            {
                if (_mpMenuEnabled == value) return;
                _mpMenuEnabled = value;
                SpMenuEnabled = false;
            }
        }
        public Visibility MpMenuVisibility => MpMenuEnabled ? Visibility.Visible : Visibility.Collapsed;
        public string[] ServerNames { get; set; }
        #endregion
    }
}