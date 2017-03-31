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
    public partial class Loading : UIRoot {
        
        private Grid e_0;
        
        private TextBlock e_1;
        
        private StackPanel e_2;
        
        private TextBlock e_3;
        
        private TextBlock e_4;
        
        private TextBlock e_5;
        
        private ProgressBar e_6;
        
        private TextBlock e_7;
        
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
            this.FontSize = 18F;
            InitializeElementResources(this);
            // e_0 element
            this.e_0 = new Grid();
            this.Content = this.e_0;
            this.e_0.Name = "e_0";
            Binding binding_e_0_Opacity = new Binding("Opacity");
            this.e_0.SetBinding(Grid.OpacityProperty, binding_e_0_Opacity);
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
            this.e_2.Margin = new Thickness(10F, 0F, 159F, 0F);
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
            // e_6 element
            this.e_6 = new ProgressBar();
            this.e_0.Children.Add(this.e_6);
            this.e_6.Name = "e_6";
            this.e_6.Height = 10F;
            this.e_6.Width = 100F;
            this.e_6.Margin = new Thickness(0F, 0F, 10F, 40F);
            this.e_6.HorizontalAlignment = HorizontalAlignment.Right;
            this.e_6.VerticalAlignment = VerticalAlignment.Bottom;
            this.e_6.Maximum = 4F;
            Binding binding_e_6_Value = new Binding("LoadingStatus");
            this.e_6.SetBinding(ProgressBar.ValueProperty, binding_e_6_Value);
            // e_7 element
            this.e_7 = new TextBlock();
            this.e_0.Children.Add(this.e_7);
            this.e_7.Name = "e_7";
            this.e_7.Margin = new Thickness(20F, 20F, 0F, 0F);
            this.e_7.HorizontalAlignment = HorizontalAlignment.Left;
            this.e_7.VerticalAlignment = VerticalAlignment.Top;
            this.e_7.TextWrapping = TextWrapping.Wrap;
            Binding binding_e_7_Text = new Binding("HintText");
            this.e_7.SetBinding(TextBlock.TextProperty, binding_e_7_Text);
            FontManager.Instance.AddFont("Segoe UI", 18F, FontStyle.Regular, "Segoe_UI_13.5_Regular");
        }
        
        private static void InitializeElementResources(UIElement elem) {
            elem.Resources.MergedDictionaries.Add(Dictionary.Instance);
        }
    }
}
