using EmptyKeys.UserInterface.Input;
using EmptyKeys.UserInterface.Mvvm;

namespace GameUILibrary.Models
{
    public class SingleplayerViewModel : ViewModelBase
    {
        public string WorldName { get; set; } = "New world";
        public string WorldSeed { get; set; } = "12345";
        public bool IsLan { get; set; } = false;
        public ICommand ExitCommand { get; set; }
        public ICommand CreateWorldCommand { get; set; }
        public void SetGameMode(string type)
        {

        }
    }
}
