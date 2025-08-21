using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
    /// Interaction logic for AdminListOffers.xaml
    /// </summary>
    public partial class AdminListOffers : Window
    {
        AnnouncmentOffers an1;
        AdminListOffersVM addOfferViewModel = new AdminListOffersVM();
        string _name = " ";
        public AdminListOffers(string name)
        {
            InitializeComponent();
            _name = name;

            this.DataContext = addOfferViewModel;
            this.Listv.DataContext = addOfferViewModel;
        }

        private void ReturnB_Click(object sender, RoutedEventArgs e)
        {           
            bool access = true;
            try
            {
                AdminWindow adminWindow = new AdminWindow(_name);
                this.Close();
                adminWindow.Show();
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

        private void Listv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            if(this.Listv.SelectedItem is AnnouncmentOffers selectedOffer)
            {
                an1 = new AnnouncmentOffers();
                an1.id = selectedOffer.id;
                an1.id = selectedOffer.id;
                an1.nameprod = selectedOffer.nameprod;
                an1.nameprob = selectedOffer.nameprob;
                an1.typeprob = selectedOffer.typeprob;
                an1.state = selectedOffer.state;
                an1.urgency = selectedOffer.urgency;
                an1.worker_login = selectedOffer.worker_login;
                an1.deals = selectedOffer.deals;
                an1.price = selectedOffer.price;
            }
        }

        private void Accept_offer_Click(object sender, RoutedEventArgs e)
        {
            addOfferViewModel.AcceptOffer(an1);
        }

        private void Decline_offer_Click(object sender, RoutedEventArgs e)
        {
            addOfferViewModel.DeclineOffer(an1);
        }
    }
}
