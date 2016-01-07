using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace Labshell.View
{
    public class WaterMarkText : DependencyObject
    {
        public static readonly DependencyProperty IsMonitoringProperty =
            DependencyProperty.RegisterAttached("IsMonitoring",
                                                typeof(bool),
                                                typeof(WaterMarkText),
                                                new UIPropertyMetadata(false, IsMonitoringChanged));

        public static bool GetIsMonitoring(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsMonitoringProperty);
        }

        public static void SetIsMonitoring(DependencyObject obj, bool value)
        {
            obj.SetValue(IsMonitoringProperty, value);
        }

        public static readonly DependencyProperty TextLengthProperty =
            DependencyProperty.RegisterAttached("TextLength",
                                                typeof(int),
                                                typeof(WaterMarkText),
                                                new UIPropertyMetadata(0, TextLengthChanged));

        public static int GetTextLength(DependencyObject obj)
        {
            return (int)obj.GetValue(TextLengthProperty);
        }

        public static void SetTextLength(DependencyObject obj, int value)
        {
            obj.SetValue(TextLengthProperty, value);
        }

        public static readonly DependencyProperty HasTextProperty =
            DependencyProperty.RegisterAttached("HasText",
                                                typeof(bool),
                                                typeof(WaterMarkText),
                                                new UIPropertyMetadata(false));

        public static bool GetHasText(DependencyObject obj)
        {
            return (bool)obj.GetValue(HasTextProperty);
        }

        public static void SetHasText(DependencyObject obj, bool value)
        {
            obj.SetValue(HasTextProperty, value);
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.RegisterAttached("Text",
                                                typeof(string),
                                                typeof(WaterMarkText),
                                                new UIPropertyMetadata(string.Empty));

        public static string GetText(DependencyObject obj)
        {
            return (string)obj.GetValue(TextProperty);
        }

        public static void SetText(DependencyObject obj, string value)
        {
            obj.SetValue(TextProperty, value);
        }

        private static void IsMonitoringChanged(DependencyObject obj,
                                                DependencyPropertyChangedEventArgs args)
        {
            var passwordBox = obj as PasswordBox;
            if (passwordBox != null)
            {
                if ((bool)args.NewValue)
                {
                    passwordBox.PasswordChanged += PasswordChanged;
                }
                else
                {
                    passwordBox.PasswordChanged -= PasswordChanged;
                }
            }
        }

        private static void TextLengthChanged(DependencyObject obj,
                                              DependencyPropertyChangedEventArgs args)
        {
            var hasText = (int)args.NewValue > 0;
            SetHasText(obj, hasText);
        }

        private static void PasswordChanged(object sender, RoutedEventArgs args)
        {
            var passwordBox = sender as PasswordBox;
            if (passwordBox != null)
            {
                SetTextLength(passwordBox, passwordBox.Password.Length);
            }
        }
    }
}
