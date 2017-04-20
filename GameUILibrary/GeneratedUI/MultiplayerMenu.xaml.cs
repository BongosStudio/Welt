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
    public partial class MultiplayerMenu : UIRoot {
        
        private Grid e_0;
        
        private ListBox e_1;
        
        private Button e_8;
        
        private TextBox e_9;
        
        private Button e_10;
        
        private TextBlock e_11;
        
        public MultiplayerMenu() : 
                base() {
            this.Initialize();
        }
        
        public MultiplayerMenu(int width, int height) : 
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
            InitializeElementResources(this);
            // e_0 element
            this.e_0 = new Grid();
            this.Content = this.e_0;
            this.e_0.Name = "e_0";
            ColumnDefinition col_e_0_0 = new ColumnDefinition();
            col_e_0_0.Width = new GridLength(2F, GridUnitType.Star);
            this.e_0.ColumnDefinitions.Add(col_e_0_0);
            ColumnDefinition col_e_0_1 = new ColumnDefinition();
            col_e_0_1.Width = new GridLength(8F, GridUnitType.Star);
            this.e_0.ColumnDefinitions.Add(col_e_0_1);
            ColumnDefinition col_e_0_2 = new ColumnDefinition();
            col_e_0_2.Width = new GridLength(3F, GridUnitType.Star);
            this.e_0.ColumnDefinitions.Add(col_e_0_2);
            ColumnDefinition col_e_0_3 = new ColumnDefinition();
            col_e_0_3.Width = new GridLength(2F, GridUnitType.Star);
            this.e_0.ColumnDefinitions.Add(col_e_0_3);
            // e_1 element
            this.e_1 = new ListBox();
            this.e_0.Children.Add(this.e_1);
            this.e_1.Name = "e_1";
            this.e_1.Margin = new Thickness(0F, 90F, 0F, 100F);
            Func<UIElement, UIElement> e_1_dtFunc = e_1_dtMethod;
            this.e_1.ItemTemplate = new DataTemplate(e_1_dtFunc);
            Grid.SetColumn(this.e_1, 1);
            Grid.SetColumnSpan(this.e_1, 2);
            Binding binding_e_1_ItemsSource = new Binding("ServerList");
            this.e_1.SetBinding(ListBox.ItemsSourceProperty, binding_e_1_ItemsSource);
            // e_8 element
            this.e_8 = new Button();
            this.e_0.Children.Add(this.e_8);
            this.e_8.Name = "e_8";
            this.e_8.Height = 28F;
            this.e_8.Width = 28F;
            this.e_8.HorizontalAlignment = HorizontalAlignment.Right;
            this.e_8.VerticalAlignment = VerticalAlignment.Top;
            this.e_8.Content = "x";
            Grid.SetColumn(this.e_8, 3);
            Binding binding_e_8_Command = new Binding("ExitCommand");
            this.e_8.SetBinding(Button.CommandProperty, binding_e_8_Command);
            this.e_8.SetResourceReference(Button.StyleProperty, "ButtonStyle");
            // e_9 element
            this.e_9 = new TextBox();
            this.e_0.Children.Add(this.e_9);
            this.e_9.Name = "e_9";
            this.e_9.Margin = new Thickness(0F, 0F, 0F, 55F);
            this.e_9.VerticalAlignment = VerticalAlignment.Bottom;
            Grid.SetColumn(this.e_9, 1);
            Binding binding_e_9_IsEnabled = new Binding("IsCompletedAddition");
            this.e_9.SetBinding(TextBox.IsEnabledProperty, binding_e_9_IsEnabled);
            Binding binding_e_9_Text = new Binding("NewIPAddressText");
            this.e_9.SetBinding(TextBox.TextProperty, binding_e_9_Text);
            // e_10 element
            this.e_10 = new Button();
            this.e_0.Children.Add(this.e_10);
            this.e_10.Name = "e_10";
            this.e_10.Margin = new Thickness(0F, 0F, 0F, 55F);
            this.e_10.VerticalAlignment = VerticalAlignment.Bottom;
            this.e_10.Content = "Add";
            Grid.SetColumn(this.e_10, 2);
            Binding binding_e_10_IsEnabled = new Binding("IsCompletedAddition");
            this.e_10.SetBinding(Button.IsEnabledProperty, binding_e_10_IsEnabled);
            Binding binding_e_10_Command = new Binding("AddNewIPCommand");
            this.e_10.SetBinding(Button.CommandProperty, binding_e_10_Command);
            this.e_10.SetResourceReference(Button.StyleProperty, "ButtonStyle");
            // e_11 element
            this.e_11 = new TextBlock();
            this.e_0.Children.Add(this.e_11);
            this.e_11.Name = "e_11";
            this.e_11.Margin = new Thickness(0F, 0F, 0F, 34F);
            this.e_11.VerticalAlignment = VerticalAlignment.Bottom;
            this.e_11.TextWrapping = TextWrapping.Wrap;
            Grid.SetColumn(this.e_11, 1);
            Grid.SetColumnSpan(this.e_11, 2);
            Binding binding_e_11_Text = new Binding("StatusText");
            this.e_11.SetBinding(TextBlock.TextProperty, binding_e_11_Text);
        }
        
        private static void InitializeElementResources(UIElement elem) {
            elem.Resources.MergedDictionaries.Add(Dictionary.Instance);
        }
        
        private static UIElement e_1_dtMethod(UIElement parent) {
            // e_2 element
            Grid e_2 = new Grid();
            e_2.Parent = parent;
            e_2.Name = "e_2";
            e_2.Height = 75F;
            // e_3 element
            StackPanel e_3 = new StackPanel();
            e_2.Children.Add(e_3);
            e_3.Name = "e_3";
            e_3.HorizontalAlignment = HorizontalAlignment.Left;
            // e_4 element
            TextBlock e_4 = new TextBlock();
            e_3.Children.Add(e_4);
            e_4.Name = "e_4";
            Binding binding_e_4_Text = new Binding("ServerName");
            e_4.SetBinding(TextBlock.TextProperty, binding_e_4_Text);
            // e_5 element
            TextBlock e_5 = new TextBlock();
            e_3.Children.Add(e_5);
            e_5.Name = "e_5";
            Binding binding_e_5_Text = new Binding("ServerMotd");
            e_5.SetBinding(TextBlock.TextProperty, binding_e_5_Text);
            // e_6 element
            TextBlock e_6 = new TextBlock();
            e_2.Children.Add(e_6);
            e_6.Name = "e_6";
            e_6.HorizontalAlignment = HorizontalAlignment.Right;
            e_6.VerticalAlignment = VerticalAlignment.Bottom;
            Binding binding_e_6_Text = new Binding("CurrentPlayerCount");
            e_6.SetBinding(TextBlock.TextProperty, binding_e_6_Text);
            // e_7 element
            Button e_7 = new Button();
            e_2.Children.Add(e_7);
            e_7.Name = "e_7";
            e_7.HorizontalAlignment = HorizontalAlignment.Right;
            e_7.VerticalAlignment = VerticalAlignment.Top;
            e_7.Content = ">";
            Binding binding_e_7_Command = new Binding("JoinServerCommand");
            e_7.SetBinding(Button.CommandProperty, binding_e_7_Command);
            e_7.SetResourceReference(Button.StyleProperty, "ButtonStyle");
            return e_2;
        }
    }
}
