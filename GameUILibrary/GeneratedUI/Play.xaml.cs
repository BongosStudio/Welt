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
    public partial class Play : UIRoot {
        
        private Grid e_0;
        
        private StackPanel e_1;
        
        private Button e_2;
        
        private Button e_3;
        
        private Button e_4;
        
        public Play() : 
                base() {
            this.Initialize();
        }
        
        public Play(int width, int height) : 
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
            // e_1 element
            this.e_1 = new StackPanel();
            this.e_0.Children.Add(this.e_1);
            this.e_1.Name = "e_1";
            this.e_1.HorizontalAlignment = HorizontalAlignment.Left;
            this.e_1.VerticalAlignment = VerticalAlignment.Center;
            Binding binding_e_1_Visibility = new Binding("PauseMenuVisiblity");
            this.e_1.SetBinding(StackPanel.VisibilityProperty, binding_e_1_Visibility);
            // e_2 element
            this.e_2 = new Button();
            this.e_1.Children.Add(this.e_2);
            this.e_2.Name = "e_2";
            this.e_2.HorizontalAlignment = HorizontalAlignment.Left;
            this.e_2.FontFamily = new FontFamily("Code 7x5");
            this.e_2.FontSize = 18F;
            this.e_2.Content = "Resume";
            Binding binding_e_2_Command = new Binding("ResumeButtonCommand");
            this.e_2.SetBinding(Button.CommandProperty, binding_e_2_Command);
            this.e_2.SetResourceReference(Button.StyleProperty, "ButtonAnimStyle");
            // e_3 element
            this.e_3 = new Button();
            this.e_1.Children.Add(this.e_3);
            this.e_3.Name = "e_3";
            this.e_3.HorizontalAlignment = HorizontalAlignment.Left;
            this.e_3.FontFamily = new FontFamily("Code 7x5");
            this.e_3.FontSize = 18F;
            this.e_3.Content = "Options";
            Binding binding_e_3_Command = new Binding("OptionsButtonCommand");
            this.e_3.SetBinding(Button.CommandProperty, binding_e_3_Command);
            this.e_3.SetResourceReference(Button.StyleProperty, "ButtonAnimStyle");
            // e_4 element
            this.e_4 = new Button();
            this.e_1.Children.Add(this.e_4);
            this.e_4.Name = "e_4";
            this.e_4.HorizontalAlignment = HorizontalAlignment.Left;
            this.e_4.FontFamily = new FontFamily("Code 7x5");
            this.e_4.FontSize = 18F;
            this.e_4.Content = "Quit";
            Binding binding_e_4_Command = new Binding("QuitButtonCommand");
            this.e_4.SetBinding(Button.CommandProperty, binding_e_4_Command);
            this.e_4.SetResourceReference(Button.StyleProperty, "ButtonAnimStyle");
            FontManager.Instance.AddFont("Code 7x5", 18F, FontStyle.Regular, "Code_7x5_13.5_Regular");
        }
        
        private static void InitializeElementResources(UIElement elem) {
            elem.Resources.MergedDictionaries.Add(Dictionary.Instance);
        }
    }
}
