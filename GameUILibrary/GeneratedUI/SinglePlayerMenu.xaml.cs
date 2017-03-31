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
    public partial class SingleplayerMenu : UIRoot {
        
        private Grid e_0;
        
        private StackPanel e_1;
        
        private TextBlock e_2;
        
        private TextBox e_3;
        
        private TextBlock e_4;
        
        private TextBox e_5;
        
        private TextBlock e_6;
        
        private RadioButton e_7;
        
        private RadioButton e_8;
        
        private RadioButton e_9;
        
        private CheckBox e_10;
        
        private Button e_11;
        
        private Button e_12;
        
        public SingleplayerMenu() : 
                base() {
            this.Initialize();
        }
        
        public SingleplayerMenu(int width, int height) : 
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
            // e_1 element
            this.e_1 = new StackPanel();
            this.e_0.Children.Add(this.e_1);
            this.e_1.Name = "e_1";
            this.e_1.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_1.VerticalAlignment = VerticalAlignment.Center;
            // e_2 element
            this.e_2 = new TextBlock();
            this.e_1.Children.Add(this.e_2);
            this.e_2.Name = "e_2";
            this.e_2.HorizontalAlignment = HorizontalAlignment.Left;
            this.e_2.Text = "World Name";
            // e_3 element
            this.e_3 = new TextBox();
            this.e_1.Children.Add(this.e_3);
            this.e_3.Name = "e_3";
            this.e_3.Width = 241F;
            this.e_3.HorizontalAlignment = HorizontalAlignment.Left;
            this.e_3.VerticalAlignment = VerticalAlignment.Top;
            Binding binding_e_3_Text = new Binding("WorldName");
            this.e_3.SetBinding(TextBox.TextProperty, binding_e_3_Text);
            // e_4 element
            this.e_4 = new TextBlock();
            this.e_1.Children.Add(this.e_4);
            this.e_4.Name = "e_4";
            this.e_4.HorizontalAlignment = HorizontalAlignment.Left;
            this.e_4.Text = "Seed";
            // e_5 element
            this.e_5 = new TextBox();
            this.e_1.Children.Add(this.e_5);
            this.e_5.Name = "e_5";
            this.e_5.Width = 241F;
            this.e_5.HorizontalAlignment = HorizontalAlignment.Left;
            this.e_5.VerticalAlignment = VerticalAlignment.Top;
            Binding binding_e_5_Text = new Binding("WorldSeed");
            this.e_5.SetBinding(TextBox.TextProperty, binding_e_5_Text);
            // e_6 element
            this.e_6 = new TextBlock();
            this.e_1.Children.Add(this.e_6);
            this.e_6.Name = "e_6";
            this.e_6.HorizontalAlignment = HorizontalAlignment.Left;
            this.e_6.VerticalAlignment = VerticalAlignment.Top;
            this.e_6.Text = "game mode";
            this.e_6.TextWrapping = TextWrapping.Wrap;
            // e_7 element
            this.e_7 = new RadioButton();
            this.e_1.Children.Add(this.e_7);
            this.e_7.Name = "e_7";
            this.e_7.Content = "Realistic";
            this.e_7.CommandParameter = "realistic";
            this.e_7.IsChecked = true;
            this.e_7.GroupName = "gm";
            Binding binding_e_7_Command = new Binding("SetGameMode");
            this.e_7.SetBinding(RadioButton.CommandProperty, binding_e_7_Command);
            // e_8 element
            this.e_8 = new RadioButton();
            this.e_1.Children.Add(this.e_8);
            this.e_8.Name = "e_8";
            this.e_8.Content = "Story";
            this.e_8.CommandParameter = "story";
            this.e_8.GroupName = "gm";
            Binding binding_e_8_Command = new Binding("SetGameMode");
            this.e_8.SetBinding(RadioButton.CommandProperty, binding_e_8_Command);
            // e_9 element
            this.e_9 = new RadioButton();
            this.e_1.Children.Add(this.e_9);
            this.e_9.Name = "e_9";
            this.e_9.Content = "Limitless";
            this.e_9.CommandParameter = "limitless";
            this.e_9.GroupName = "gm";
            Binding binding_e_9_Command = new Binding("SetGameMode");
            this.e_9.SetBinding(RadioButton.CommandProperty, binding_e_9_Command);
            // e_10 element
            this.e_10 = new CheckBox();
            this.e_1.Children.Add(this.e_10);
            this.e_10.Name = "e_10";
            this.e_10.Content = "Enable LAN";
            Binding binding_e_10_IsChecked = new Binding("IsLan");
            this.e_10.SetBinding(CheckBox.IsCheckedProperty, binding_e_10_IsChecked);
            // e_11 element
            this.e_11 = new Button();
            this.e_1.Children.Add(this.e_11);
            this.e_11.Name = "e_11";
            this.e_11.Height = 50F;
            this.e_11.HorizontalAlignment = HorizontalAlignment.Left;
            this.e_11.Content = "Create World";
            Binding binding_e_11_Command = new Binding("CreateWorldCommand");
            this.e_11.SetBinding(Button.CommandProperty, binding_e_11_Command);
            this.e_11.SetResourceReference(Button.StyleProperty, "ButtonStyle");
            // e_12 element
            this.e_12 = new Button();
            this.e_0.Children.Add(this.e_12);
            this.e_12.Name = "e_12";
            this.e_12.Height = 28F;
            this.e_12.Width = 28F;
            this.e_12.HorizontalAlignment = HorizontalAlignment.Right;
            this.e_12.VerticalAlignment = VerticalAlignment.Top;
            this.e_12.Content = "x";
            Binding binding_e_12_Command = new Binding("ExitCommand");
            this.e_12.SetBinding(Button.CommandProperty, binding_e_12_Command);
            this.e_12.SetResourceReference(Button.StyleProperty, "ButtonStyle");
            FontManager.Instance.AddFont("Code 7x5", 18F, FontStyle.Regular, "Code_7x5_13.5_Regular");
        }
        
        private static void InitializeElementResources(UIElement elem) {
            elem.Resources.MergedDictionaries.Add(Dictionary.Instance);
        }
    }
}
