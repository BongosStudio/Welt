using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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
using JsonSettings;
using WeltLauncher.Pages;
using WeltLauncher.Pages.Settings;

namespace WeltLauncher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ModernWindow
    {
        internal static string Token;
        public static Settings Settings = new Settings("launcher.json");
        public static AppearanceViewModel ViewModel = new AppearanceViewModel();

        public MainWindow()
        {
            InitializeComponent();

            var ip = new IPEndPoint(IPAddress.Any, 9032);
            var udp = new UdpClient(ip);
            var t = new Thread(() =>
            {
                while (true)
                {
                    if (!udp.Client.Connected)
                    {
                        try
                        {
                            udp.Connect(ip);
                        }
                        catch(SocketException e)
                        {
                            Dispatcher.Invoke(() => GameConsole.WriteLine("Couldn't connect to game session. " + e.SocketErrorCode));
                            Thread.Sleep(5000);
                        }
                    }
                    else
                    {
                        if (udp.Available == 0) Thread.Sleep(1000);
                        else
                        {
                            var result = udp.Receive(ref ip);
                            Dispatcher.Invoke(() => GameConsole.WriteLine(Encoding.UTF8.GetString(result)));
                            Thread.Sleep(1000);
                        }
                    }
                }
            })
            {IsBackground = true};
            t.SetApartmentState(ApartmentState.STA);
            //t.Start();
        }
    }
}
