using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProjectAccount
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void WLogIn_Click(object sender, RoutedEventArgs e)
        {
            WorkerLogIn wLogIn = new WorkerLogIn();
            wLogIn.Show();
            this.Close();
        }

        private void WSignUp_Click(object sender, RoutedEventArgs e)
        {
            WorkerSignUp wSignUp = new WorkerSignUp();
            wSignUp.Show();
            this.Close();
        }

        private void aLogIn_Click(object sender, RoutedEventArgs e)
        {
            AdminLogIn aLogIn = new AdminLogIn();
            aLogIn.Show();
            this.Close();
        }
    }
}