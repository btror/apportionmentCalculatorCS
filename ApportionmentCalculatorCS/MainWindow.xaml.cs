using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Linq;

namespace ApportionmentCalculatorNET
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            // Create a default row for the datagrid.
            var list = ApportionRowData.GetRowData();
            Console.WriteLine(list);
            ApportionRow newRow = new ApportionRow()
            {
                state = 1,      
            };
            ApportionRowData.AddToList(newRow);
            DataGridXAML.ItemsSource = ApportionRowData.GetRowData();
        }

        /// <summary>
        /// Adds a new row to the datagrid. 
        /// </summary>
        /// <param name="sender">Reference to the add button.</param>
        /// <param name="e">Event data.</param>
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var list = ApportionRowData.GetRowData();
            ApportionRow newRow = new ApportionRow()
            {
                state = list.Count + 1,
            };

            // Clear all entries except the populations and name field.
            for (int i = 0; i < list.Count; i++)
            {
                list[i].initialFairShare = "";
                list[i].finalFairShare = "";
                list[i].initialQuota = "";
                list[i].finalQuota = "";
            }
            list[0].initialFairShare = "";
            list[0].finalFairShare = "";
            list[0].initialQuota = "";
            list[0].finalQuota = "";

            // Add a new row to the datagrid. 
            ApportionRowData.AddToList(newRow);
            DataGridXAML.ItemsSource = null;
            DataGridXAML.ItemsSource = ApportionRowData.GetRowData();
        }

        /// <summary>
        /// Removes an existing row from the datagrid. 
        /// </summary>
        /// <param name="sender">Reference to the remove button.</param>
        /// <param name="e">Event data.</param>
        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            var list = ApportionRowData.GetRowData();
            if (list.Count > 1)
            {
                list.RemoveAt(list.Count - 1);

                // Clear all entries except the populations and name field.
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].initialFairShare = "";
                    list[i].finalFairShare = "";
                    list[i].initialQuota = "";
                    list[i].finalQuota = "";
                }
                list[0].initialFairShare = "";
                list[0].finalFairShare = "";
                list[0].initialQuota = "";
                list[0].finalQuota = "";

                DataGridXAML.ItemsSource = null;
                DataGridXAML.ItemsSource = ApportionRowData.GetRowData();
            }
        }

        /// <summary>
        /// Removes all existing rows from the datagrid. 
        /// </summary>
        /// <param name="sender">Reference to the clear button.</param>
        /// <param name="e">Event data.</param>
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            var list = ApportionRowData.GetRowData();
            for (int i = list.Count - 1; i > 0; i--)
            {
                list.RemoveAt(i);
                DataGridXAML.ItemsSource = null;
                DataGridXAML.ItemsSource = ApportionRowData.GetRowData();
            }

            // Clear the data from the columns in the default row.
            list[0].population = "";
            list[0].initialFairShare = "";
            list[0].finalFairShare = "";
            list[0].initialQuota = "";
            list[0].finalQuota = "";
            list[0].nickname = "";
            DataGridXAML.ItemsSource = null;
            DataGridXAML.ItemsSource = ApportionRowData.GetRowData();
            Output.Content = "";
        }

        /// <summary>
        /// Calculates results (apportions seats to states, calculates fair shares and quotas, etc).
        /// </summary>
        /// <param name="sender">Reference to the calculate button.</param>
        /// <param name="e">Event data.</param>
        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            var list = ApportionRowData.GetRowData();

            // Collect the amount of seats.
            int seats = 0;
            try
            {
                seats = int.Parse(SeatsInput.Text);
            } catch (System.FormatException)
            {
                SeatsInput.Text = "";
            }

            // Collect the total population of all states.
            int populationTotal = 0;
            bool lettersDetected = false;
            for (int i = 0; i < list.Count; i++)
            {
                try
                {
                    populationTotal += int.Parse(list[i].population);
                } catch (Exception ex)
                {
                    if (ex is System.FormatException || ex is System.ArgumentNullException)
                    {
                        lettersDetected = true;
                    }
                    break;
                } 
            }

            // Make sure there is a positive number of seats to assign and a population to assign them to.
            if (seats > 0 && populationTotal > 0 && !lettersDetected)
            {
                // Collect the data required for apportionment.
                int[] states = new int[list.Count];
                int[] populations = new int[list.Count];
                decimal[] initialQuotas = new decimal[list.Count];
                decimal[] finalQuotas = new decimal[list.Count];
                int[] initialFairShares = new int[list.Count];
                int[] finalFairShares = new int[list.Count];

                // Create lists for eeach state's population.
                for (int i = 0; i < list.Count; i++)
                {
                    states[i] = list[i].state;
                    populations[i] = int.Parse(list[i].population);
                }

                // Calculate the results depending on the selected apportionment method.
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

                // Update the values in the datagrid to reflect the final calculations.
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].initialFairShare = initialFairShares[i] + "";
                    list[i].finalFairShare = finalFairShares[i] + "";
                    list[i].initialQuota = initialQuotas[i] + "";
                    list[i].finalQuota = finalQuotas[i] + "";
                }
  
                DataGridXAML.ItemsSource = null;
                DataGridXAML.ItemsSource = ApportionRowData.GetRowData();
            } 
            
            // If the user input is invalid, display the specific issue to the GUI. 
            else
            {
                string output = "Errors detected...";

                // Not enought seats to apportion.
                if (seats < 1)
                {
                    output += "\n- Number of seats must be greater than 0.";
                }

                // No population to assign seats to.
                if (populationTotal < 1)
                {
                    output += "\n- Total population must be greater than 0.";
                }

                // Letters detected in input fields.
                if (lettersDetected)
                {
                    output += "\n- Invalid input detected in population fields.";
                }

                Output.Content = output;
            }
        }

        /// <summary>
        /// Formats input for the seats field (removes letters and special characters). 
        /// </summary>
        /// <param name="sender">Reference to the seats textbox.</param>
        /// <param name="e">Event data.</param>
        private void Seats(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }

    /// <summary>
    /// Object containing data for each row in the datagrid. 
    /// </summary>
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

    /// <summary>
    /// Object containing data for each column of each row in the datagrid. 
    /// </summary>
    public class ApportionRow
    {
        public string nickname { get; set; }
        public int state { get; set; }
        public string population { get; set; }
        public string initialQuota { get; set; }
        public string finalQuota { get; set; }
        public string initialFairShare { get; set; }
        public string finalFairShare { get; set; }
    }
}