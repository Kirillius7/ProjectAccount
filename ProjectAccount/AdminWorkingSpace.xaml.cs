using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for AdminWorkingSpace.xaml
    /// </summary>
    public partial class AdminWorkingSpace : Window
    {
        string _name = " ";
        public AdminWorkingSpace(string name)
        {
            InitializeComponent();
            AdminWorkingSpaceVM adminWorkingSpaceVM = new AdminWorkingSpaceVM();
            this.Listv.DataContext = adminWorkingSpaceVM;
            this.DataContext = adminWorkingSpaceVM;
            _name = name;
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

        private void OpenHistory_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("notepad.exe", "History.txt");
        }

        private void Deals_Click(object sender, RoutedEventArgs e)
        {
            ModelManager1.DataDeals();
            Process.Start("notepad.exe", "Deals.txt");
        }

        private void Diagram_Click(object sender, RoutedEventArgs e)
        {
            
            bool access = true;
            try
            {
                Diagrams aOffers = new Diagrams(_name);
                this.Close();
                aOffers.Show();
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

        private void Chart_Click(object sender, RoutedEventArgs e)
        {
            bool access = true;
            try
            {
                Chart woff = new Chart(_name);
                this.Close();
                woff.Show();
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
