using System;
using System.Windows.Controls;
using System.Windows.Threading;

namespace WeltLauncher.Pages
{
    /// <summary>
    /// Interaction logic for GameConsole.xaml
    /// </summary>
    public partial class GameConsole : UserControl
    {
        private static GameConsole _instance;
        public static GameConsole Instance
        {
            get { return _instance ?? (_instance = new GameConsole()); }
            set { _instance = value; }
        }

        public GameConsole()
        {
            InitializeComponent();
            Instance = this;
        }

        public static void Write(string input)
        {
            Instance.Log.Text += $"{DateTime.Now.ToShortTimeString()}: {input}";
        }

        public static void WriteLine(string input)
        {
            Write(input + "\r\n");
        }
    }
}
