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
    public partial class SettingsMenu : UIRoot {
        
        private Grid e_0;
        
        private TabControl e_1;
        
        private Button e_29;
        
        public SettingsMenu() : 
                base() {
            this.Initialize();
        }
        
        public SettingsMenu(int width, int height) : 
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
            this.e_1 = new TabControl();
            this.e_0.Children.Add(this.e_1);
            this.e_1.Name = "e_1";
            this.e_1.ItemsSource = Get_e_1_Items();
            // e_29 element
            this.e_29 = new Button();
            this.e_0.Children.Add(this.e_29);
            this.e_29.Name = "e_29";
            this.e_29.Height = 28F;
            this.e_29.Width = 28F;
            this.e_29.HorizontalAlignment = HorizontalAlignment.Right;
            this.e_29.VerticalAlignment = VerticalAlignment.Top;
            this.e_29.Content = "x";
            Binding binding_e_29_Command = new Binding("ExitCommand");
            this.e_29.SetBinding(Button.CommandProperty, binding_e_29_Command);
            this.e_29.SetResourceReference(Button.StyleProperty, "ButtonStyle");
            FontManager.Instance.AddFont("Code 7x5", 18F, FontStyle.Regular, "Code_7x5_13.5_Regular");
        }
        
        private static void InitializeElementResources(UIElement elem) {
            elem.Resources.MergedDictionaries.Add(Dictionary.Instance);
        }
        
        private static System.Collections.ObjectModel.ObservableCollection<object> Get_e_1_Items() {
            System.Collections.ObjectModel.ObservableCollection<object> items = new System.Collections.ObjectModel.ObservableCollection<object>();
            // e_2 element
            TabItem e_2 = new TabItem();
            e_2.Name = "e_2";
            e_2.Header = "General";
            // e_3 element
            StackPanel e_3 = new StackPanel();
            e_2.Content = e_3;
            e_3.Name = "e_3";
            e_3.Margin = new Thickness(20F, 0F, 20F, 0F);
            // e_4 element
            TextBlock e_4 = new TextBlock();
            e_3.Children.Add(e_4);
            e_4.Name = "e_4";
            e_4.Text = "Horizontal Sensitivity";
            // e_5 element
            Grid e_5 = new Grid();
            e_3.Children.Add(e_5);
            e_5.Name = "e_5";
            e_5.Margin = new Thickness(0F, 0F, -0.4F, 0F);
            // e_6 element
            Slider e_6 = new Slider();
            e_5.Children.Add(e_6);
            e_6.Name = "e_6";
            e_6.Margin = new Thickness(0F, 0F, 135F, 0F);
            e_6.Maximum = 1F;
            e_6.SmallChange = 0.1F;
            e_6.LargeChange = 1F;
            Binding binding_e_6_Value = new Binding("HorizontalSens");
            e_6.SetBinding(Slider.ValueProperty, binding_e_6_Value);
            // e_7 element
            TextBlock e_7 = new TextBlock();
            e_5.Children.Add(e_7);
            e_7.Name = "e_7";
            e_7.Width = 35F;
            e_7.Margin = new Thickness(0F, 0F, 100F, 0F);
            e_7.HorizontalAlignment = HorizontalAlignment.Right;
            Binding binding_e_7_Text = new Binding("HorizontalSens");
            e_7.SetBinding(TextBlock.TextProperty, binding_e_7_Text);
            // e_8 element
            TextBlock e_8 = new TextBlock();
            e_3.Children.Add(e_8);
            e_8.Name = "e_8";
            e_8.Text = "Vertical Sensitivity";
            // e_9 element
            Grid e_9 = new Grid();
            e_3.Children.Add(e_9);
            e_9.Name = "e_9";
            // e_10 element
            Slider e_10 = new Slider();
            e_9.Children.Add(e_10);
            e_10.Name = "e_10";
            e_10.Margin = new Thickness(0F, 0F, 135F, 0F);
            e_10.Maximum = 1F;
            e_10.SmallChange = 0.1F;
            e_10.LargeChange = 1F;
            Binding binding_e_10_Value = new Binding("VerticalSens");
            e_10.SetBinding(Slider.ValueProperty, binding_e_10_Value);
            // e_11 element
            TextBlock e_11 = new TextBlock();
            e_9.Children.Add(e_11);
            e_11.Name = "e_11";
            e_11.Width = 35F;
            e_11.Margin = new Thickness(0F, 0F, 99.6F, 0F);
            e_11.HorizontalAlignment = HorizontalAlignment.Right;
            Binding binding_e_11_Text = new Binding("VerticalSens");
            e_11.SetBinding(TextBlock.TextProperty, binding_e_11_Text);
            items.Add(e_2);
            // e_12 element
            TabItem e_12 = new TabItem();
            e_12.Name = "e_12";
            e_12.Header = "Audio";
            // e_13 element
            StackPanel e_13 = new StackPanel();
            e_12.Content = e_13;
            e_13.Name = "e_13";
            e_13.Margin = new Thickness(20F, 0F, 20F, 0F);
            // e_14 element
            TextBlock e_14 = new TextBlock();
            e_13.Children.Add(e_14);
            e_14.Name = "e_14";
            e_14.Text = "Master Volume";
            // e_15 element
            Grid e_15 = new Grid();
            e_13.Children.Add(e_15);
            e_15.Name = "e_15";
            // e_16 element
            Slider e_16 = new Slider();
            e_15.Children.Add(e_16);
            e_16.Name = "e_16";
            e_16.Margin = new Thickness(0F, 0F, 65F, 0F);
            e_16.Maximum = 100F;
            e_16.SmallChange = 1F;
            e_16.LargeChange = 10F;
            Binding binding_e_16_Value = new Binding("MasterVolume");
            e_16.SetBinding(Slider.ValueProperty, binding_e_16_Value);
            // e_17 element
            TextBlock e_17 = new TextBlock();
            e_15.Children.Add(e_17);
            e_17.Name = "e_17";
            e_17.Width = 45F;
            e_17.Margin = new Thickness(0F, 0F, 0F, 0F);
            e_17.HorizontalAlignment = HorizontalAlignment.Right;
            Binding binding_e_17_Text = new Binding("MasterVolume");
            e_17.SetBinding(TextBlock.TextProperty, binding_e_17_Text);
            // e_18 element
            TextBlock e_18 = new TextBlock();
            e_13.Children.Add(e_18);
            e_18.Name = "e_18";
            e_18.Text = "Music Volume";
            // e_19 element
            Grid e_19 = new Grid();
            e_13.Children.Add(e_19);
            e_19.Name = "e_19";
            // e_20 element
            Slider e_20 = new Slider();
            e_19.Children.Add(e_20);
            e_20.Name = "e_20";
            e_20.Margin = new Thickness(0F, 0F, 65F, 0F);
            e_20.Maximum = 100F;
            Binding binding_e_20_Value = new Binding("MusicVolume");
            e_20.SetBinding(Slider.ValueProperty, binding_e_20_Value);
            // e_21 element
            TextBlock e_21 = new TextBlock();
            e_19.Children.Add(e_21);
            e_21.Name = "e_21";
            e_21.Width = 45F;
            e_21.Margin = new Thickness(0F, 0F, 0F, 0F);
            e_21.HorizontalAlignment = HorizontalAlignment.Right;
            Binding binding_e_21_Text = new Binding("MusicVolume");
            e_21.SetBinding(TextBlock.TextProperty, binding_e_21_Text);
            // e_22 element
            TextBlock e_22 = new TextBlock();
            e_13.Children.Add(e_22);
            e_22.Name = "e_22";
            e_22.Text = "Effect Volume";
            // e_23 element
            Grid e_23 = new Grid();
            e_13.Children.Add(e_23);
            e_23.Name = "e_23";
            // e_24 element
            Slider e_24 = new Slider();
            e_23.Children.Add(e_24);
            e_24.Name = "e_24";
            e_24.Margin = new Thickness(0F, 0F, 65F, 0F);
            e_24.Maximum = 100F;
            Binding binding_e_24_Value = new Binding("EffectVolume");
            e_24.SetBinding(Slider.ValueProperty, binding_e_24_Value);
            // e_25 element
            TextBlock e_25 = new TextBlock();
            e_23.Children.Add(e_25);
            e_25.Name = "e_25";
            e_25.Width = 45F;
            e_25.Margin = new Thickness(0F, 0F, 0F, 0F);
            e_25.HorizontalAlignment = HorizontalAlignment.Right;
            Binding binding_e_25_Text = new Binding("EffectVolume");
            e_25.SetBinding(TextBlock.TextProperty, binding_e_25_Text);
            items.Add(e_12);
            // e_26 element
            TabItem e_26 = new TabItem();
            e_26.Name = "e_26";
            e_26.Header = "Visual";
            // e_27 element
            StackPanel e_27 = new StackPanel();
            e_26.Content = e_27;
            e_27.Name = "e_27";
            // e_28 element
            CheckBox e_28 = new CheckBox();
            e_27.Children.Add(e_28);
            e_28.Name = "e_28";
            e_28.Content = "Is fullscreen";
            items.Add(e_26);
            return items;
        }
    }
}
