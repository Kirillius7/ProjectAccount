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
    /// Interaction logic for Announcments.xaml
    /// </summary>
    public partial class Announcments : Window
    {
        string _login = " ";
        string _type = " ";
        string _name = " ";
        AnnouncmentsViewModel announcmentsViewModel;
        public Announcments(string name, string login, string type)
        {
            InitializeComponent();
            _login = login;
            _type = type;
            _name = name;
            announcmentsViewModel = new AnnouncmentsViewModel(login, type);
            this.Listv.DataContext = announcmentsViewModel;
            this.DataContext = announcmentsViewModel;
            
        }

        Announcment wOffers = new Announcment();

        private void Listv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Listv.SelectedItem is Announcment selectedCommodity)
            {
                wOffers.id = selectedCommodity.id;
                wOffers.nameprod = selectedCommodity.nameprod;
                wOffers.nameprob = selectedCommodity.nameprob;
                wOffers.typeprob = selectedCommodity.typeprob;
                wOffers.state = selectedCommodity.state;
                wOffers.urgency = selectedCommodity.urgency;
            }
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

        private void Accept_offer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int deals = ModelWorker2.ReturnDeals(_login);
                decimal price = Convert.ToDecimal(txt1.Text);
                announcmentsViewModel.AcceptOffer(wOffers, _login, deals, price);
            }
            catch
            {
                MessageBox.Show("Incorrect data! Try again");
            }
            finally
            {
                wOffers = new Announcment();
            }
        }
    }
}
