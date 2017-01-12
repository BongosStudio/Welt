using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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
        private readonly AuthService _auth;
        private static readonly HttpClient _client = new HttpClient();
        private static bool _hasInstalledUpdates = false;
        private static Task _updateTask;

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
                var game = Process.Start("welt.exe", $"{UsernameTxt.Text} {json["token"]}");
                if (game != null)
                {
                    game.OutputDataReceived += (sender, args) => GameConsole.WriteLine(args.Data);
                }
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

        private async void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            UpdateMd.Document = new Markdown().Transform(GetChangelogString());
            UpdateMd.Document.FontFamily = new FontFamily("Arial");

            #region Get game version

            StatusTxt.Text = "Checking resources.";
            if (await RequiresUpdate())
            {
                StatusTxt.Text = "Beginning update.";

                #region Download the new game

                var game = await _client.GetByteArrayAsync(ApiResources.GetUrl(ApiResources.RESX_DL));
                File.WriteAllBytes("Welt.exe", game);

                #endregion

                #region Download resource objects

                var resourceJson = JArray.Parse(await _client.GetStringAsync(ApiResources.GetUrl(ApiResources.RESX_OBJ)));
                foreach (var r in resourceJson.Select(j => (JObject) j))
                {
                    var file = r["file"].ToString();
                    var url = r["url"].ToString();
                    StatusTxt.Text = $"Installing {file}.";
                    // this means it belongs in a directory
                    if (file.Contains("\\")) Directory.CreateDirectory(file.Substring(0, file.LastIndexOf('\\')));
                    var data = await _client.GetByteArrayAsync(ApiResources.GetUrl(ApiResources.RESX_DL + url));
                    File.WriteAllBytes(file, data);
                }

                #endregion

                _hasInstalledUpdates = true;
            }
            StatusTxt.Text = "";

            #endregion
        }

        private static async Task<bool> RequiresUpdate()
        {
            if (_hasInstalledUpdates) return false;
            var clientv = MainWindow.Settings["version"];
            var gamev = await _client.GetStringAsync(ApiResources.GetUrl(ApiResources.RESX_VER));
            MainWindow.Settings.Save();
            return gamev != clientv;
        }

        private static string GetChangelogString()
        {
            var client = new WebClient();
            try
            {
                var builder = new StringBuilder();
                var response = JArray.Parse(client.DownloadString(ApiResources.GetUrl(ApiResources.CHANGELOG)));
                foreach (var ja in response.Reverse())
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
            LoginBtn.IsEnabled = (UsernameTxt.Text.Length > 0 && PasswordTxt.Password.Length > 4);
            if (e.Key == Key.Enter && LoginBtn.IsEnabled) LoginUser();
        }
    }
}
