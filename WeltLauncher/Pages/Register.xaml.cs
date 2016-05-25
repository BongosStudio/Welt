using System;
using System.Collections.Generic;
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

namespace WeltLauncher.Pages
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : UserControl
    {
        public Register()
        {
            InitializeComponent();
        }

        private async void RegisterAccount(object sender, RoutedEventArgs e)
        {
            try
            {
                UsernameTxt.IsEnabled = false;
                PasswordTxt.IsEnabled = false;
                ConfirmPasswordTxt.IsEnabled = false;
                EmailTxt.IsEnabled = false;
                var json = JObject.FromObject(
                    new
                    {
                        username = UsernameTxt.Text,
                        password = PasswordTxt.Password,
                        confirm_password = ConfirmPasswordTxt.Password,
                        email = EmailTxt.Text
                    });
                var web = new WebClient {Headers = {["Content-Type"] = "application/json"}};
                var response = await web.UploadStringTaskAsync(ApiResources.TEST_USER_REG_URL, json.ToString());
                var jr = JObject.Parse(response);
                var token = jr["token"].ToString(); // TODO: use this lol
                ResponseTxt.Text = $"{jr["message"]} with the token {token}";           
                MainWindow.Token = token;
            }
            catch (Exception ex)
            {
                ResponseTxt.Text = ex.Message;
            }
            finally
            {
                UsernameTxt.IsEnabled = true;
                PasswordTxt.IsEnabled = true;
                ConfirmPasswordTxt.IsEnabled = true;
                EmailTxt.IsEnabled = true;
            }
        }
    }
}
