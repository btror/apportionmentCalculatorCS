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

            // default row
            var list = ApportionRowData.GetRowData();
            Console.WriteLine(list);
            ApportionRow newRow = new ApportionRow()
            {
                state = "1",
                population = "",
                initialQuota = "",
                finalQuota = "",
                initialFairShare = "",
                finalFairShare = "",
            };
            ApportionRowData.AddToList(newRow);
            DataGridXAML.ItemsSource = ApportionRowData.GetRowData();


        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var list = ApportionRowData.GetRowData();
            ApportionRow newRow = new ApportionRow()
            {
                state = "" + (list.Count + 1),
                population = "",
                initialQuota = "",
                finalQuota = "",
                initialFairShare = "",
                finalFairShare = "",
            };
            ApportionRowData.AddToList(newRow);
            DataGridXAML.ItemsSource = null;
            DataGridXAML.ItemsSource = ApportionRowData.GetRowData();
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            var list = ApportionRowData.GetRowData();
            if (list.Count > 1)
            {
                list.RemoveAt(list.Count - 1);
                DataGridXAML.ItemsSource = null;
                DataGridXAML.ItemsSource = ApportionRowData.GetRowData();
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            var list = ApportionRowData.GetRowData();
            for (int i = list.Count - 1; i > 0; i--)
            {
                list.RemoveAt(i);
                DataGridXAML.ItemsSource = null;
                DataGridXAML.ItemsSource = ApportionRowData.GetRowData();
            }
            DataGridXAML.ItemsSource = null;
            DataGridXAML.ItemsSource = ApportionRowData.GetRowData();
        }
    }

    public class ApportionRowData
    {
        private static List<ApportionRow> list = new List<ApportionRow>();
        public static List<ApportionRow> GetRowData()
        {
            return list;
        }

        public static void AddToList(ApportionRow newRow)
        {
            list.Add(newRow);
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