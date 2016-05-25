using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
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
using Newtonsoft.Json.Linq;
using WeltLauncher.Core;
using WeltLauncher.Core.Net;
using WeltLauncher.MdXaml;

namespace WeltLauncher.Pages
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : UserControl
    {
        private AuthService _auth;

        public Home()
        {
            _auth = new AuthService();
            InitializeComponent();
            LoginBtn.Click += (sender, args) => LoginUser();
        }

        private async void LoginUser()
        {
            LoginBtn.IsEnabled = false;
            var login = await _auth.AttemptLogin(UsernameTxt.Text, PasswordTxt.Password);
            var json = JObject.Parse(await login.Content.ReadAsStringAsync());
            var message = json["message"].ToString();
            if (login.IsSuccessStatusCode)
            {
                StatusTxt.Foreground = new SolidColorBrush(FirstFloor.ModernUI.Presentation.AppearanceManager.Current.AccentColor);
                Process.Start("welt.exe", $"{UsernameTxt.Text} {json["token"]}");
                if (MainWindow.Settings["keep_open"] != "true")
                {
                    Application.Current.Shutdown();
                }
            }
            else
            {
                StatusTxt.Foreground = new SolidColorBrush(Colors.Red);
            }
            LoginBtn.IsEnabled = true;
            StatusTxt.Text = message;
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            UpdateMd.Document = new Markdown().Transform(GetChangelogString());
        }
        

        private static string GetChangelogString()
        {
            var client = new WebClient();
            try
            {
                var builder = new StringBuilder();
                var response = JArray.Parse(client.DownloadString(ApiResources.TEST_CHANGELOG_URL));
                foreach (var ja in response)
                {
                    builder.Append($"{ja["date"]}\r\n===========\r\n\r\n{ja["text"]}\r\n\r\n");
                }
                return builder.ToString();
            }
            catch
            {
                return "Can't fetch changelog. To try again, please restart the client.";
            }
        }

        private void PasswordTxt_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (UsernameTxt.Text.Length > 0 && PasswordTxt.Password.Length > 5) LoginBtn.IsEnabled = true;
            else LoginBtn.IsEnabled = false;
            if (e.Key == Key.Enter && LoginBtn.IsEnabled) LoginUser();
        }
    }
}
