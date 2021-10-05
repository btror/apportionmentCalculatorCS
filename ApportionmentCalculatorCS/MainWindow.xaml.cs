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
using System.Diagnostics;

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
                state = 1,      
            };
            ApportionRowData.AddToList(newRow);
            DataGridXAML.ItemsSource = ApportionRowData.GetRowData();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var list = ApportionRowData.GetRowData();
            ApportionRow newRow = new ApportionRow()
            {
                state = list.Count + 1,
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
            list[0].population = 0;
            list[0].initialFairShare = 0;
            list[0].finalFairShare = 0;
            list[0].initialQuota = 0;
            list[0].finalQuota = 0;
            DataGridXAML.ItemsSource = null;
            DataGridXAML.ItemsSource = ApportionRowData.GetRowData();
        }

        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            var list = ApportionRowData.GetRowData();
            int seats = int.Parse(SeatsInput.Text); //(int)SeatsLabel.Content;
            int[] states = new int[list.Count];
            int[] populations = new int[list.Count];
            decimal[] initialQuotas = new decimal[list.Count];
            decimal[] finalQuotas = new decimal[list.Count];
            int[] initialFairShares = new int[list.Count];
            int[] finalFairShares = new int[list.Count];

            for (int i = 0; i < list.Count; i++)
            {
                states[i] = list[i].state;
                populations[i] = list[i].population;
            }

            string method = ComboBox.SelectedValue.ToString();

            if (method.Equals("System.Windows.Controls.ComboBoxItem: hamilton"))
            {
                var result = Hamilton.Calculate(seats, populations);
                initialQuotas = result.Item3;
                finalQuotas = result.Item4;
                initialFairShares = result.Item1;
                finalFairShares = result.Item2;
            } else if (method.Equals("System.Windows.Controls.ComboBoxItem: jefferson"))
            {
                var result = Jefferson.Calculate(seats, populations);
                initialQuotas = result.Item3;
                finalQuotas = result.Item4;
                initialFairShares = result.Item1;
                finalFairShares = result.Item2;
            } else if (method.Equals("System.Windows.Controls.ComboBoxItem: webster"))
            {
                var result = Webster.Calculate(seats, populations);
                initialQuotas = result.Item3;
                finalQuotas = result.Item4;
                initialFairShares = result.Item1;
                finalFairShares = result.Item2;
            } else if (method.Equals("System.Windows.Controls.ComboBoxItem: adam"))
            {
                var result = Adam.Calculate(seats, populations);
                initialQuotas = result.Item3;
                finalQuotas = result.Item4;
                initialFairShares = result.Item1;
                finalFairShares = result.Item2;
            } else if (method.Equals("System.Windows.Controls.ComboBoxItem: huntington hill"))
            {
                var result = HuntingtonHill.Calculate(seats, populations);
                initialQuotas = result.Item3;
                finalQuotas = result.Item4;
                initialFairShares = result.Item1;
                finalFairShares = result.Item2;
            }

            for (int i = 0; i < list.Count; i++)
            {
                list[i].initialFairShare = initialFairShares[i];
                list[i].finalFairShare = finalFairShares[i];
                list[i].initialQuota = initialQuotas[i];
                list[i].finalQuota = finalQuotas[i];
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
        public int state { get; set; }
        public int population { get; set; }
        public decimal initialQuota { get; set; }
        public decimal finalQuota { get; set; }
        public int initialFairShare { get; set; }
        public int finalFairShare { get; set; }
    }
}