using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using WeltLauncher.Core;

namespace WeltLauncher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private const string LAUNCHER_VERSION = "1.0.0";

        protected override void OnStartup(StartupEventArgs e)
        {
            CheckVersion();
            base.OnStartup(e);
            
            LoadStyles();
        }
        
        private void CheckVersion()
        {
            var client = new HttpClient();
            var version = client.GetStringAsync(ApiResources.GetUrl(ApiResources.ResxLauncherVer)).Result;
            if (version == LAUNCHER_VERSION) return;
            MessageBox.Show("An update is available. The launcher will now install and restart.", "", MessageBoxButton.OK);
            var data = client.GetByteArrayAsync(ApiResources.GetUrl(ApiResources.ResxLauncher)).Result;
            File.WriteAllBytes($"Welt-{LAUNCHER_VERSION}.exe", data);
            Process.Start($"Welt-{LAUNCHER_VERSION}.exe", AppDomain.CurrentDomain.FriendlyName);
            Shutdown();
        }

        private void LoadStyles()
        {
            // Load a custom styles file if it exists
            string currentAssembly = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string stylesBaseFile = Path.Combine(Path.GetDirectoryName(currentAssembly), Path.GetFileNameWithoutExtension(currentAssembly) + ".Styles");
            string stylesFile = stylesBaseFile + ".xaml";
            if (File.Exists(stylesFile) == false)
            {
                stylesFile = stylesBaseFile + ".Default.xaml";
            }

            try
            {
                using (Stream stream = new FileStream(stylesFile, FileMode.Open, FileAccess.Read))
                {
                    ResourceDictionary resources = (ResourceDictionary)XamlReader.Load(stream);
                    Resources.MergedDictionaries.Add(resources);
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show($"Unable to load styles file '{stylesFile}'.\n{ex.Message}");
            }
        }
    }
}
