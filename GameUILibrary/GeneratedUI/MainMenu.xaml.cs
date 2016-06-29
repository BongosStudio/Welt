// -----------------------------------------------------------
//  
//  This file was generated, please do not modify.
//  
// -----------------------------------------------------------
namespace EmptyKeys.UserInterface.Generated {
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.ObjectModel;
    using EmptyKeys.UserInterface;
    using EmptyKeys.UserInterface.Charts;
    using EmptyKeys.UserInterface.Data;
    using EmptyKeys.UserInterface.Controls;
    using EmptyKeys.UserInterface.Controls.Primitives;
    using EmptyKeys.UserInterface.Input;
    using EmptyKeys.UserInterface.Interactions.Core;
    using EmptyKeys.UserInterface.Interactivity;
    using EmptyKeys.UserInterface.Media;
    using EmptyKeys.UserInterface.Media.Animation;
    using EmptyKeys.UserInterface.Media.Imaging;
    using EmptyKeys.UserInterface.Shapes;
    using EmptyKeys.UserInterface.Renderers;
    using EmptyKeys.UserInterface.Themes;
    
    
    [GeneratedCodeAttribute("Empty Keys UI Generator", "2.2.0.0")]
    public partial class MainMenu : UIRoot {
        
        private Grid e_0;
        
        private Grid MainButtons;
        
        private StackPanel e_1;
        
        private Button spBtn;
        
        private Button e_2;
        
        private Button e_3;
        
        private Button e_4;
        
        private StackPanel spMenu;
        
        private TextBlock e_5;
        
        private TextBox e_6;
        
        private TextBlock e_7;
        
        private TextBox e_8;
        
        private TextBlock e_9;
        
        private RadioButton e_10;
        
        private RadioButton e_11;
        
        private RadioButton e_12;
        
        private CheckBox e_13;
        
        private Button e_14;
        
        public MainMenu() : 
                base() {
            this.Initialize();
        }
        
        public MainMenu(int width, int height) : 
                base(width, height) {
            this.Initialize();
        }
        
        private void Initialize() {
            Style style = RootStyle.CreateRootStyle();
            style.TargetType = this.GetType();
            this.Style = style;
            this.InitializeComponent();
        }
        
        private void InitializeComponent() {
            this.FontFamily = new FontFamily("Code 7x5");
            this.FontSize = 18F;
            InitializeElementResources(this);
            // e_0 element
            this.e_0 = new Grid();
            this.Content = this.e_0;
            this.e_0.Name = "e_0";
            // MainButtons element
            this.MainButtons = new Grid();
            this.e_0.Children.Add(this.MainButtons);
            this.MainButtons.Name = "MainButtons";
            // e_1 element
            this.e_1 = new StackPanel();
            this.MainButtons.Children.Add(this.e_1);
            this.e_1.Name = "e_1";
            this.e_1.HorizontalAlignment = HorizontalAlignment.Left;
            this.e_1.VerticalAlignment = VerticalAlignment.Center;
            // spBtn element
            this.spBtn = new Button();
            this.e_1.Children.Add(this.spBtn);
            this.spBtn.Name = "spBtn";
            this.spBtn.Height = 50F;
            this.spBtn.Width = 200F;
            this.spBtn.HorizontalAlignment = HorizontalAlignment.Left;
            EventTrigger spBtn_ET_0 = new EventTrigger(Button.ClickEvent, this.spBtn);
            spBtn.Triggers.Add(spBtn_ET_0);
            BeginStoryboard spBtn_ET_0_AC_0 = new BeginStoryboard();
            spBtn_ET_0_AC_0.Name = "spBtn_ET_0_AC_0";
            spBtn_ET_0.AddAction(spBtn_ET_0_AC_0);
            Storyboard spBtn_ET_0_AC_0_SB = new Storyboard();
            spBtn_ET_0_AC_0.Storyboard = spBtn_ET_0_AC_0_SB;
            spBtn_ET_0_AC_0_SB.Name = "spBtn_ET_0_AC_0_SB";
            FloatAnimation spBtn_ET_0_AC_0_SB_TL_0 = new FloatAnimation();
            spBtn_ET_0_AC_0_SB_TL_0.Name = "spBtn_ET_0_AC_0_SB_TL_0";
            spBtn_ET_0_AC_0_SB_TL_0.Duration = new Duration(new TimeSpan(0, 0, 0, 0, 200));
            spBtn_ET_0_AC_0_SB_TL_0.To = 1F;
            SineEase spBtn_ET_0_AC_0_SB_TL_0_EA = new SineEase();
            spBtn_ET_0_AC_0_SB_TL_0.EasingFunction = spBtn_ET_0_AC_0_SB_TL_0_EA;
            Storyboard.SetTargetName(spBtn_ET_0_AC_0_SB_TL_0, "spMenu");
            Storyboard.SetTargetProperty(spBtn_ET_0_AC_0_SB_TL_0, Button.OpacityProperty);
            spBtn_ET_0_AC_0_SB.Children.Add(spBtn_ET_0_AC_0_SB_TL_0);
            this.spBtn.FontFamily = new FontFamily("Code 7x5");
            this.spBtn.FontSize = 18F;
            this.spBtn.Content = "Singleplayer";
            Binding binding_spBtn_Command = new Binding("SinglePlayerButtonCommand");
            this.spBtn.SetBinding(Button.CommandProperty, binding_spBtn_Command);
            this.spBtn.SetResourceReference(Button.StyleProperty, "ButtonAnimStyle");
            // e_2 element
            this.e_2 = new Button();
            this.e_1.Children.Add(this.e_2);
            this.e_2.Name = "e_2";
            this.e_2.Height = 50F;
            this.e_2.Width = 200F;
            this.e_2.IsEnabled = false;
            this.e_2.HorizontalAlignment = HorizontalAlignment.Left;
            this.e_2.FontFamily = new FontFamily("Code 7x5");
            this.e_2.FontSize = 18F;
            this.e_2.Content = "Multiplayer";
            Binding binding_e_2_Command = new Binding("MultiPlayerButtonCommand");
            this.e_2.SetBinding(Button.CommandProperty, binding_e_2_Command);
            this.e_2.SetResourceReference(Button.StyleProperty, "ButtonAnimStyle");
            // e_3 element
            this.e_3 = new Button();
            this.e_1.Children.Add(this.e_3);
            this.e_3.Name = "e_3";
            this.e_3.Height = 50F;
            this.e_3.Width = 200F;
            this.e_3.IsEnabled = false;
            this.e_3.HorizontalAlignment = HorizontalAlignment.Left;
            this.e_3.FontFamily = new FontFamily("Code 7x5");
            this.e_3.FontSize = 18F;
            this.e_3.Content = "Settings";
            Binding binding_e_3_Command = new Binding("SettingsButtonCommand");
            this.e_3.SetBinding(Button.CommandProperty, binding_e_3_Command);
            this.e_3.SetResourceReference(Button.StyleProperty, "ButtonAnimStyle");
            // e_4 element
            this.e_4 = new Button();
            this.e_1.Children.Add(this.e_4);
            this.e_4.Name = "e_4";
            this.e_4.Height = 50F;
            this.e_4.Width = 200F;
            this.e_4.HorizontalAlignment = HorizontalAlignment.Left;
            this.e_4.FontFamily = new FontFamily("Code 7x5");
            this.e_4.FontSize = 18F;
            this.e_4.Content = "Exit";
            Binding binding_e_4_Command = new Binding("ExitButtonCommand");
            this.e_4.SetBinding(Button.CommandProperty, binding_e_4_Command);
            this.e_4.SetResourceReference(Button.StyleProperty, "ButtonAnimStyle");
            // spMenu element
            this.spMenu = new StackPanel();
            this.e_0.Children.Add(this.spMenu);
            this.spMenu.Name = "spMenu";
            this.spMenu.Width = 400F;
            this.spMenu.Margin = new Thickness(261F, 201F, 0F, 60F);
            this.spMenu.HorizontalAlignment = HorizontalAlignment.Left;
            this.spMenu.Opacity = 0F;
            // e_5 element
            this.e_5 = new TextBlock();
            this.spMenu.Children.Add(this.e_5);
            this.e_5.Name = "e_5";
            this.e_5.HorizontalAlignment = HorizontalAlignment.Left;
            this.e_5.Text = "World Name";
            // e_6 element
            this.e_6 = new TextBox();
            this.spMenu.Children.Add(this.e_6);
            this.e_6.Name = "e_6";
            this.e_6.Width = 241F;
            this.e_6.HorizontalAlignment = HorizontalAlignment.Left;
            this.e_6.VerticalAlignment = VerticalAlignment.Top;
            Binding binding_e_6_Text = new Binding("WorldName");
            this.e_6.SetBinding(TextBox.TextProperty, binding_e_6_Text);
            // e_7 element
            this.e_7 = new TextBlock();
            this.spMenu.Children.Add(this.e_7);
            this.e_7.Name = "e_7";
            this.e_7.HorizontalAlignment = HorizontalAlignment.Left;
            this.e_7.Text = "Seed";
            // e_8 element
            this.e_8 = new TextBox();
            this.spMenu.Children.Add(this.e_8);
            this.e_8.Name = "e_8";
            this.e_8.Width = 241F;
            this.e_8.HorizontalAlignment = HorizontalAlignment.Left;
            this.e_8.VerticalAlignment = VerticalAlignment.Top;
            Binding binding_e_8_Text = new Binding("WorldSeed");
            this.e_8.SetBinding(TextBox.TextProperty, binding_e_8_Text);
            // e_9 element
            this.e_9 = new TextBlock();
            this.spMenu.Children.Add(this.e_9);
            this.e_9.Name = "e_9";
            this.e_9.HorizontalAlignment = HorizontalAlignment.Left;
            this.e_9.VerticalAlignment = VerticalAlignment.Top;
            this.e_9.Text = "game mode";
            this.e_9.TextWrapping = TextWrapping.Wrap;
            // e_10 element
            this.e_10 = new RadioButton();
            this.spMenu.Children.Add(this.e_10);
            this.e_10.Name = "e_10";
            this.e_10.Content = "Realistic";
            this.e_10.CommandParameter = "realistic";
            this.e_10.IsChecked = true;
            this.e_10.GroupName = "gm";
            Binding binding_e_10_Command = new Binding("SetGameMode");
            this.e_10.SetBinding(RadioButton.CommandProperty, binding_e_10_Command);
            // e_11 element
            this.e_11 = new RadioButton();
            this.spMenu.Children.Add(this.e_11);
            this.e_11.Name = "e_11";
            this.e_11.Content = "Story";
            this.e_11.CommandParameter = "story";
            this.e_11.GroupName = "gm";
            Binding binding_e_11_Command = new Binding("SetGameMode");
            this.e_11.SetBinding(RadioButton.CommandProperty, binding_e_11_Command);
            // e_12 element
            this.e_12 = new RadioButton();
            this.spMenu.Children.Add(this.e_12);
            this.e_12.Name = "e_12";
            this.e_12.Content = "Limitless";
            this.e_12.CommandParameter = "limitless";
            this.e_12.GroupName = "gm";
            Binding binding_e_12_Command = new Binding("SetGameMode");
            this.e_12.SetBinding(RadioButton.CommandProperty, binding_e_12_Command);
            // e_13 element
            this.e_13 = new CheckBox();
            this.spMenu.Children.Add(this.e_13);
            this.e_13.Name = "e_13";
            this.e_13.Content = "Enable LAN";
            // e_14 element
            this.e_14 = new Button();
            this.spMenu.Children.Add(this.e_14);
            this.e_14.Name = "e_14";
            this.e_14.Height = 50F;
            this.e_14.HorizontalAlignment = HorizontalAlignment.Left;
            this.e_14.Content = "Create World";
            Binding binding_e_14_Command = new Binding("CreateNewWorldCommand");
            this.e_14.SetBinding(Button.CommandProperty, binding_e_14_Command);
            FontManager.Instance.AddFont("Code 7x5", 18F, FontStyle.Regular, "Code_7x5_13.5_Regular");
        }
        
        private static void InitializeElementResources(UIElement elem) {
            elem.Resources.MergedDictionaries.Add(Dictionary.Instance);
        }
    }
}
