using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WeltLauncher.Pages.Settings
{
    /// <summary>
    /// Interaction logic for LauncherSettings.xaml
    /// </summary>
    public partial class LauncherSettings : UserControl
    {
        public LauncherSettings()
        {
            InitializeComponent();
            ToggleKeepOpen.IsChecked = MainWindow.Settings["keep_open"] == "true";
            ToggleKeepOpen.Checked +=
                (sender, args) =>
                {
                    MainWindow.Settings.ChangeSetting("keep_open", "true");
                    MainWindow.Settings.Save();
                };
            ToggleKeepOpen.Unchecked +=
                (sender, args) =>
                {
                    MainWindow.Settings.ChangeSetting("keep_open", "false");
                    MainWindow.Settings.Save();
                };
        }
    }
}
