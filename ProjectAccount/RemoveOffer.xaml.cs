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
using static System.Net.Mime.MediaTypeNames;

namespace ProjectAccount
{
    /// <summary>
    /// Interaction logic for RemoveOffer.xaml
    /// </summary>
    public partial class RemoveOffer : Window
    {
        string adminName = " ";
        public RemoveOffer(string txt)
        {
            InitializeComponent();

            //TableAn tableAn = new TableAn();
            //this.Listv.DataContext = tableAn;

            RemoveOfferViewModel offerViewModel = new RemoveOfferViewModel();
            this.DataContext = offerViewModel;
            this.Listv.DataContext = offerViewModel;
            adminName = txt;
        }

        private void ReturnB_Click(object sender, RoutedEventArgs e)
        {        
            bool access = true;
            try
            {
                AdminWindow adminWindow = new AdminWindow(adminName);
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

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            if (txt1.Text != " " )
            {
                txt1.Text = " ";
            }
            else
                MessageBox.Show("Your field isn't filled yet!");
        }
    }
}
