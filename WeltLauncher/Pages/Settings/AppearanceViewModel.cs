using FirstFloor.ModernUI.Presentation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WeltLauncher.Pages.Settings
{
    /// <summary>
    /// A simple view model for configuring theme, font and accent colors.
    /// </summary>
    public class AppearanceViewModel : NotifyPropertyChanged
    {
        private const string FONT_SMALL = "small";
        private const string FONT_LARGE = "large";

        // 9 accent colors from metro design principles
        /*private Color[] accentColors = new Color[]{
            Color.FromRgb(0x33, 0x99, 0xff),   // blue
            Color.FromRgb(0x00, 0xab, 0xa9),   // teal
            Color.FromRgb(0x33, 0x99, 0x33),   // green
            Color.FromRgb(0x8c, 0xbf, 0x26),   // lime
            Color.FromRgb(0xf0, 0x96, 0x09),   // orange
            Color.FromRgb(0xff, 0x45, 0x00),   // orange red
            Color.FromRgb(0xe5, 0x14, 0x00),   // red
            Color.FromRgb(0xff, 0x00, 0x97),   // magenta
            Color.FromRgb(0xa2, 0x00, 0xff),   // purple            
        };*/

        // 20 accent colors from Windows Phone 8

        private Color _selectedAccentColor;
        private Link _selectedTheme;
        private string _selectedFontSize;

        public AppearanceViewModel()
        {
            // add the default themes
            Themes.Add(new Link {DisplayName = "dark", Source = AppearanceManager.DarkThemeSource});
            Themes.Add(new Link {DisplayName = "light", Source = AppearanceManager.LightThemeSource});
            LoadFromSettings();
            SelectedFontSize = AppearanceManager.Current.FontSize == FontSize.Large ? FONT_LARGE : FONT_SMALL;
            SyncThemeAndColor();

            AppearanceManager.Current.PropertyChanged += OnAppearanceManagerPropertyChanged;
        }

        private static void LoadFromSettings()
        {

            try
            {
                var colordata = MainWindow.Settings["accent_color"].Split(';');

                AppearanceManager.Current.AccentColor = Color.FromRgb(
                    byte.Parse(colordata[0]),
                    byte.Parse(colordata[1]),
                    byte.Parse(colordata[2])
                    );
                AppearanceManager.Current.ThemeSource = new Uri(MainWindow.Settings["theme_source"], UriKind.Relative);
                FontSize size;
                if (Enum.TryParse(MainWindow.Settings["font_size"], true, out size))
                {
                    AppearanceManager.Current.FontSize = size;
                }
            }
            catch (Exception e)
            {
                GameConsole.WriteLine($"An error occured when loading visual settings.\r\n{e.Message}");
            }
        }

        private void SyncThemeAndColor()
        {
            // synchronizes the selected viewmodel theme with the actual theme used by the appearance manager.
            SelectedTheme = Themes.FirstOrDefault(l => l.Source.Equals(AppearanceManager.Current.ThemeSource));

            // and make sure accent color is up-to-date
            SelectedAccentColor = AppearanceManager.Current.AccentColor;
            MainWindow.Settings["theme_source"] = AppearanceManager.Current.ThemeSource.ToString();
            MainWindow.Settings["accent_color"] =
                $"{SelectedAccentColor.R};{SelectedAccentColor.G};{SelectedAccentColor.B}";
            MainWindow.Settings.Save();
        }

        private void OnAppearanceManagerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ThemeSource" || e.PropertyName == "AccentColor")
            {
                SyncThemeAndColor();
            }
        }

        public LinkCollection Themes { get; } = new LinkCollection();

        public string[] FontSizes => new[] { FONT_SMALL, FONT_LARGE };

        public Color[] AccentColors { get; } = {
            Color.FromRgb(0xa4, 0xc4, 0x00),   // lime
            Color.FromRgb(0x60, 0xa9, 0x17),   // green
            Color.FromRgb(0x00, 0x8a, 0x00),   // emerald
            Color.FromRgb(0x00, 0xab, 0xa9),   // teal
            Color.FromRgb(0x1b, 0xa1, 0xe2),   // cyan
            Color.FromRgb(0x00, 0x50, 0xef),   // cobalt
            Color.FromRgb(0x6a, 0x00, 0xff),   // indigo
            Color.FromRgb(0xaa, 0x00, 0xff),   // violet
            Color.FromRgb(0xf4, 0x72, 0xd0),   // pink
            Color.FromRgb(0xd8, 0x00, 0x73),   // magenta
            Color.FromRgb(0xa2, 0x00, 0x25),   // crimson
            Color.FromRgb(0xe5, 0x14, 0x00),   // red
            Color.FromRgb(0xfa, 0x68, 0x00),   // orange
            Color.FromRgb(0xf0, 0xa3, 0x0a),   // amber
            Color.FromRgb(0xe3, 0xc8, 0x00),   // yellow
            Color.FromRgb(0x82, 0x5a, 0x2c),   // brown
            Color.FromRgb(0x6d, 0x87, 0x64),   // olive
            Color.FromRgb(0x64, 0x76, 0x87),   // steel
            Color.FromRgb(0x76, 0x60, 0x8a),   // mauve
            Color.FromRgb(0x87, 0x79, 0x4e),   // taupe
        };

        public Link SelectedTheme
        {
            get { return _selectedTheme; }
            set
            {
                if (_selectedTheme != value)
                {
                    _selectedTheme = value;
                    OnPropertyChanged("SelectedTheme");

                    // and update the actual theme
                    AppearanceManager.Current.ThemeSource = value.Source;
                }
            }
        }

        public string SelectedFontSize
        {
            get { return _selectedFontSize; }
            set
            {
                if (_selectedFontSize != value)
                {
                    _selectedFontSize = value;
                    OnPropertyChanged("SelectedFontSize");
                    MainWindow.Settings["font_size"] = value == FONT_LARGE ? "large" : "small";
                    AppearanceManager.Current.FontSize = value == FONT_LARGE ? FontSize.Large : FontSize.Small;
                }
            }
        }

        public Color SelectedAccentColor
        {
            get { return _selectedAccentColor; }
            set
            {
                if (_selectedAccentColor != value)
                {
                    _selectedAccentColor = value;
                    OnPropertyChanged("SelectedAccentColor");

                    AppearanceManager.Current.AccentColor = value;
                }
            }
        }
    }
}
