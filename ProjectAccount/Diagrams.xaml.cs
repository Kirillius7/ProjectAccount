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
using LiveCharts;
using LiveCharts.Wpf;

namespace ProjectAccount
{
    /// <summary>
    /// Interaction logic for AOffers.xaml
    /// </summary>
    public partial class Diagrams : Window
    {
        public SeriesCollection seriesCollection { get; set;}
        public string[]Labs { get; set; }
        private string _name = " ";
        public Diagrams(string name)
        {
            InitializeComponent();
            AdminWorkingSpaceVM adminWorkingSpace = new AdminWorkingSpaceVM();
            
            seriesCollection = new SeriesCollection();

            LineSeries lineSeries = new LineSeries
            {
                Title = "Price",
                Values = new ChartValues<decimal>(adminWorkingSpace._announcments.Select(item => item.price))
            };

            Axis x = new Axis
            {
                Title = "ID",
            };

            Labs = adminWorkingSpace._announcments.Select(item => item.id.ToString()).ToArray();
            seriesCollection.Add(lineSeries);
            DataContext = this;
            this._name = name;
        }

        private void ReturnB_Click(object sender, RoutedEventArgs e)
        {
            bool access = true;
            try
            {            
                AdminWorkingSpace adminWorkingSpace = new AdminWorkingSpace(_name);
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
