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
    public partial class Loading : UIRoot {
        
        private Grid e_0;
        
        private TextBlock e_1;
        
        private StackPanel e_2;
        
        private TextBlock e_3;
        
        private TextBlock e_4;
        
        private TextBlock e_5;
        
        public Loading() : 
                base() {
            this.Initialize();
        }
        
        public Loading(int width, int height) : 
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
            // e_0 element
            this.e_0 = new Grid();
            this.Content = this.e_0;
            this.e_0.Name = "e_0";
            // e_1 element
            this.e_1 = new TextBlock();
            this.e_0.Children.Add(this.e_1);
            this.e_1.Name = "e_1";
            this.e_1.Margin = new Thickness(0F, 0F, 10F, 10F);
            this.e_1.HorizontalAlignment = HorizontalAlignment.Right;
            this.e_1.VerticalAlignment = VerticalAlignment.Bottom;
            this.e_1.TextAlignment = TextAlignment.Right;
            Binding binding_e_1_Text = new Binding("LoadingStatusText");
            this.e_1.SetBinding(TextBlock.TextProperty, binding_e_1_Text);
            // e_2 element
            this.e_2 = new StackPanel();
            this.e_0.Children.Add(this.e_2);
            this.e_2.Name = "e_2";
            this.e_2.Height = 130F;
            this.e_2.Margin = new Thickness(0F, 0F, 159F, 0F);
            this.e_2.VerticalAlignment = VerticalAlignment.Bottom;
            // e_3 element
            this.e_3 = new TextBlock();
            this.e_2.Children.Add(this.e_3);
            this.e_3.Name = "e_3";
            Binding binding_e_3_Text = new Binding("UsernameText");
            this.e_3.SetBinding(TextBlock.TextProperty, binding_e_3_Text);
            // e_4 element
            this.e_4 = new TextBlock();
            this.e_2.Children.Add(this.e_4);
            this.e_4.Name = "e_4";
            Binding binding_e_4_Text = new Binding("WorldNameText");
            this.e_4.SetBinding(TextBlock.TextProperty, binding_e_4_Text);
            // e_5 element
            this.e_5 = new TextBlock();
            this.e_2.Children.Add(this.e_5);
            this.e_5.Name = "e_5";
            Binding binding_e_5_Text = new Binding("WorldData");
            this.e_5.SetBinding(TextBlock.TextProperty, binding_e_5_Text);
            FontManager.Instance.AddFont("Code 7x5", 18F, FontStyle.Regular, "Code_7x5_13.5_Regular");
        }
    }
}
