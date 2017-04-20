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
    
    
    [GeneratedCodeAttribute("Empty Keys UI Generator", "3.0.0.0")]
    public sealed class Dictionary : ResourceDictionary {
        
        private static Dictionary singleton = new Dictionary();
        
        public Dictionary() {
            this.InitializeResources();
        }
        
        public static Dictionary Instance {
            get {
                return singleton;
            }
        }
        
        private void InitializeResources() {
            // Resource - [AccentColor] SolidColorBrush
            this.Add("AccentColor", new SolidColorBrush(new ColorW(54, 85, 143, 255)));
            // Resource - [ActiveColor] SolidColorBrush
            this.Add("ActiveColor", new SolidColorBrush(new ColorW(100, 89, 134, 255)));
            // Resource - [BackgroundColor] SolidColorBrush
            this.Add("BackgroundColor", new SolidColorBrush(new ColorW(42, 45, 52, 255)));
            // Resource - [ButtonStyle] Style
            var r_3_s_bo = this[typeof(Button)];
            Style r_3_s = new Style(typeof(Button), r_3_s_bo as Style);
            Setter r_3_s_S_0 = new Setter(Button.BackgroundProperty, new ResourceReferenceExpression("BackgroundColor"));
            r_3_s.Setters.Add(r_3_s_S_0);
            Setter r_3_s_S_1 = new Setter(Button.ForegroundProperty, new ResourceReferenceExpression("ForegroundColor"));
            r_3_s.Setters.Add(r_3_s_S_1);
            Func<UIElement, UIElement> r_3_s_S_2_ctFunc = r_3_s_S_2_ctMethod;
            ControlTemplate r_3_s_S_2_ct = new ControlTemplate(typeof(Button), r_3_s_S_2_ctFunc);
            Trigger r_3_s_S_2_ct_T_0 = new Trigger();
            r_3_s_S_2_ct_T_0.Property = Button.IsPressedProperty;
            r_3_s_S_2_ct_T_0.Value = true;
            Setter r_3_s_S_2_ct_T_0_S_0 = new Setter(Button.BackgroundProperty, new ResourceReferenceExpression("ActiveColor"));
            r_3_s_S_2_ct_T_0.Setters.Add(r_3_s_S_2_ct_T_0_S_0);
            Setter r_3_s_S_2_ct_T_0_S_1 = new Setter(Button.ForegroundProperty, new ResourceReferenceExpression("AccentColor"));
            r_3_s_S_2_ct_T_0.Setters.Add(r_3_s_S_2_ct_T_0_S_1);
            r_3_s_S_2_ct.Triggers.Add(r_3_s_S_2_ct_T_0);
            Trigger r_3_s_S_2_ct_T_1 = new Trigger();
            r_3_s_S_2_ct_T_1.Property = Button.IsFocusedProperty;
            r_3_s_S_2_ct_T_1.Value = true;
            Setter r_3_s_S_2_ct_T_1_S_0 = new Setter(Button.BackgroundProperty, new ResourceReferenceExpression("ActiveColor"));
            r_3_s_S_2_ct_T_1.Setters.Add(r_3_s_S_2_ct_T_1_S_0);
            Setter r_3_s_S_2_ct_T_1_S_1 = new Setter(Button.ForegroundProperty, new ResourceReferenceExpression("ForegroundColor"));
            r_3_s_S_2_ct_T_1.Setters.Add(r_3_s_S_2_ct_T_1_S_1);
            r_3_s_S_2_ct.Triggers.Add(r_3_s_S_2_ct_T_1);
            Trigger r_3_s_S_2_ct_T_2 = new Trigger();
            r_3_s_S_2_ct_T_2.Property = Button.IsMouseOverProperty;
            r_3_s_S_2_ct_T_2.Value = true;
            Setter r_3_s_S_2_ct_T_2_S_0 = new Setter(Button.BackgroundProperty, new ResourceReferenceExpression("ActiveColor"));
            r_3_s_S_2_ct_T_2.Setters.Add(r_3_s_S_2_ct_T_2_S_0);
            Setter r_3_s_S_2_ct_T_2_S_1 = new Setter(Button.ForegroundProperty, new ResourceReferenceExpression("ForegroundColor"));
            r_3_s_S_2_ct_T_2.Setters.Add(r_3_s_S_2_ct_T_2_S_1);
            r_3_s_S_2_ct.Triggers.Add(r_3_s_S_2_ct_T_2);
            Trigger r_3_s_S_2_ct_T_3 = new Trigger();
            r_3_s_S_2_ct_T_3.Property = Button.IsEnabledProperty;
            r_3_s_S_2_ct_T_3.Value = false;
            Setter r_3_s_S_2_ct_T_3_S_0 = new Setter(Button.IsHitTestVisibleProperty, false);
            r_3_s_S_2_ct_T_3.Setters.Add(r_3_s_S_2_ct_T_3_S_0);
            Setter r_3_s_S_2_ct_T_3_S_1 = new Setter(Button.BackgroundProperty, new ResourceReferenceExpression("DisabledBackground"));
            r_3_s_S_2_ct_T_3.Setters.Add(r_3_s_S_2_ct_T_3_S_1);
            Setter r_3_s_S_2_ct_T_3_S_2 = new Setter(Button.ForegroundProperty, new ResourceReferenceExpression("DisabledForeground"));
            r_3_s_S_2_ct_T_3.Setters.Add(r_3_s_S_2_ct_T_3_S_2);
            r_3_s_S_2_ct.Triggers.Add(r_3_s_S_2_ct_T_3);
            Trigger r_3_s_S_2_ct_T_4 = new Trigger();
            r_3_s_S_2_ct_T_4.Property = Button.IsEnabledProperty;
            r_3_s_S_2_ct_T_4.Value = true;
            Setter r_3_s_S_2_ct_T_4_S_0 = new Setter(Button.IsHitTestVisibleProperty, true);
            r_3_s_S_2_ct_T_4.Setters.Add(r_3_s_S_2_ct_T_4_S_0);
            r_3_s_S_2_ct.Triggers.Add(r_3_s_S_2_ct_T_4);
            Setter r_3_s_S_2 = new Setter(Button.TemplateProperty, r_3_s_S_2_ct);
            r_3_s.Setters.Add(r_3_s_S_2);
            this.Add("ButtonStyle", r_3_s);
            // Resource - [ButtonTextColor] SolidColorBrush
            this.Add("ButtonTextColor", new SolidColorBrush(new ColorW(255, 255, 255, 255)));
            // Resource - [ButtonTextFocusedColor] SolidColorBrush
            this.Add("ButtonTextFocusedColor", new SolidColorBrush(new ColorW(128, 128, 128, 255)));
            // Resource - [ButtonTextHoverColor] SolidColorBrush
            this.Add("ButtonTextHoverColor", new SolidColorBrush(new ColorW(128, 128, 128, 255)));
            // Resource - [ButtonTextPressedColor] SolidColorBrush
            this.Add("ButtonTextPressedColor", new SolidColorBrush(new ColorW(255, 255, 255, 255)));
            // Resource - [DisabledBackground] SolidColorBrush
            this.Add("DisabledBackground", new SolidColorBrush(new ColorW(61, 64, 70, 255)));
            // Resource - [DisabledForeground] SolidColorBrush
            this.Add("DisabledForeground", new SolidColorBrush(new ColorW(128, 128, 128, 255)));
            // Resource - [ForegroundColor] SolidColorBrush
            this.Add("ForegroundColor", new SolidColorBrush(new ColorW(245, 245, 245, 255)));
            // Resource - [HighlightColor] SolidColorBrush
            this.Add("HighlightColor", new SolidColorBrush(new ColorW(128, 26, 134, 255)));
            // Resource - [HorizontalSlider] ControlTemplate
            Func<UIElement, UIElement> r_12_ctFunc = r_12_ctMethod;
            ControlTemplate r_12_ct = new ControlTemplate(typeof(Slider), r_12_ctFunc);
            this.Add("HorizontalSlider", r_12_ct);
            // Resource - [LowlightColor] SolidColorBrush
            this.Add("LowlightColor", new SolidColorBrush(new ColorW(113, 116, 120, 255)));
            // Resource - [SliderButtonStyle] Style
            Style r_14_s = new Style(typeof(RepeatButton));
            Setter r_14_s_S_0 = new Setter(RepeatButton.SnapsToDevicePixelsProperty, true);
            r_14_s.Setters.Add(r_14_s_S_0);
            Setter r_14_s_S_1 = new Setter(RepeatButton.FocusableProperty, false);
            r_14_s.Setters.Add(r_14_s_S_1);
            Func<UIElement, UIElement> r_14_s_S_2_ctFunc = r_14_s_S_2_ctMethod;
            ControlTemplate r_14_s_S_2_ct = new ControlTemplate(typeof(RepeatButton), r_14_s_S_2_ctFunc);
            Setter r_14_s_S_2 = new Setter(RepeatButton.TemplateProperty, r_14_s_S_2_ct);
            r_14_s.Setters.Add(r_14_s_S_2);
            this.Add("SliderButtonStyle", r_14_s);
            // Resource - [SliderThumbStyle] Style
            Style r_15_s = new Style(typeof(Thumb));
            Setter r_15_s_S_0 = new Setter(Thumb.SnapsToDevicePixelsProperty, true);
            r_15_s.Setters.Add(r_15_s_S_0);
            Func<UIElement, UIElement> r_15_s_S_1_ctFunc = r_15_s_S_1_ctMethod;
            ControlTemplate r_15_s_S_1_ct = new ControlTemplate(typeof(Thumb), r_15_s_S_1_ctFunc);
            Trigger r_15_s_S_1_ct_T_0 = new Trigger();
            r_15_s_S_1_ct_T_0.Property = Thumb.IsMouseOverProperty;
            r_15_s_S_1_ct_T_0.Value = true;
            Setter r_15_s_S_1_ct_T_0_S_0 = new Setter(Panel.BackgroundProperty, new ResourceReferenceExpression("AccentColor"));
            r_15_s_S_1_ct_T_0_S_0.TargetName = "PART_SliderThumb";
            r_15_s_S_1_ct_T_0.Setters.Add(r_15_s_S_1_ct_T_0_S_0);
            r_15_s_S_1_ct.Triggers.Add(r_15_s_S_1_ct_T_0);
            Setter r_15_s_S_1 = new Setter(Thumb.TemplateProperty, r_15_s_S_1_ct);
            r_15_s.Setters.Add(r_15_s_S_1);
            this.Add("SliderThumbStyle", r_15_s);
            // Resource - [SuccessColor] SolidColorBrush
            this.Add("SuccessColor", new SolidColorBrush(new ColorW(143, 227, 136, 255)));
            // Resource - [System.Windows.Controls.CheckBox] Style
            var r_17_s_bo = this[typeof(CheckBox)];
            Style r_17_s = new Style(typeof(CheckBox), r_17_s_bo as Style);
            Setter r_17_s_S_0 = new Setter(CheckBox.BackgroundProperty, new ResourceReferenceExpression("BackgroundColor"));
            r_17_s.Setters.Add(r_17_s_S_0);
            Setter r_17_s_S_1 = new Setter(CheckBox.ForegroundProperty, new ResourceReferenceExpression("ForegroundColor"));
            r_17_s.Setters.Add(r_17_s_S_1);
            Func<UIElement, UIElement> r_17_s_S_2_ctFunc = r_17_s_S_2_ctMethod;
            ControlTemplate r_17_s_S_2_ct = new ControlTemplate(typeof(CheckBox), r_17_s_S_2_ctFunc);
            Trigger r_17_s_S_2_ct_T_0 = new Trigger();
            r_17_s_S_2_ct_T_0.Property = CheckBox.IsCheckedProperty;
            r_17_s_S_2_ct_T_0.Value = true;
            Setter r_17_s_S_2_ct_T_0_S_0 = new Setter(UIElement.VisibilityProperty, Visibility.Visible);
            r_17_s_S_2_ct_T_0_S_0.TargetName = "PART_CheckMark";
            r_17_s_S_2_ct_T_0.Setters.Add(r_17_s_S_2_ct_T_0_S_0);
            r_17_s_S_2_ct.Triggers.Add(r_17_s_S_2_ct_T_0);
            Trigger r_17_s_S_2_ct_T_1 = new Trigger();
            r_17_s_S_2_ct_T_1.Property = CheckBox.IsCheckedProperty;
            r_17_s_S_2_ct_T_1.Value = false;
            Setter r_17_s_S_2_ct_T_1_S_0 = new Setter(UIElement.VisibilityProperty, Visibility.Collapsed);
            r_17_s_S_2_ct_T_1_S_0.TargetName = "PART_CheckMark";
            r_17_s_S_2_ct_T_1.Setters.Add(r_17_s_S_2_ct_T_1_S_0);
            r_17_s_S_2_ct.Triggers.Add(r_17_s_S_2_ct_T_1);
            Trigger r_17_s_S_2_ct_T_2 = new Trigger();
            r_17_s_S_2_ct_T_2.Property = CheckBox.IsMouseOverProperty;
            r_17_s_S_2_ct_T_2.Value = true;
            Setter r_17_s_S_2_ct_T_2_S_0 = new Setter(Panel.BackgroundProperty, new ResourceReferenceExpression("ActiveColor"));
            r_17_s_S_2_ct_T_2_S_0.TargetName = "PART_NotChecked";
            r_17_s_S_2_ct_T_2.Setters.Add(r_17_s_S_2_ct_T_2_S_0);
            r_17_s_S_2_ct.Triggers.Add(r_17_s_S_2_ct_T_2);
            Setter r_17_s_S_2 = new Setter(CheckBox.TemplateProperty, r_17_s_S_2_ct);
            r_17_s.Setters.Add(r_17_s_S_2);
            this.Add(typeof(CheckBox), r_17_s);
            // Resource - [System.Windows.Controls.ComboBox] Style
            Style r_18_s = new Style(typeof(ComboBox));
            Setter r_18_s_S_0 = new Setter(ComboBox.ForegroundProperty, new ResourceReferenceExpression("ForegroundColor"));
            r_18_s.Setters.Add(r_18_s_S_0);
            Setter r_18_s_S_1 = new Setter(ComboBox.HeightProperty, 32F);
            r_18_s.Setters.Add(r_18_s_S_1);
            Setter r_18_s_S_2 = new Setter(ComboBox.MaxDropDownHeightProperty, 150F);
            r_18_s.Setters.Add(r_18_s_S_2);
            Func<UIElement, UIElement> r_18_s_S_3_ctFunc = r_18_s_S_3_ctMethod;
            ControlTemplate r_18_s_S_3_ct = new ControlTemplate(typeof(ComboBox), r_18_s_S_3_ctFunc);
            Trigger r_18_s_S_3_ct_T_0 = new Trigger();
            r_18_s_S_3_ct_T_0.Property = ComboBox.IsMouseOverProperty;
            r_18_s_S_3_ct_T_0.Value = true;
            Setter r_18_s_S_3_ct_T_0_S_0 = new Setter(Image.SourceProperty, new ResourceReferenceExpression("ActiveColor"));
            r_18_s_S_3_ct_T_0_S_0.TargetName = "PART_ComboBoxLeft";
            r_18_s_S_3_ct_T_0.Setters.Add(r_18_s_S_3_ct_T_0_S_0);
            Setter r_18_s_S_3_ct_T_0_S_1 = new Setter(Image.SourceProperty, new ResourceReferenceExpression("ActiveColor"));
            r_18_s_S_3_ct_T_0_S_1.TargetName = "PART_ComboBoxCenter";
            r_18_s_S_3_ct_T_0.Setters.Add(r_18_s_S_3_ct_T_0_S_1);
            Setter r_18_s_S_3_ct_T_0_S_2 = new Setter(Image.SourceProperty, new ResourceReferenceExpression("ActiveColor"));
            r_18_s_S_3_ct_T_0_S_2.TargetName = "PART_ComboBoxRight";
            r_18_s_S_3_ct_T_0.Setters.Add(r_18_s_S_3_ct_T_0_S_2);
            r_18_s_S_3_ct.Triggers.Add(r_18_s_S_3_ct_T_0);
            Setter r_18_s_S_3 = new Setter(ComboBox.TemplateProperty, r_18_s_S_3_ct);
            r_18_s.Setters.Add(r_18_s_S_3);
            this.Add(typeof(ComboBox), r_18_s);
            // Resource - [System.Windows.Controls.ComboBoxItem] Style
            Style r_19_s = new Style(typeof(ComboBoxItem));
            Func<UIElement, UIElement> r_19_s_S_0_ctFunc = r_19_s_S_0_ctMethod;
            ControlTemplate r_19_s_S_0_ct = new ControlTemplate(typeof(ComboBoxItem), r_19_s_S_0_ctFunc);
            Trigger r_19_s_S_0_ct_T_0 = new Trigger();
            r_19_s_S_0_ct_T_0.Property = ComboBoxItem.IsHighlightedProperty;
            r_19_s_S_0_ct_T_0.Value = true;
            Setter r_19_s_S_0_ct_T_0_S_0 = new Setter(ComboBoxItem.ForegroundProperty, new SolidColorBrush(new ColorW(128, 128, 128, 255)));
            r_19_s_S_0_ct_T_0.Setters.Add(r_19_s_S_0_ct_T_0_S_0);
            r_19_s_S_0_ct.Triggers.Add(r_19_s_S_0_ct_T_0);
            Trigger r_19_s_S_0_ct_T_1 = new Trigger();
            r_19_s_S_0_ct_T_1.Property = ComboBoxItem.IsMouseOverProperty;
            r_19_s_S_0_ct_T_1.Value = true;
            Setter r_19_s_S_0_ct_T_1_S_0 = new Setter(ComboBoxItem.ForegroundProperty, new SolidColorBrush(new ColorW(128, 128, 128, 255)));
            r_19_s_S_0_ct_T_1.Setters.Add(r_19_s_S_0_ct_T_1_S_0);
            r_19_s_S_0_ct.Triggers.Add(r_19_s_S_0_ct_T_1);
            Setter r_19_s_S_0 = new Setter(ComboBoxItem.TemplateProperty, r_19_s_S_0_ct);
            r_19_s.Setters.Add(r_19_s_S_0);
            this.Add(typeof(ComboBoxItem), r_19_s);
            // Resource - [System.Windows.Controls.ListBoxItem] Style
            Style r_20_s = new Style(typeof(ListBoxItem));
            Setter r_20_s_S_0 = new Setter(ListBoxItem.BackgroundProperty, new ResourceReferenceExpression("BackgroundColor"));
            r_20_s.Setters.Add(r_20_s_S_0);
            Setter r_20_s_S_1 = new Setter(ListBoxItem.ForegroundProperty, new ResourceReferenceExpression("ForegroundColor"));
            r_20_s.Setters.Add(r_20_s_S_1);
            Func<UIElement, UIElement> r_20_s_S_2_ctFunc = r_20_s_S_2_ctMethod;
            ControlTemplate r_20_s_S_2_ct = new ControlTemplate(typeof(ListBoxItem), r_20_s_S_2_ctFunc);
            Trigger r_20_s_S_2_ct_T_0 = new Trigger();
            r_20_s_S_2_ct_T_0.Property = ListBoxItem.IsFocusedProperty;
            r_20_s_S_2_ct_T_0.Value = true;
            Setter r_20_s_S_2_ct_T_0_S_0 = new Setter(ListBoxItem.BackgroundProperty, new ResourceReferenceExpression("ActiveColor"));
            r_20_s_S_2_ct_T_0.Setters.Add(r_20_s_S_2_ct_T_0_S_0);
            r_20_s_S_2_ct.Triggers.Add(r_20_s_S_2_ct_T_0);
            Trigger r_20_s_S_2_ct_T_1 = new Trigger();
            r_20_s_S_2_ct_T_1.Property = ListBoxItem.IsMouseOverProperty;
            r_20_s_S_2_ct_T_1.Value = true;
            Setter r_20_s_S_2_ct_T_1_S_0 = new Setter(ListBoxItem.BackgroundProperty, new ResourceReferenceExpression("AccentColor"));
            r_20_s_S_2_ct_T_1.Setters.Add(r_20_s_S_2_ct_T_1_S_0);
            r_20_s_S_2_ct.Triggers.Add(r_20_s_S_2_ct_T_1);
            Setter r_20_s_S_2 = new Setter(ListBoxItem.TemplateProperty, r_20_s_S_2_ct);
            r_20_s.Setters.Add(r_20_s_S_2);
            this.Add(typeof(ListBoxItem), r_20_s);
            // Resource - [System.Windows.Controls.Slider] Style
            Style r_21_s = new Style(typeof(Slider));
            Setter r_21_s_S_0 = new Setter(Slider.SnapsToDevicePixelsProperty, true);
            r_21_s.Setters.Add(r_21_s_S_0);
            Setter r_21_s_S_1 = new Setter(Slider.TemplateProperty, new ResourceReferenceExpression("HorizontalSlider"));
            r_21_s.Setters.Add(r_21_s_S_1);
            this.Add(typeof(Slider), r_21_s);
            // Resource - [System.Windows.Controls.TabItem] Style
            Style r_22_s = new Style(typeof(TabItem));
            Setter r_22_s_S_0 = new Setter(TabItem.BackgroundProperty, new ResourceReferenceExpression("BackgroundColor"));
            r_22_s.Setters.Add(r_22_s_S_0);
            Setter r_22_s_S_1 = new Setter(TabItem.ForegroundProperty, new ResourceReferenceExpression("ForegroundColor"));
            r_22_s.Setters.Add(r_22_s_S_1);
            Func<UIElement, UIElement> r_22_s_S_2_ctFunc = r_22_s_S_2_ctMethod;
            ControlTemplate r_22_s_S_2_ct = new ControlTemplate(typeof(TabItem), r_22_s_S_2_ctFunc);
            Trigger r_22_s_S_2_ct_T_0 = new Trigger();
            r_22_s_S_2_ct_T_0.Property = TabItem.IsSelectedProperty;
            r_22_s_S_2_ct_T_0.Value = true;
            Setter r_22_s_S_2_ct_T_0_S_0 = new Setter(TabItem.BackgroundProperty, new ResourceReferenceExpression("ActiveColor"));
            r_22_s_S_2_ct_T_0.Setters.Add(r_22_s_S_2_ct_T_0_S_0);
            r_22_s_S_2_ct.Triggers.Add(r_22_s_S_2_ct_T_0);
            Trigger r_22_s_S_2_ct_T_1 = new Trigger();
            r_22_s_S_2_ct_T_1.Property = TabItem.IsFocusedProperty;
            r_22_s_S_2_ct_T_1.Value = true;
            Setter r_22_s_S_2_ct_T_1_S_0 = new Setter(TabItem.BackgroundProperty, new ResourceReferenceExpression("ActiveColor"));
            r_22_s_S_2_ct_T_1.Setters.Add(r_22_s_S_2_ct_T_1_S_0);
            r_22_s_S_2_ct.Triggers.Add(r_22_s_S_2_ct_T_1);
            Trigger r_22_s_S_2_ct_T_2 = new Trigger();
            r_22_s_S_2_ct_T_2.Property = TabItem.IsMouseOverProperty;
            r_22_s_S_2_ct_T_2.Value = true;
            Setter r_22_s_S_2_ct_T_2_S_0 = new Setter(TabItem.BackgroundProperty, new ResourceReferenceExpression("AccentColor"));
            r_22_s_S_2_ct_T_2.Setters.Add(r_22_s_S_2_ct_T_2_S_0);
            r_22_s_S_2_ct.Triggers.Add(r_22_s_S_2_ct_T_2);
            Setter r_22_s_S_2 = new Setter(TabItem.TemplateProperty, r_22_s_S_2_ct);
            r_22_s.Setters.Add(r_22_s_S_2);
            this.Add(typeof(TabItem), r_22_s);
            // Resource - [System.Windows.Controls.TextBox] Style
            var r_23_s_bo = this[typeof(TextBox)];
            Style r_23_s = new Style(typeof(TextBox), r_23_s_bo as Style);
            Setter r_23_s_S_0 = new Setter(TextBox.BackgroundProperty, new ResourceReferenceExpression("BackgroundColor"));
            r_23_s.Setters.Add(r_23_s_S_0);
            Setter r_23_s_S_1 = new Setter(TextBox.ForegroundProperty, new ResourceReferenceExpression("ForegroundColor"));
            r_23_s.Setters.Add(r_23_s_S_1);
            Setter r_23_s_S_2 = new Setter(TextBox.SnapsToDevicePixelsProperty, true);
            r_23_s.Setters.Add(r_23_s_S_2);
            Setter r_23_s_S_3 = new Setter(TextBox.BorderThicknessProperty, new Thickness(0F));
            r_23_s.Setters.Add(r_23_s_S_3);
            Setter r_23_s_S_4 = new Setter(TextBox.CaretBrushProperty, new SolidColorBrush(new ColorW(255, 255, 255, 255)));
            r_23_s.Setters.Add(r_23_s_S_4);
            Setter r_23_s_S_5 = new Setter(TextBox.SnapsToDevicePixelsProperty, true);
            r_23_s.Setters.Add(r_23_s_S_5);
            Func<UIElement, UIElement> r_23_s_S_6_ctFunc = r_23_s_S_6_ctMethod;
            ControlTemplate r_23_s_S_6_ct = new ControlTemplate(typeof(TextBox), r_23_s_S_6_ctFunc);
            Trigger r_23_s_S_6_ct_T_0 = new Trigger();
            r_23_s_S_6_ct_T_0.Property = TextBox.IsFocusedProperty;
            r_23_s_S_6_ct_T_0.Value = true;
            Setter r_23_s_S_6_ct_T_0_S_0 = new Setter(TextBox.BackgroundProperty, new ResourceReferenceExpression("ActiveColor"));
            r_23_s_S_6_ct_T_0.Setters.Add(r_23_s_S_6_ct_T_0_S_0);
            Setter r_23_s_S_6_ct_T_0_S_1 = new Setter(TextBox.ForegroundProperty, new ResourceReferenceExpression("ForegroundColor"));
            r_23_s_S_6_ct_T_0.Setters.Add(r_23_s_S_6_ct_T_0_S_1);
            r_23_s_S_6_ct.Triggers.Add(r_23_s_S_6_ct_T_0);
            Trigger r_23_s_S_6_ct_T_1 = new Trigger();
            r_23_s_S_6_ct_T_1.Property = TextBox.IsMouseOverProperty;
            r_23_s_S_6_ct_T_1.Value = true;
            Setter r_23_s_S_6_ct_T_1_S_0 = new Setter(TextBox.BackgroundProperty, new ResourceReferenceExpression("ActiveColor"));
            r_23_s_S_6_ct_T_1.Setters.Add(r_23_s_S_6_ct_T_1_S_0);
            Setter r_23_s_S_6_ct_T_1_S_1 = new Setter(TextBox.ForegroundProperty, new ResourceReferenceExpression("ForegroundColor"));
            r_23_s_S_6_ct_T_1.Setters.Add(r_23_s_S_6_ct_T_1_S_1);
            r_23_s_S_6_ct.Triggers.Add(r_23_s_S_6_ct_T_1);
            Trigger r_23_s_S_6_ct_T_2 = new Trigger();
            r_23_s_S_6_ct_T_2.Property = TextBox.IsEnabledProperty;
            r_23_s_S_6_ct_T_2.Value = false;
            Setter r_23_s_S_6_ct_T_2_S_0 = new Setter(TextBox.IsHitTestVisibleProperty, false);
            r_23_s_S_6_ct_T_2.Setters.Add(r_23_s_S_6_ct_T_2_S_0);
            Setter r_23_s_S_6_ct_T_2_S_1 = new Setter(TextBox.BackgroundProperty, new ResourceReferenceExpression("DisabledBackground"));
            r_23_s_S_6_ct_T_2.Setters.Add(r_23_s_S_6_ct_T_2_S_1);
            Setter r_23_s_S_6_ct_T_2_S_2 = new Setter(TextBox.ForegroundProperty, new ResourceReferenceExpression("DisabledForeground"));
            r_23_s_S_6_ct_T_2.Setters.Add(r_23_s_S_6_ct_T_2_S_2);
            r_23_s_S_6_ct.Triggers.Add(r_23_s_S_6_ct_T_2);
            Trigger r_23_s_S_6_ct_T_3 = new Trigger();
            r_23_s_S_6_ct_T_3.Property = TextBox.IsEnabledProperty;
            r_23_s_S_6_ct_T_3.Value = true;
            Setter r_23_s_S_6_ct_T_3_S_0 = new Setter(TextBox.IsHitTestVisibleProperty, true);
            r_23_s_S_6_ct_T_3.Setters.Add(r_23_s_S_6_ct_T_3_S_0);
            r_23_s_S_6_ct.Triggers.Add(r_23_s_S_6_ct_T_3);
            Setter r_23_s_S_6 = new Setter(TextBox.TemplateProperty, r_23_s_S_6_ct);
            r_23_s.Setters.Add(r_23_s_S_6);
            this.Add(typeof(TextBox), r_23_s);
            // Resource - [TextBoxScrollViewer] Style
            Style r_24_s = new Style(typeof(ScrollViewer));
            Setter r_24_s_S_0 = new Setter(ScrollViewer.SnapsToDevicePixelsProperty, true);
            r_24_s.Setters.Add(r_24_s_S_0);
            Func<UIElement, UIElement> r_24_s_S_1_ctFunc = r_24_s_S_1_ctMethod;
            ControlTemplate r_24_s_S_1_ct = new ControlTemplate(typeof(ScrollViewer), r_24_s_S_1_ctFunc);
            Setter r_24_s_S_1 = new Setter(ScrollViewer.TemplateProperty, r_24_s_S_1_ct);
            r_24_s.Setters.Add(r_24_s_S_1);
            this.Add("TextBoxScrollViewer", r_24_s);
            // Resource - [UiStyle] Style
            Style r_25_s = new Style(typeof(UIRoot));
            Setter r_25_s_S_0 = new Setter(UIRoot.ForegroundProperty, new ResourceReferenceExpression("ForegroundColor"));
            r_25_s.Setters.Add(r_25_s_S_0);
            Setter r_25_s_S_1 = new Setter(UIRoot.FontFamilyProperty, new FontFamily("Code 7x5"));
            r_25_s.Setters.Add(r_25_s_S_1);
            Setter r_25_s_S_2 = new Setter(UIRoot.FontSizeProperty, 18F);
            r_25_s.Setters.Add(r_25_s_S_2);
            this.Add("UiStyle", r_25_s);
        }
        
        private static UIElement r_3_s_S_2_ctMethod(UIElement parent) {
            // e_0 element
            Border e_0 = new Border();
            e_0.Parent = parent;
            e_0.Name = "e_0";
            e_0.SnapsToDevicePixels = true;
            e_0.Padding = new Thickness(5F, 5F, 5F, 5F);
            Binding binding_e_0_Background = new Binding("Background");
            binding_e_0_Background.Source = parent;
            e_0.SetBinding(Border.BackgroundProperty, binding_e_0_Background);
            // e_1 element
            ContentPresenter e_1 = new ContentPresenter();
            e_0.Child = e_1;
            e_1.Name = "e_1";
            e_1.HorizontalAlignment = HorizontalAlignment.Center;
            e_1.VerticalAlignment = VerticalAlignment.Center;
            Binding binding_e_1_Content = new Binding("Content");
            binding_e_1_Content.Source = parent;
            e_1.SetBinding(ContentPresenter.ContentProperty, binding_e_1_Content);
            return e_0;
        }
        
        private static UIElement r_12_ctMethod(UIElement parent) {
            // e_2 element
            Grid e_2 = new Grid();
            e_2.Parent = parent;
            e_2.Name = "e_2";
            e_2.Height = 24F;
            ColumnDefinition col_e_2_0 = new ColumnDefinition();
            col_e_2_0.Width = new GridLength(12F, GridUnitType.Pixel);
            e_2.ColumnDefinitions.Add(col_e_2_0);
            ColumnDefinition col_e_2_1 = new ColumnDefinition();
            e_2.ColumnDefinitions.Add(col_e_2_1);
            ColumnDefinition col_e_2_2 = new ColumnDefinition();
            col_e_2_2.Width = new GridLength(12F, GridUnitType.Pixel);
            e_2.ColumnDefinitions.Add(col_e_2_2);
            // PART_SliderRailLeft element
            Border PART_SliderRailLeft = new Border();
            e_2.Children.Add(PART_SliderRailLeft);
            PART_SliderRailLeft.Name = "PART_SliderRailLeft";
            PART_SliderRailLeft.Height = 10F;
            PART_SliderRailLeft.IsHitTestVisible = false;
            PART_SliderRailLeft.SetResourceReference(Border.BackgroundProperty, "LowlightColor");
            // PART_SliderRailCenter element
            Border PART_SliderRailCenter = new Border();
            e_2.Children.Add(PART_SliderRailCenter);
            PART_SliderRailCenter.Name = "PART_SliderRailCenter";
            PART_SliderRailCenter.Height = 10F;
            PART_SliderRailCenter.IsHitTestVisible = false;
            Grid.SetColumn(PART_SliderRailCenter, 1);
            PART_SliderRailCenter.SetResourceReference(Border.BackgroundProperty, "LowlightColor");
            // PART_SliderRailRight element
            Border PART_SliderRailRight = new Border();
            e_2.Children.Add(PART_SliderRailRight);
            PART_SliderRailRight.Name = "PART_SliderRailRight";
            PART_SliderRailRight.Height = 10F;
            PART_SliderRailRight.IsHitTestVisible = false;
            Grid.SetColumn(PART_SliderRailRight, 2);
            PART_SliderRailRight.SetResourceReference(Border.BackgroundProperty, "LowlightColor");
            // PART_Track element
            Track PART_Track = new Track();
            e_2.Children.Add(PART_Track);
            PART_Track.Name = "PART_Track";
            PART_Track.Margin = new Thickness(6F, 0F, 6F, 0F);
            // e_3 element
            RepeatButton e_3 = new RepeatButton();
            e_3.Name = "e_3";
            e_3.ClickMode = ClickMode.Press;
            e_3.SetResourceReference(RepeatButton.StyleProperty, "SliderButtonStyle");
            PART_Track.IncreaseRepeatButton = e_3;
            // e_4 element
            RepeatButton e_4 = new RepeatButton();
            e_4.Name = "e_4";
            e_4.ClickMode = ClickMode.Press;
            e_4.SetResourceReference(RepeatButton.StyleProperty, "SliderButtonStyle");
            PART_Track.DecreaseRepeatButton = e_4;
            // e_5 element
            Thumb e_5 = new Thumb();
            e_5.Name = "e_5";
            e_5.SetResourceReference(Thumb.StyleProperty, "SliderThumbStyle");
            PART_Track.Thumb = e_5;
            Grid.SetColumnSpan(PART_Track, 3);
            return e_2;
        }
        
        private static UIElement r_14_s_S_2_ctMethod(UIElement parent) {
            // e_6 element
            Border e_6 = new Border();
            e_6.Parent = parent;
            e_6.Name = "e_6";
            e_6.Background = new SolidColorBrush(new ColorW(255, 255, 255, 0));
            return e_6;
        }
        
        private static UIElement r_15_s_S_1_ctMethod(UIElement parent) {
            // PART_SliderThumb element
            Border PART_SliderThumb = new Border();
            PART_SliderThumb.Parent = parent;
            PART_SliderThumb.Name = "PART_SliderThumb";
            PART_SliderThumb.Height = 20F;
            PART_SliderThumb.Width = 20F;
            PART_SliderThumb.SnapsToDevicePixels = true;
            PART_SliderThumb.SetResourceReference(Border.BackgroundProperty, "ActiveColor");
            return PART_SliderThumb;
        }
        
        private static UIElement r_17_s_S_2_ctMethod(UIElement parent) {
            // e_7 element
            StackPanel e_7 = new StackPanel();
            e_7.Parent = parent;
            e_7.Name = "e_7";
            e_7.Orientation = Orientation.Horizontal;
            // e_8 element
            Grid e_8 = new Grid();
            e_7.Children.Add(e_8);
            e_8.Name = "e_8";
            e_8.Margin = new Thickness(0F, 0F, 10F, 0F);
            // PART_NotChecked element
            Border PART_NotChecked = new Border();
            e_8.Children.Add(PART_NotChecked);
            PART_NotChecked.Name = "PART_NotChecked";
            PART_NotChecked.Height = 20F;
            PART_NotChecked.Width = 20F;
            PART_NotChecked.SetResourceReference(Border.BackgroundProperty, "BackgroundColor");
            // PART_CheckMark element
            Border PART_CheckMark = new Border();
            e_8.Children.Add(PART_CheckMark);
            PART_CheckMark.Name = "PART_CheckMark";
            PART_CheckMark.Height = 20F;
            PART_CheckMark.Width = 20F;
            PART_CheckMark.Visibility = Visibility.Collapsed;
            PART_CheckMark.BorderThickness = new Thickness(4F, 4F, 4F, 4F);
            PART_CheckMark.SetResourceReference(Border.BorderBrushProperty, "BackgroundColor");
            PART_CheckMark.SetResourceReference(Border.BackgroundProperty, "AccentColor");
            // e_9 element
            ContentPresenter e_9 = new ContentPresenter();
            e_7.Children.Add(e_9);
            e_9.Name = "e_9";
            e_9.VerticalAlignment = VerticalAlignment.Center;
            Binding binding_e_9_Content = new Binding("Content");
            binding_e_9_Content.Source = parent;
            e_9.SetBinding(ContentPresenter.ContentProperty, binding_e_9_Content);
            return e_7;
        }
        
        private static UIElement PART_Button_s_S_0_ctMethod(UIElement parent) {
            // e_11 element
            ContentPresenter e_11 = new ContentPresenter();
            e_11.Parent = parent;
            e_11.Name = "e_11";
            Binding binding_e_11_Content = new Binding("Content");
            binding_e_11_Content.Source = parent;
            e_11.SetBinding(ContentPresenter.ContentProperty, binding_e_11_Content);
            return e_11;
        }
        
        private static UIElement r_18_s_S_3_ctMethod(UIElement parent) {
            // e_10 element
            Grid e_10 = new Grid();
            e_10.Parent = parent;
            e_10.Name = "e_10";
            ColumnDefinition col_e_10_0 = new ColumnDefinition();
            col_e_10_0.Width = new GridLength(16F, GridUnitType.Pixel);
            e_10.ColumnDefinitions.Add(col_e_10_0);
            ColumnDefinition col_e_10_1 = new ColumnDefinition();
            e_10.ColumnDefinitions.Add(col_e_10_1);
            ColumnDefinition col_e_10_2 = new ColumnDefinition();
            col_e_10_2.Width = new GridLength(32F, GridUnitType.Pixel);
            e_10.ColumnDefinitions.Add(col_e_10_2);
            // PART_ComboBoxLeft element
            Image PART_ComboBoxLeft = new Image();
            e_10.Children.Add(PART_ComboBoxLeft);
            PART_ComboBoxLeft.Name = "PART_ComboBoxLeft";
            PART_ComboBoxLeft.SnapsToDevicePixels = true;
            PART_ComboBoxLeft.SetResourceReference(Image.SourceProperty, "BackgroundColor");
            // PART_ComboBoxCenter element
            Image PART_ComboBoxCenter = new Image();
            e_10.Children.Add(PART_ComboBoxCenter);
            PART_ComboBoxCenter.Name = "PART_ComboBoxCenter";
            PART_ComboBoxCenter.SnapsToDevicePixels = true;
            PART_ComboBoxCenter.Stretch = Stretch.Fill;
            Grid.SetColumn(PART_ComboBoxCenter, 1);
            PART_ComboBoxCenter.SetResourceReference(Image.SourceProperty, "BackgroundColor");
            // PART_ComboBoxRight element
            Image PART_ComboBoxRight = new Image();
            e_10.Children.Add(PART_ComboBoxRight);
            PART_ComboBoxRight.Name = "PART_ComboBoxRight";
            PART_ComboBoxRight.SnapsToDevicePixels = true;
            Grid.SetColumn(PART_ComboBoxRight, 2);
            PART_ComboBoxRight.SetResourceReference(Image.SourceProperty, "BackgroundColor");
            // PART_Button element
            ToggleButton PART_Button = new ToggleButton();
            e_10.Children.Add(PART_Button);
            PART_Button.Name = "PART_Button";
            PART_Button.Focusable = false;
            Style PART_Button_s = new Style(typeof(ToggleButton));
            Func<UIElement, UIElement> PART_Button_s_S_0_ctFunc = PART_Button_s_S_0_ctMethod;
            ControlTemplate PART_Button_s_S_0_ct = new ControlTemplate(typeof(ToggleButton), PART_Button_s_S_0_ctFunc);
            Setter PART_Button_s_S_0 = new Setter(ToggleButton.TemplateProperty, PART_Button_s_S_0_ct);
            PART_Button_s.Setters.Add(PART_Button_s_S_0);
            PART_Button.Style = PART_Button_s;
            PART_Button.IsTabStop = false;
            PART_Button.ClickMode = ClickMode.Press;
            Grid.SetColumnSpan(PART_Button, 3);
            Binding binding_PART_Button_IsChecked = new Binding("IsDropDownOpen");
            binding_PART_Button_IsChecked.Source = parent;
            PART_Button.SetBinding(ToggleButton.IsCheckedProperty, binding_PART_Button_IsChecked);
            // e_12 element
            ContentPresenter e_12 = new ContentPresenter();
            e_10.Children.Add(e_12);
            e_12.Name = "e_12";
            e_12.IsHitTestVisible = false;
            Grid.SetColumn(e_12, 1);
            Binding binding_e_12_Content = new Binding("SelectionBoxItem");
            binding_e_12_Content.Source = parent;
            e_12.SetBinding(ContentPresenter.ContentProperty, binding_e_12_Content);
            // PART_Popup element
            Popup PART_Popup = new Popup();
            e_10.Children.Add(PART_Popup);
            PART_Popup.Name = "PART_Popup";
            Grid.SetColumnSpan(PART_Popup, 3);
            Binding binding_PART_Popup_MaxHeight = new Binding("MaxDropDownHeight");
            binding_PART_Popup_MaxHeight.Source = parent;
            PART_Popup.SetBinding(Popup.MaxHeightProperty, binding_PART_Popup_MaxHeight);
            Binding binding_PART_Popup_IsOpen = new Binding("IsDropDownOpen");
            binding_PART_Popup_IsOpen.Source = parent;
            PART_Popup.SetBinding(Popup.IsOpenProperty, binding_PART_Popup_IsOpen);
            // e_13 element
            ScrollViewer e_13 = new ScrollViewer();
            PART_Popup.Child = e_13;
            e_13.Name = "e_13";
            // e_14 element
            StackPanel e_14 = new StackPanel();
            e_13.Content = e_14;
            e_14.Name = "e_14";
            e_14.Margin = new Thickness(16F, 0F, 0F, 0F);
            e_14.IsItemsHost = true;
            return e_10;
        }
        
        private static UIElement r_19_s_S_0_ctMethod(UIElement parent) {
            // e_15 element
            ContentPresenter e_15 = new ContentPresenter();
            e_15.Parent = parent;
            e_15.Name = "e_15";
            Binding binding_e_15_Content = new Binding("Content");
            binding_e_15_Content.Source = parent;
            e_15.SetBinding(ContentPresenter.ContentProperty, binding_e_15_Content);
            return e_15;
        }
        
        private static UIElement r_20_s_S_2_ctMethod(UIElement parent) {
            // e_16 element
            Border e_16 = new Border();
            e_16.Parent = parent;
            e_16.Name = "e_16";
            e_16.SnapsToDevicePixels = true;
            e_16.Padding = new Thickness(5F, 5F, 5F, 5F);
            Binding binding_e_16_Background = new Binding("Background");
            binding_e_16_Background.Source = parent;
            e_16.SetBinding(Border.BackgroundProperty, binding_e_16_Background);
            // e_17 element
            ContentPresenter e_17 = new ContentPresenter();
            e_16.Child = e_17;
            e_17.Name = "e_17";
            e_17.Margin = new Thickness(5F, 2F, 5F, 2F);
            Binding binding_e_17_Content = new Binding("Content");
            binding_e_17_Content.Source = parent;
            e_17.SetBinding(ContentPresenter.ContentProperty, binding_e_17_Content);
            return e_16;
        }
        
        private static UIElement r_22_s_S_2_ctMethod(UIElement parent) {
            // e_18 element
            Border e_18 = new Border();
            e_18.Parent = parent;
            e_18.Name = "e_18";
            e_18.SnapsToDevicePixels = true;
            e_18.Padding = new Thickness(5F, 5F, 5F, 5F);
            Binding binding_e_18_Background = new Binding("Background");
            binding_e_18_Background.Source = parent;
            e_18.SetBinding(Border.BackgroundProperty, binding_e_18_Background);
            // e_19 element
            ContentPresenter e_19 = new ContentPresenter();
            e_18.Child = e_19;
            e_19.Name = "e_19";
            e_19.Margin = new Thickness(10F, 2F, 10F, 2F);
            e_19.HorizontalAlignment = HorizontalAlignment.Center;
            e_19.VerticalAlignment = VerticalAlignment.Center;
            Binding binding_e_19_Content = new Binding("Header");
            binding_e_19_Content.Source = parent;
            e_19.SetBinding(ContentPresenter.ContentProperty, binding_e_19_Content);
            return e_18;
        }
        
        private static UIElement r_23_s_S_6_ctMethod(UIElement parent) {
            // e_20 element
            Grid e_20 = new Grid();
            e_20.Parent = parent;
            e_20.Name = "e_20";
            RowDefinition row_e_20_0 = new RowDefinition();
            row_e_20_0.MinHeight = 36F;
            e_20.RowDefinitions.Add(row_e_20_0);
            ColumnDefinition col_e_20_0 = new ColumnDefinition();
            col_e_20_0.Width = new GridLength(4F, GridUnitType.Pixel);
            e_20.ColumnDefinitions.Add(col_e_20_0);
            ColumnDefinition col_e_20_1 = new ColumnDefinition();
            e_20.ColumnDefinitions.Add(col_e_20_1);
            ColumnDefinition col_e_20_2 = new ColumnDefinition();
            col_e_20_2.Width = new GridLength(4F, GridUnitType.Pixel);
            e_20.ColumnDefinitions.Add(col_e_20_2);
            // PART_TextBoxLeft element
            Border PART_TextBoxLeft = new Border();
            e_20.Children.Add(PART_TextBoxLeft);
            PART_TextBoxLeft.Name = "PART_TextBoxLeft";
            PART_TextBoxLeft.IsHitTestVisible = false;
            PART_TextBoxLeft.SetResourceReference(Border.BackgroundProperty, "BackgroundColor");
            // PART_TextBoxCenter element
            Border PART_TextBoxCenter = new Border();
            e_20.Children.Add(PART_TextBoxCenter);
            PART_TextBoxCenter.Name = "PART_TextBoxCenter";
            PART_TextBoxCenter.IsHitTestVisible = false;
            Grid.SetColumn(PART_TextBoxCenter, 1);
            PART_TextBoxCenter.SetResourceReference(Border.BackgroundProperty, "BackgroundColor");
            // PART_TextBoxRight element
            Border PART_TextBoxRight = new Border();
            e_20.Children.Add(PART_TextBoxRight);
            PART_TextBoxRight.Name = "PART_TextBoxRight";
            PART_TextBoxRight.IsHitTestVisible = false;
            Grid.SetColumn(PART_TextBoxRight, 2);
            PART_TextBoxRight.SetResourceReference(Border.BackgroundProperty, "BackgroundColor");
            // PART_ScrollViewer element
            ScrollViewer PART_ScrollViewer = new ScrollViewer();
            e_20.Children.Add(PART_ScrollViewer);
            PART_ScrollViewer.Name = "PART_ScrollViewer";
            PART_ScrollViewer.Margin = new Thickness(0F, 4F, 0F, 4F);
            PART_ScrollViewer.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            PART_ScrollViewer.VerticalContentAlignment = VerticalAlignment.Stretch;
            Grid.SetColumn(PART_ScrollViewer, 1);
            PART_ScrollViewer.SetResourceReference(ScrollViewer.StyleProperty, "TextBoxScrollViewer");
            // e_21 element
            TextBlock e_21 = new TextBlock();
            PART_ScrollViewer.Content = e_21;
            e_21.Name = "e_21";
            e_21.VerticalAlignment = VerticalAlignment.Center;
            Binding binding_e_21_HorizontalAlignment = new Binding("HorizontalContentAlignment");
            binding_e_21_HorizontalAlignment.Source = parent;
            e_21.SetBinding(TextBlock.HorizontalAlignmentProperty, binding_e_21_HorizontalAlignment);
            Binding binding_e_21_Padding = new Binding("Padding");
            binding_e_21_Padding.Source = parent;
            e_21.SetBinding(TextBlock.PaddingProperty, binding_e_21_Padding);
            Binding binding_e_21_TextAlignment = new Binding("TextAlignment");
            binding_e_21_TextAlignment.Source = parent;
            e_21.SetBinding(TextBlock.TextAlignmentProperty, binding_e_21_TextAlignment);
            Binding binding_e_21_Text = new Binding("Text");
            binding_e_21_Text.Source = parent;
            e_21.SetBinding(TextBlock.TextProperty, binding_e_21_Text);
            return e_20;
        }
        
        private static UIElement r_24_s_S_1_ctMethod(UIElement parent) {
            // PART_ScrollContentPresenter element
            ScrollContentPresenter PART_ScrollContentPresenter = new ScrollContentPresenter();
            PART_ScrollContentPresenter.Parent = parent;
            PART_ScrollContentPresenter.Name = "PART_ScrollContentPresenter";
            PART_ScrollContentPresenter.SnapsToDevicePixels = true;
            Binding binding_PART_ScrollContentPresenter_Margin = new Binding("Padding");
            binding_PART_ScrollContentPresenter_Margin.Source = parent;
            PART_ScrollContentPresenter.SetBinding(ScrollContentPresenter.MarginProperty, binding_PART_ScrollContentPresenter_Margin);
            Binding binding_PART_ScrollContentPresenter_HorizontalAlignment = new Binding("HorizontalContentAlignment");
            binding_PART_ScrollContentPresenter_HorizontalAlignment.Source = parent;
            PART_ScrollContentPresenter.SetBinding(ScrollContentPresenter.HorizontalAlignmentProperty, binding_PART_ScrollContentPresenter_HorizontalAlignment);
            Binding binding_PART_ScrollContentPresenter_VerticalAlignment = new Binding("VerticalContentAlignment");
            binding_PART_ScrollContentPresenter_VerticalAlignment.Source = parent;
            PART_ScrollContentPresenter.SetBinding(ScrollContentPresenter.VerticalAlignmentProperty, binding_PART_ScrollContentPresenter_VerticalAlignment);
            Binding binding_PART_ScrollContentPresenter_Content = new Binding("Content");
            binding_PART_ScrollContentPresenter_Content.Source = parent;
            PART_ScrollContentPresenter.SetBinding(ScrollContentPresenter.ContentProperty, binding_PART_ScrollContentPresenter_Content);
            return PART_ScrollContentPresenter;
        }
    }
}
