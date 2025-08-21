using System;
using System.Collections;
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
    /// Interaction logic for EditOffer.xaml
    /// </summary>
    public partial class EditOffer : Window
    {
        string adminName = " ";
        public EditOffer(string txt)
        {
            InitializeComponent();

            //TableAn tableAn = new TableAn();
            //this.Listv.DataContext = tableAn;

            EditOfferViewModel offerViewModel = new EditOfferViewModel();
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
            if (txt1.Text != " " && txt2.Text != " " && txt3.Text != " " && cmbbox1.Text != " "
                && cmbbox2.Text != " " && cmbbox3.Text != " ")
            {
                txt1.Text = " ";
                txt2.Text = " ";
                txt3.Text = " ";
                cmbbox1.Text = " ";
                cmbbox2.Text = " ";
                cmbbox3.Text = " ";
            }
            else
                MessageBox.Show("Your fields aren't filled yet!");
        }
    }
}
