using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
namespace ProjectAccount
{
    internal class PasswordBoxBehavior : Behavior<PasswordBox>
    {
        public static readonly DependencyProperty BoundPasswordProperty =
            DependencyProperty.Register("BoundPassword", typeof(string), typeof(PasswordBoxBehavior), new PropertyMetadata(string.Empty, OnBoundPasswordChanged));

        public string BoundPassword
        {
            get { return (string)GetValue(BoundPasswordProperty); }
            set { SetValue(BoundPasswordProperty, value); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PasswordChanged += OnPasswordChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PasswordChanged -= OnPasswordChanged;
        }

        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            BoundPassword = AssociatedObject.Password;
        }

        //private static void OnBoundPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    var behavior = d as PasswordBoxBehavior;
        //    var passwordBox = behavior?.AssociatedObject;

        //    if (passwordBox == null)
        //        return;

        //    var newPassword = (string)e.NewValue;

        //    if (passwordBox.Password != newPassword)
        //    {
        //        passwordBox.Password = newPassword;
        //    }
        //}
        private static void OnBoundPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var behavior = d as PasswordBoxBehavior;
            var passwordBox = behavior?.AssociatedObject;
            if (passwordBox == null) return;

            var newPassword = (string)e.NewValue;

            if (!passwordBox.IsLoaded)
            {
                passwordBox.Loaded += (s, args) =>
                {
                    if (passwordBox.Password != newPassword)
                        passwordBox.Password = newPassword;
                };
                return;
            }

            if (passwordBox.Password != newPassword)
            {
                passwordBox.Password = newPassword;
            }
        }
    }
}
