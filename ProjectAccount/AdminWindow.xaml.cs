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
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        string adminName = " ";
        public AdminWindow(string txt)
        {
            InitializeComponent();
            TableAn tableAn = new TableAn();
            this.Listv.DataContext = tableAn;
            adminName = txt;
            main_label.Content = "Welcome, " + adminName;
        }

        private void ReturnB_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }

        private void add_offer_Click(object sender, RoutedEventArgs e)
        {
            
            bool access = true;
            try
            {
                AddOffer addOffer = new AddOffer(adminName);
                addOffer.Show();
                this.Close();
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

        private void edit_offer_Click(object sender, RoutedEventArgs e)
        {
            
            bool access = true;
            try
            {
                EditOffer editOffer = new EditOffer(adminName);
                editOffer.Show();
                this.Close();
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

        private void remove_offer_Click(object sender, RoutedEventArgs e)
        {
            
            bool access = true;
            try
            {
                RemoveOffer removeOffer = new RemoveOffer(adminName);
                removeOffer.Show();
                this.Close();
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

        private void list_offers_Click(object sender, RoutedEventArgs e)
        {
           
            bool access = true;
            try
            {
                AdminListOffers adminListOffers = new AdminListOffers(adminName);
                adminListOffers.Show();
                this.Close();
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

        private void list_deals_Click(object sender, RoutedEventArgs e)
        {
            
            bool access = true;
            try
            {
                AdminWorkingSpace adminWorkingSpace = new AdminWorkingSpace(adminName);
                this.Close();
                adminWorkingSpace.Show();
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
