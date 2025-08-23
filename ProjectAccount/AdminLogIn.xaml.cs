using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Prism.Commands;
using Prism.Mvvm;

namespace ProjectAccount
{
    /// <summary>
    /// Interaction logic for AdminLogIn.xaml
    /// </summary>
    public partial class AdminLogIn : Window
    {
        Admin_IdentificationVM ivm;
        public AdminLogIn()
        {
            ivm = new Admin_IdentificationVM();
            InitializeComponent();
            this.DataContext = ivm;
        }

        private void AccessB_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ivm.CheckUser())
                {
                    string name = ModelManager1.ReturnName(txt1.Text);
                    AdminWindow uw = new AdminWindow(name);
                    uw.Show();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            if ((txt1.Text != "" && pass1.Text != "") || (txt1.Text != "" && pass.Password != ""))
            {
                txt1.Text = "";
                pass1.Text = "";
                pass.Password = "";
            }
            else
                MessageBox.Show("Your fields aren't filled yet!");
        }

        private void ReturnB_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }

        //private void pass_PasswordChanged(object sender, RoutedEventArgs e)
        //{
        //    if(pass1.Text != pass.Password)
        //        pass1.Text = pass.Password;
        //}
        //private void pass1_PasswordChanged(object sender, RoutedEventArgs e)
        //{
        //    if (pass1.Text != pass.Password)
        //        pass.Password = pass1.Text;
        //}
        private void show_password_Click(object sender, RoutedEventArgs e)
        {
            pass1.Visibility = Visibility.Visible;
            if(pass1.Text != "")
                pass1.Text = pass.Password;
            show_password.Visibility = Visibility.Hidden;
            hide_password.Visibility = Visibility.Visible;
            
        }

        private void hide_password_Click(object sender, RoutedEventArgs e)
        {
            pass1.Visibility = Visibility.Hidden;
            if (pass.Password != "")
                pass.Password = pass1.Text;
            hide_password.Visibility = Visibility.Hidden;
            show_password.Visibility = Visibility.Visible;
        }
    }
}
