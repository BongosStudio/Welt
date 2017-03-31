using EmptyKeys.UserInterface.Input;
using EmptyKeys.UserInterface.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameUILibrary.Models
{
    public class MultiplayerViewModel : ViewModelBase
    {
        private ServerModel[] m_ServerList;
        public ServerModel[] ServerList
        {
            get { return m_ServerList; }
            set { SetProperty(ref m_ServerList, value); }
        }

        private string m_NewIpAddressText = "Type IP";
        public string NewIPAddressText
        {
            get { return m_NewIpAddressText; }
            set { SetProperty(ref m_NewIpAddressText, value); }
        }

        private bool m_IsCompletedAddition = true;
        public bool IsCompletedAddition
        {
            get { return m_IsCompletedAddition; }
            set { SetProperty(ref m_IsCompletedAddition, value); }
        }
        private string m_StatusText;
        public string StatusText
        {
            get { return m_StatusText; }
            set { SetProperty(ref m_StatusText, value); }
        }

        public ICommand AddNewIPCommand { get; set; }
        public ICommand ExitCommand { get; set; }

        public void SetStatus(string text)
        {
            StatusText = text;
        }
    }

    public class ServerModel
    {
        public string ServerName { get; set; }
        public string ServerMotd { get; set; }
        public string CurrentPlayerCount { get; set; }

        public ICommand JoinServerCommand { get; set; }
    }
}
