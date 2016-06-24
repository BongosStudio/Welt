#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using EmptyKeys.UserInterface;
using EmptyKeys.UserInterface.Mvvm;

namespace GameUILibrary.Models
{
    public class PlayViewModel : ViewModelBase
    {
        public Visibility PauseMenuVisibility { get; set; } = Visibility.Collapsed;
    }
}