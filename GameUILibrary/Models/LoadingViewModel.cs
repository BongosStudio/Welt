#region Copyright

// COPYRIGHT 2016 JUSTIN COX (CONJI)

#endregion Copyright

using EmptyKeys.UserInterface.Mvvm;

namespace GameUILibrary.Models
{
    public class LoadingViewModel : ViewModelBase
    {
        public string LoadingStatusText { get; set; }
        public string UsernameText { get; set; }
        public string WorldData { get; set; }
        public string WorldNameText { get; set; }
    }
}