using EmptyKeys.UserInterface.Input;
using EmptyKeys.UserInterface.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameUILibrary.Models
{
    public class ErrorViewModel : ViewModelBase 
    {
        private string m_Result;
        public string Result
        {
            get { return m_Result; }
            set { SetProperty(ref m_Result, value); }
        }

        public ICommand ReturnCommand
        {
            get { return new RelayCommand(o => ReturnAction?.Invoke()); }
        }

        public Action ReturnAction;
    }
}
