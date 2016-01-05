using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace Labshell.View
{
    public class WateMarkTextBox: TextBox
    {
        private Label wateMarkLable;  
  
        private ScrollViewer wateMarkScrollViewer;  
  
        static WateMarkTextBox()  
        {  
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WateMarkTextBox), new FrameworkPropertyMetadata(typeof(WateMarkTextBox)));  
        }  
  
        public WateMarkTextBox()  
        {  
            this.Loaded += new RoutedEventHandler(WateMarkTextBox_Loaded);  
        }  
  
        void WateMarkTextBox_LostFocus(object sender, RoutedEventArgs e)  
        {  
           
        }  
  
        void WateMarkTextBox_Loaded(object sender, RoutedEventArgs e)  
        {  
            this.wateMarkLable.Content = WateMark;  
        }  
  
        void WateMarkTextBox_GotFocus(object sender, RoutedEventArgs e)  
        {  
            this.wateMarkLable.Visibility = Visibility.Hidden;  
        }  
  
        public string WateMark  
        {  
            get { return (string)GetValue(WateMarkProperty); }  
  
            set { SetValue(WateMarkProperty, value); }  
        }  
  
        public static DependencyProperty WateMarkProperty =  
            DependencyProperty.Register("WateMark", typeof(string), typeof(WateMarkTextBox), new UIPropertyMetadata(""));  
  
        public override void OnApplyTemplate()  
        {  
            base.OnApplyTemplate();  
  
            this.wateMarkLable = this.GetTemplateChild("TextPrompt") as Label;  
  
            this.wateMarkScrollViewer = this.GetTemplateChild("PART_ContentHost") as ScrollViewer;  
        }
    }
}
