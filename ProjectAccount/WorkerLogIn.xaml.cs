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
using System.Globalization;

namespace ProjectAccount
{
    /// <summary>
    /// Interaction logic for WorkerLogIn.xaml
    /// </summary>
    public partial class WorkerLogIn : Window
    {
        Worker_IdentificationVM ivm = new Worker_IdentificationVM();
        public WorkerLogIn()
        {
            InitializeComponent();
            this.DataContext = ivm;
        }

        private void AccessB_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ivm.CheckUser())
                {
                    string name = ModelWorker2.ReturnName(txt1.Text);
                    WorkerWindow uw = new WorkerWindow(name, txt1.Text);
                    uw.Show();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
            WorkerSignUp workerSignUp = new WorkerSignUp();
            this.Close();
            workerSignUp.Show();
        }

        private void ReturnB_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            this.Close();
            mainWindow.Show();
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

        private void show_password_Click(object sender, RoutedEventArgs e)
        {
            pass1.Visibility = Visibility.Visible;
            if (pass1.Text != "")
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
        private void pass_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (pass1.Text != pass.Password)
                pass1.Text = pass.Password;
        }
        private void pass1_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (pass1.Text != pass.Password)
                pass.Password = pass1.Text;
        }
    }
}
