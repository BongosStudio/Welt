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
    using EmptyKeys.UserInterface.Media.Effects;
    using EmptyKeys.UserInterface.Media.Animation;
    using EmptyKeys.UserInterface.Media.Imaging;
    using EmptyKeys.UserInterface.Shapes;
    using EmptyKeys.UserInterface.Renderers;
    using EmptyKeys.UserInterface.Themes;
    
    
    [GeneratedCodeAttribute("Empty Keys UI Generator", "2.6.0.0")]
    public partial class MainMenu : UIRoot {
        
        private Grid e_0;
        
        private Grid MainButtons;
        
        private StackPanel e_1;
        
        private Button e_2;
        
        private Button e_3;
        
        private Button e_4;
        
        private Button e_5;
        
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
            this.SetResourceReference(UIRoot.StyleProperty, "UiStyle");
            InitializeElementResources(this);
            // e_0 element
            this.e_0 = new Grid();
            this.Content = this.e_0;
            this.e_0.Name = "e_0";
            RowDefinition row_e_0_0 = new RowDefinition();
            row_e_0_0.Height = new GridLength(33F, GridUnitType.Star);
            this.e_0.RowDefinitions.Add(row_e_0_0);
            RowDefinition row_e_0_1 = new RowDefinition();
            row_e_0_1.Height = new GridLength(67F, GridUnitType.Star);
            this.e_0.RowDefinitions.Add(row_e_0_1);
            // MainButtons element
            this.MainButtons = new Grid();
            this.e_0.Children.Add(this.MainButtons);
            this.MainButtons.Name = "MainButtons";
            Grid.SetRowSpan(this.MainButtons, 2);
            // e_1 element
            this.e_1 = new StackPanel();
            this.MainButtons.Children.Add(this.e_1);
            this.e_1.Name = "e_1";
            this.e_1.HorizontalAlignment = HorizontalAlignment.Left;
            this.e_1.VerticalAlignment = VerticalAlignment.Center;
            // e_2 element
            this.e_2 = new Button();
            this.e_1.Children.Add(this.e_2);
            this.e_2.Name = "e_2";
            this.e_2.Height = 50F;
            this.e_2.Width = 200F;
            this.e_2.HorizontalAlignment = HorizontalAlignment.Left;
            this.e_2.FontFamily = new FontFamily("Code 7x5");
            this.e_2.FontSize = 18F;
            this.e_2.Content = "Singleplayer";
            Binding binding_e_2_Command = new Binding("SingleplayerButtonCommand");
            this.e_2.SetBinding(Button.CommandProperty, binding_e_2_Command);
            this.e_2.SetResourceReference(Button.StyleProperty, "ButtonStyle");
            // e_3 element
            this.e_3 = new Button();
            this.e_1.Children.Add(this.e_3);
            this.e_3.Name = "e_3";
            this.e_3.Height = 50F;
            this.e_3.Width = 200F;
            this.e_3.HorizontalAlignment = HorizontalAlignment.Left;
            this.e_3.FontFamily = new FontFamily("Code 7x5");
            this.e_3.FontSize = 18F;
            this.e_3.Content = "Multiplayer";
            Binding binding_e_3_Command = new Binding("MultiplayerButtonCommand");
            this.e_3.SetBinding(Button.CommandProperty, binding_e_3_Command);
            this.e_3.SetResourceReference(Button.StyleProperty, "ButtonStyle");
            // e_4 element
            this.e_4 = new Button();
            this.e_1.Children.Add(this.e_4);
            this.e_4.Name = "e_4";
            this.e_4.Height = 50F;
            this.e_4.Width = 200F;
            this.e_4.HorizontalAlignment = HorizontalAlignment.Left;
            this.e_4.FontFamily = new FontFamily("Code 7x5");
            this.e_4.FontSize = 18F;
            this.e_4.Content = "Settings";
            Binding binding_e_4_Command = new Binding("SettingsButtonCommand");
            this.e_4.SetBinding(Button.CommandProperty, binding_e_4_Command);
            this.e_4.SetResourceReference(Button.StyleProperty, "ButtonStyle");
            // e_5 element
            this.e_5 = new Button();
            this.e_1.Children.Add(this.e_5);
            this.e_5.Name = "e_5";
            this.e_5.Height = 50F;
            this.e_5.Width = 200F;
            this.e_5.HorizontalAlignment = HorizontalAlignment.Left;
            this.e_5.FontFamily = new FontFamily("Code 7x5");
            this.e_5.FontSize = 18F;
            this.e_5.Content = "Exit";
            Binding binding_e_5_Command = new Binding("ExitButtonCommand");
            this.e_5.SetBinding(Button.CommandProperty, binding_e_5_Command);
            this.e_5.SetResourceReference(Button.StyleProperty, "ButtonStyle");
            FontManager.Instance.AddFont("Code 7x5", 18F, FontStyle.Regular, "Code_7x5_13.5_Regular");
        }
        
        private static void InitializeElementResources(UIElement elem) {
            elem.Resources.MergedDictionaries.Add(Dictionary.Instance);
        }
    }
}
