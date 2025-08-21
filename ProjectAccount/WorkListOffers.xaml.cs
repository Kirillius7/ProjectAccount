using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProjectAccount
{
    /// <summary>
    /// Interaction logic for WorkListOffers.xaml
    /// </summary>
    public partial class WorkListOffers : Window
    {
        string _login = " ";
        int _id = 0;
        string _name = " ";
        WorkListOffersVM workListOffersVM;
        public WorkListOffers(string name, string login)
        {
            InitializeComponent();
            _login = login;
            _name = name;
            workListOffersVM = new WorkListOffersVM(login);
            this.Listv.DataContext = workListOffersVM;
            this.DataContext = workListOffersVM;
        }

        private void Listv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(this.Listv.SelectedItem is AnnouncmentOffers an) 
            {
                _id = an.id;
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

        private void Decline_offer_Click(object sender, RoutedEventArgs e)
        {
            workListOffersVM.DeclineOffer(_id);
        }
    }
}
