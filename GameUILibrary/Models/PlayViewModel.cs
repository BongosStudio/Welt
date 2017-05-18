using EmptyKeys.UserInterface.Mvvm;
using System;

namespace GameUILibrary.Models
{
    public class PlayViewModel : ViewModelBase
    {
        private string m_SelectedItemName;
        public string SelectedItemName
        {
            get { return m_SelectedItemName; }
            set
            {
                if (value == "Air") m_SelectedItemName = "";
                else m_SelectedItemName = value ?? "";
                RaisePropertyChanged();
            }
        }

        private string m_ChatBoxText;

        public string ChatBoxText
        {
            get { return m_ChatBoxText; }
            set
            {
                m_ChatBoxText = value ?? "";
                RaisePropertyChanged();
            }
        }

        private string m_ChatBoxInput;
        public string ChatBoxInput
        {
            get { return m_ChatBoxInput; }
            set
            {
                m_ChatBoxInput = ChatBoxInput ?? "";
                RaisePropertyChanged();
            }
        }

        private bool m_ChatBoxInputSelected;
        public bool IsChatBoxSelected
        {
            get { return m_ChatBoxInputSelected; }
            set
            {
                m_ChatBoxInputSelected = value;
                RaisePropertyChanged();
            }
        }

        private string m_TooltipText = "";
        public string TooltipText
        {
            get { return m_TooltipText; }
            set { SetProperty(ref m_TooltipText, value ?? ""); }
        }

        private double m_TooltipOpacity = 0;
        public double TooltipOpacity
        {
            get { return m_TooltipOpacity; }
            set { SetProperty(ref m_TooltipOpacity, value); }
        }

        public void AddMessage(string message)
        {
            ChatBoxText += Environment.NewLine + message;
        }
    }
}
