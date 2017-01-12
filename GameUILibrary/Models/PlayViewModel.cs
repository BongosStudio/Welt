#region Copyright

// COPYRIGHT 2016 JUSTIN COX (CONJI)

#endregion Copyright

using EmptyKeys.UserInterface;
using EmptyKeys.UserInterface.Input;
using EmptyKeys.UserInterface.Mvvm;

namespace GameUILibrary.Models
{
    public class PlayViewModel : ViewModelBase
    {
        public RelayCommand OptionsButtonCommand { get; set; }
        private Visibility m_PmVis = Visibility.Collapsed;
        public Visibility PauseMenuVisibility
        {
            get { return m_PmVis; }
            set
            {
                if (m_PmVis == value) return;
                m_PmVis = value;
                RaisePropertyChanged();
            }
        }
        public RelayCommand QuitButtonCommand { get; set; }
        public RelayCommand ResumeButtonCommand { get; set; }
    }
}