using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using System.Windows.Input;

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
            Output.Content = "";
        }

        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            var list = ApportionRowData.GetRowData();
            int seats = 0;
            try
            {
                seats = int.Parse(SeatsInput.Text); // System.FormatException: 
            } catch (System.FormatException)
            {
                SeatsInput.Text = "";
            }

            int populationTotal = 0;
            for (int i = 0; i < list.Count; i++)
            {
                populationTotal += list[i].population;
            }

            if (seats > 0 && populationTotal > 0)
            {
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
                    Output.Content = "Divisor is " + result.Item6;
                }
                else if (method.Equals("System.Windows.Controls.ComboBoxItem: jefferson"))
                {
                    var result = Jefferson.Calculate(seats, populations);
                    initialQuotas = result.Item3;
                    finalQuotas = result.Item4;
                    initialFairShares = result.Item1;
                    finalFairShares = result.Item2;
                    Output.Content = "Original divisor is " + result.Item5 + "\nModified divisor is " + result.Item6;
                }
                else if (method.Equals("System.Windows.Controls.ComboBoxItem: webster"))
                {
                    var result = Webster.Calculate(seats, populations);
                    initialQuotas = result.Item3;
                    finalQuotas = result.Item4;
                    initialFairShares = result.Item1;
                    finalFairShares = result.Item2;
                    Output.Content = "Original divisor is " + result.Item5 + "\nModified divisor is " + result.Item6;
                }
                else if (method.Equals("System.Windows.Controls.ComboBoxItem: adam"))
                {
                    var result = Adam.Calculate(seats, populations);
                    initialQuotas = result.Item3;
                    finalQuotas = result.Item4;
                    initialFairShares = result.Item1;
                    finalFairShares = result.Item2;
                    Output.Content = "Original divisor is " + result.Item5 + "\nModified divisor is " + result.Item6;
                }
                else if (method.Equals("System.Windows.Controls.ComboBoxItem: huntington hill"))
                {
                    var result = HuntingtonHill.Calculate(seats, populations);
                    initialQuotas = result.Item3;
                    finalQuotas = result.Item4;
                    initialFairShares = result.Item1;
                    finalFairShares = result.Item2;
                    Output.Content = "Original divisor is " + result.Item5 + "\nModified divisor is " + result.Item6;
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
            } else
            {
                string output = "Errors detected...";
                if (seats < 1)
                {
                    output += "\n- Number of seats must be greater than 0.";
                }

                if (populationTotal < 1)
                {
                    output += "\n- Total population must be greater than 0.";
                }
                Output.Content = output;
            }
            
      
        }

        private void Seats(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
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