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
    /// Interaction logic for AddOffer.xaml
    /// </summary>
    public partial class AddOffer : Window
    {
        string adminName = " ";
        AddOfferViewModel addOfferViewModel = new AddOfferViewModel();
        public AddOffer(string txt)
        {
            InitializeComponent();
            adminName = txt;

            this.DataContext = addOfferViewModel;
            this.Listv.DataContext = addOfferViewModel;
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
            if(!access)
            {
                MainWindow mw = new MainWindow();
                this.Close();
                mw.Show();
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            if (txt1.Text != " " && txt2.Text != " " && cmbbox1.Text != " "
                && cmbbox2.Text != " " && cmbbox3.Text != " ")
            {
                txt1.Text = " ";
                txt2.Text = " ";
                cmbbox1.Text = " ";
                cmbbox2.Text = " ";
                cmbbox3.Text = " ";
            }
            else
                MessageBox.Show("Your fields aren't filled yet!");
        }
    }
}
