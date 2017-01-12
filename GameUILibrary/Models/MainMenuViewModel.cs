#region Copyright

// COPYRIGHT 2016 JUSTIN COX (CONJI)

#endregion Copyright

using EmptyKeys.UserInterface;
using EmptyKeys.UserInterface.Input;
using EmptyKeys.UserInterface.Mvvm;
using System.ComponentModel;

namespace GameUILibrary.Models
{
    public class MainMenuViewModel : ViewModelBase, INotifyPropertyChanged 
    {
        public ICommand SingleplayerButtonCommand { get; set; }
        public ICommand MultiplayerButtonCommand { get; set; }
        public ICommand SettingsButtonCommand { get; set; }
        public ICommand ExitButtonCommand { get; set; }
    }
}