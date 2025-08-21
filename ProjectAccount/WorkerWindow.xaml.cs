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

namespace ProjectAccount
{
    /// <summary>
    /// Interaction logic for WorkerWindow.xaml
    /// </summary>
    public partial class WorkerWindow : Window
    {
        string occupation = " ";
        string _login = " ";
        string _name = " ";
        public WorkerWindow(string name, string login)
        {
            InitializeComponent();
            occupation = AnnouncmentsViewModel.ReturnOccup(login);
            _login = login;
            _name = name;
            main_label.Content = "Welcome, " + _name;
            WorkListOffersVM workListOffersVM = new WorkListOffersVM(_login);
            this.Listv.DataContext = workListOffersVM;
        }

        private void ReturnB_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }

        private void Announcments_Click(object sender, RoutedEventArgs e)
        {         
            bool access = true;
            try
            {
                Announcments an = new Announcments(_name, _login, occupation);
                this.Close();
                an.Show();
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

        private void my_offers_Click(object sender, RoutedEventArgs e)
        {           
            bool access = true;
            try
            {
                WorkListOffers workListOffers = new WorkListOffers(_name, _login);
                this.Close();
                workListOffers.Show();
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

        private void Deals_Click(object sender, RoutedEventArgs e)
        {
            bool access = true;
            try
            {
                WorkersDeals workersDeals = new WorkersDeals(_name, _login);
                this.Close();
                workersDeals.Show();
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
