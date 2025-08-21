using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace ProjectAccount
{
    /// <summary>
    /// Interaction logic for WOffers.xaml
    /// </summary>
    public partial class Chart : Window
    {
        public SeriesCollection seriesCollection { get; set; }
        public string[] Labs { get; set; }
        private string _name = " ";
        public Chart(string name)
        {
            InitializeComponent();
            AdminWorkingSpaceVM adminWorkingSpace = new AdminWorkingSpaceVM();

            seriesCollection = new SeriesCollection();

            var jobs = adminWorkingSpace._announcments.DistinctBy(i => i.typeprob).Select(i => i.typeprob);

            foreach (var _job in jobs)
            {
                PieSeries pieSeries1 = new PieSeries
                {
                    Title = _job + " -> spent ",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(Convert.ToDouble
                    (adminWorkingSpace._announcments.Where(i => i.typeprob == _job).Sum(i => i.price)))}
                };

                seriesCollection.Add(pieSeries1);
            }

            Labs = adminWorkingSpace._announcments.Select(item => item.id.ToString()).ToArray();

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

        private void PieChart_DataClick(object sender, ChartPoint chartPoint)
        {
            MessageBox.Show("Money spent: " + chartPoint.Y);
        }
    }
}
