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
            // e_0 element
            this.e_0 = new Grid();
            this.Content = this.e_0;
            this.e_0.Name = "e_0";
            // e_1 element
            this.e_1 = new TextBlock();
            this.e_0.Children.Add(this.e_1);
            this.e_1.Name = "e_1";
            this.e_1.Height = 20F;
            this.e_1.Width = 112F;
            this.e_1.Margin = new Thickness(0F, 0F, 10F, 10F);
            this.e_1.HorizontalAlignment = HorizontalAlignment.Right;
            this.e_1.VerticalAlignment = VerticalAlignment.Bottom;
            this.e_1.TextAlignment = TextAlignment.Right;
            this.e_1.TextWrapping = TextWrapping.Wrap;
            this.e_1.FontFamily = new FontFamily("Code 7x5");
            this.e_1.FontSize = 18F;
            Binding binding_e_1_Text = new Binding("LoadingStatusText");
            this.e_1.SetBinding(TextBlock.TextProperty, binding_e_1_Text);
            FontManager.Instance.AddFont("Code 7x5", 18F, FontStyle.Regular, "Code_7x5_13.5_Regular");
        }
    }
}
