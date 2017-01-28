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
    public partial class PlayUI : UIRoot {
        
        private Grid e_0;
        
        private Grid Hotbar;
        
        private TextBlock e_1;
        
        private TextBlock e_2;
        
        private TextBox e_3;
        
        public PlayUI() : 
                base() {
            this.Initialize();
        }
        
        public PlayUI(int width, int height) : 
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
            this.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            InitializeElementResources(this);
            // e_0 element
            this.e_0 = new Grid();
            this.Content = this.e_0;
            this.e_0.Name = "e_0";
            // Hotbar element
            this.Hotbar = new Grid();
            this.e_0.Children.Add(this.Hotbar);
            this.Hotbar.Name = "Hotbar";
            this.Hotbar.Height = 50F;
            this.Hotbar.VerticalAlignment = VerticalAlignment.Bottom;
            // e_1 element
            this.e_1 = new TextBlock();
            this.e_0.Children.Add(this.e_1);
            this.e_1.Name = "e_1";
            this.e_1.Margin = new Thickness(0F, 0F, 0F, 50F);
            this.e_1.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_1.VerticalAlignment = VerticalAlignment.Bottom;
            this.e_1.Foreground = new SolidColorBrush(new ColorW(255, 255, 255, 255));
            this.e_1.TextWrapping = TextWrapping.Wrap;
            Binding binding_e_1_Text = new Binding("SelectedItemName");
            this.e_1.SetBinding(TextBlock.TextProperty, binding_e_1_Text);
            // e_2 element
            this.e_2 = new TextBlock();
            this.e_0.Children.Add(this.e_2);
            this.e_2.Name = "e_2";
            this.e_2.Height = 66F;
            this.e_2.Margin = new Thickness(0F, 0F, 150F, 87F);
            this.e_2.VerticalAlignment = VerticalAlignment.Bottom;
            this.e_2.TextWrapping = TextWrapping.Wrap;
            Binding binding_e_2_Text = new Binding("ChatBoxText");
            this.e_2.SetBinding(TextBlock.TextProperty, binding_e_2_Text);
            // e_3 element
            this.e_3 = new TextBox();
            this.e_0.Children.Add(this.e_3);
            this.e_3.Name = "e_3";
            this.e_3.Height = 37F;
            this.e_3.Visibility = Visibility.Collapsed;
            this.e_3.Margin = new Thickness(0F, 0F, 150F, 50F);
            this.e_3.VerticalAlignment = VerticalAlignment.Bottom;
            Binding binding_e_3_IsEnabled = new Binding("IsChatBoxSelected");
            this.e_3.SetBinding(TextBox.IsEnabledProperty, binding_e_3_IsEnabled);
            Binding binding_e_3_Text = new Binding("ChatBoxInput");
            this.e_3.SetBinding(TextBox.TextProperty, binding_e_3_Text);
            FontManager.Instance.AddFont("Code 7x5", 18F, FontStyle.Regular, "Code_7x5_13.5_Regular");
        }
        
        private static void InitializeElementResources(UIElement elem) {
            elem.Resources.MergedDictionaries.Add(Dictionary.Instance);
        }
    }
}
