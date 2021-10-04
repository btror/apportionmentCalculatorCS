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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ApportionmentCalculatorNET
{
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            DataGridXAML.ItemsSource = ApportionRowData.GetRowData();


        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var list = ApportionRowData.GetRowData();
            Console.WriteLine(list);
            ApportionRow newRow = new ApportionRow()
            {
                state = "x",
                population = "",
                initialQuota = "",
                finalQuota = "",
                initialFairShare = "",
                finalFairShare = "",
            };
            list.Add(newRow);
            DataGridXAML.ItemsSource = list;
        }
    }

    public class ApportionRowData
    {

        public static List<ApportionRow> GetRowData()
        {
            var list = new List<ApportionRow>();
            ApportionRow newRow = new ApportionRow()
            {
                state = "1",
                population = "",
                initialQuota = "",
                finalQuota = "",
                initialFairShare = "",
                finalFairShare = "",
            };
            list.Add(newRow);
            return list;
        }
    }

    public class ApportionRow
    {
        public string state { get; set; }
        public string population { get; set; }
        public string initialQuota { get; set; }
        public string finalQuota { get; set; }
        public string initialFairShare { get; set; }
        public string finalFairShare { get; set; }
    }
}
