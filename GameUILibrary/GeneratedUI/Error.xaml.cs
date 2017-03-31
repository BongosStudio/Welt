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
    public partial class Error : UIRoot {
        
        private Grid e_0;
        
        private TextBlock e_1;
        
        private Button e_2;
        
        public Error() : 
                base() {
            this.Initialize();
        }
        
        public Error(int width, int height) : 
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
            // e_1 element
            this.e_1 = new TextBlock();
            this.e_0.Children.Add(this.e_1);
            this.e_1.Name = "e_1";
            this.e_1.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_1.VerticalAlignment = VerticalAlignment.Center;
            this.e_1.TextWrapping = TextWrapping.Wrap;
            Binding binding_e_1_Text = new Binding("Result");
            this.e_1.SetBinding(TextBlock.TextProperty, binding_e_1_Text);
            // e_2 element
            this.e_2 = new Button();
            this.e_0.Children.Add(this.e_2);
            this.e_2.Name = "e_2";
            this.e_2.Width = 75F;
            this.e_2.Margin = new Thickness(62F, 0F, 62F, 22F);
            this.e_2.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_2.VerticalAlignment = VerticalAlignment.Bottom;
#warning Style BasedOn is supported only in Dictionary.
            Style e_2_s = new Style(typeof(Button));
            Setter e_2_s_S_0 = new Setter(Button.BackgroundProperty, new ResourceReferenceExpression("BackgroundColor"));
            e_2_s.Setters.Add(e_2_s_S_0);
            Setter e_2_s_S_1 = new Setter(Button.ForegroundProperty, new ResourceReferenceExpression("ForegroundColor"));
            e_2_s.Setters.Add(e_2_s_S_1);
            Func<UIElement, UIElement> e_2_s_S_2_ctFunc = e_2_s_S_2_ctMethod;
            ControlTemplate e_2_s_S_2_ct = new ControlTemplate(typeof(Button), e_2_s_S_2_ctFunc);
            Trigger e_2_s_S_2_ct_T_0 = new Trigger();
            e_2_s_S_2_ct_T_0.Property = Button.IsPressedProperty;
            e_2_s_S_2_ct_T_0.Value = true;
            Setter e_2_s_S_2_ct_T_0_S_0 = new Setter(Button.BackgroundProperty, new ResourceReferenceExpression("ActiveColor"));
            e_2_s_S_2_ct_T_0.Setters.Add(e_2_s_S_2_ct_T_0_S_0);
            Setter e_2_s_S_2_ct_T_0_S_1 = new Setter(Button.ForegroundProperty, new ResourceReferenceExpression("AccentColor"));
            e_2_s_S_2_ct_T_0.Setters.Add(e_2_s_S_2_ct_T_0_S_1);
            e_2_s_S_2_ct.Triggers.Add(e_2_s_S_2_ct_T_0);
            Trigger e_2_s_S_2_ct_T_1 = new Trigger();
            e_2_s_S_2_ct_T_1.Property = Button.IsFocusedProperty;
            e_2_s_S_2_ct_T_1.Value = true;
            Setter e_2_s_S_2_ct_T_1_S_0 = new Setter(Button.BackgroundProperty, new ResourceReferenceExpression("ActiveColor"));
            e_2_s_S_2_ct_T_1.Setters.Add(e_2_s_S_2_ct_T_1_S_0);
            Setter e_2_s_S_2_ct_T_1_S_1 = new Setter(Button.ForegroundProperty, new ResourceReferenceExpression("ForegroundColor"));
            e_2_s_S_2_ct_T_1.Setters.Add(e_2_s_S_2_ct_T_1_S_1);
            e_2_s_S_2_ct.Triggers.Add(e_2_s_S_2_ct_T_1);
            Trigger e_2_s_S_2_ct_T_2 = new Trigger();
            e_2_s_S_2_ct_T_2.Property = Button.IsMouseOverProperty;
            e_2_s_S_2_ct_T_2.Value = true;
            Setter e_2_s_S_2_ct_T_2_S_0 = new Setter(Button.BackgroundProperty, new ResourceReferenceExpression("ActiveColor"));
            e_2_s_S_2_ct_T_2.Setters.Add(e_2_s_S_2_ct_T_2_S_0);
            Setter e_2_s_S_2_ct_T_2_S_1 = new Setter(Button.ForegroundProperty, new ResourceReferenceExpression("ForegroundColor"));
            e_2_s_S_2_ct_T_2.Setters.Add(e_2_s_S_2_ct_T_2_S_1);
            e_2_s_S_2_ct.Triggers.Add(e_2_s_S_2_ct_T_2);
            Trigger e_2_s_S_2_ct_T_3 = new Trigger();
            e_2_s_S_2_ct_T_3.Property = Button.IsEnabledProperty;
            e_2_s_S_2_ct_T_3.Value = false;
            Setter e_2_s_S_2_ct_T_3_S_0 = new Setter(Button.IsHitTestVisibleProperty, false);
            e_2_s_S_2_ct_T_3.Setters.Add(e_2_s_S_2_ct_T_3_S_0);
            Setter e_2_s_S_2_ct_T_3_S_1 = new Setter(Button.BackgroundProperty, new ResourceReferenceExpression("DisabledBackground"));
            e_2_s_S_2_ct_T_3.Setters.Add(e_2_s_S_2_ct_T_3_S_1);
            Setter e_2_s_S_2_ct_T_3_S_2 = new Setter(Button.ForegroundProperty, new ResourceReferenceExpression("DisabledForeground"));
            e_2_s_S_2_ct_T_3.Setters.Add(e_2_s_S_2_ct_T_3_S_2);
            e_2_s_S_2_ct.Triggers.Add(e_2_s_S_2_ct_T_3);
            Trigger e_2_s_S_2_ct_T_4 = new Trigger();
            e_2_s_S_2_ct_T_4.Property = Button.IsEnabledProperty;
            e_2_s_S_2_ct_T_4.Value = true;
            Setter e_2_s_S_2_ct_T_4_S_0 = new Setter(Button.IsHitTestVisibleProperty, true);
            e_2_s_S_2_ct_T_4.Setters.Add(e_2_s_S_2_ct_T_4_S_0);
            e_2_s_S_2_ct.Triggers.Add(e_2_s_S_2_ct_T_4);
            Setter e_2_s_S_2 = new Setter(Button.TemplateProperty, e_2_s_S_2_ct);
            e_2_s.Setters.Add(e_2_s_S_2);
            this.e_2.Style = e_2_s;
            this.e_2.Content = "Return";
            Binding binding_e_2_Command = new Binding("ReturnCommand");
            this.e_2.SetBinding(Button.CommandProperty, binding_e_2_Command);
            FontManager.Instance.AddFont("Segoe UI", 18F, FontStyle.Regular, "Segoe_UI_13.5_Regular");
        }
        
        private static void InitializeElementResources(UIElement elem) {
            elem.Resources.MergedDictionaries.Add(Dictionary.Instance);
        }
        
        private static UIElement e_2_s_S_2_ctMethod(UIElement parent) {
            // e_3 element
            Border e_3 = new Border();
            e_3.Parent = parent;
            e_3.Name = "e_3";
            e_3.SnapsToDevicePixels = true;
            e_3.Padding = new Thickness(5F, 5F, 5F, 5F);
            Binding binding_e_3_Background = new Binding("Background");
            binding_e_3_Background.Source = parent;
            e_3.SetBinding(Border.BackgroundProperty, binding_e_3_Background);
            // e_4 element
            ContentPresenter e_4 = new ContentPresenter();
            e_3.Child = e_4;
            e_4.Name = "e_4";
            e_4.HorizontalAlignment = HorizontalAlignment.Center;
            e_4.VerticalAlignment = VerticalAlignment.Center;
            Binding binding_e_4_Content = new Binding("Content");
            binding_e_4_Content.Source = parent;
            e_4.SetBinding(ContentPresenter.ContentProperty, binding_e_4_Content);
            return e_3;
        }
    }
}
