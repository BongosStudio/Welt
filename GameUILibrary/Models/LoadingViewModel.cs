#region Copyright

// COPYRIGHT 2016 JUSTIN COX (CONJI)

#endregion Copyright

using EmptyKeys.UserInterface.Mvvm;

namespace GameUILibrary.Models
{
    public class LoadingViewModel : ViewModelBase
    {
        private string m_Status = "";
        private string m_Username = "";
        private string m_WorldData = "";
        private string m_WorldName = "";
        private int m_LoadingStatus = 0;
        private string m_HintText = "";
        private double m_Opacity = 1;

        public string LoadingStatusText
        {
            get { return m_Status; }
            set { SetProperty(ref m_Status, value ?? ""); }
        }
        public string UsernameText
        {
            get { return m_Username; }
            set { SetProperty(ref m_Username, value ?? ""); }
        }
        public string WorldData
        {
            get { return m_WorldData; }
            set { SetProperty(ref m_WorldData, value ?? ""); }
        }
        public string WorldNameText
        {
            get { return m_WorldName; }
            set { SetProperty(ref m_WorldName, value ?? ""); }
        }

        public int LoadingStatus
        {
            get { return m_LoadingStatus; }
            set { SetProperty(ref m_LoadingStatus, value); }
        }

        public string HintText
        {
            get { return m_HintText; }
            set { SetProperty(ref m_HintText, value ?? ""); }
        }

        public double Opacity
        {
            get { return m_Opacity; }
            set { SetProperty(ref m_Opacity, value); }
        }
    }
}