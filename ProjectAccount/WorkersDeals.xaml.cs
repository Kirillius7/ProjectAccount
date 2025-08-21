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
using System.Xml.Linq;

namespace ProjectAccount
{
    /// <summary>
    /// Interaction logic for WorkersDeals.xaml
    /// </summary>
    public partial class WorkersDeals : Window
    {
        string _name = " ";
        string _login = " ";
        public WorkersDeals(string name, string login)
        {
            InitializeComponent();
            _name = name;
            _login = login;
            WorkersDealsVM workersDealsVM = new WorkersDealsVM(_login);
            this.Listv.DataContext = workersDealsVM;
            this.DataContext = workersDealsVM;
        }

        private void ReturnB_Click(object sender, RoutedEventArgs e)
        {
            
            bool access = true;
            try
            {
                WorkerWindow workerWindow = new WorkerWindow(_name, _login);
                this.Close();
                workerWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                access = false;
            }
            if (!access)
            {
                MainWindow mw = new MainWindow();
                this.Close();
                mw.Show();
            }
        }
    }
}
