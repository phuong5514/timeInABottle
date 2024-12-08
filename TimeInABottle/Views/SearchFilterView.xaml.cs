using System;
using System.Windows;
using Microsoft.UI.Xaml;

namespace TimeInABottle.Views
{
    public partial class SearchFilterView : Window
    {
        public SearchFilterView()
        {
            InitializeComponent();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve input values
            string keyword = SearchTextBox.Text;
            DateTime? startDate = StartDatePicker.SelectedDate;
            DateTime? endDate = EndDatePicker.SelectedDate;

            // Validate inputs
            if (string.IsNullOrWhiteSpace(keyword) && !startDate.HasValue && !endDate.HasValue)
            {
                MessageBox.Show("Please provide at least one filter (keyword or date).", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (startDate.HasValue && endDate.HasValue && startDate > endDate)
            {
                MessageBox.Show("Start date cannot be later than end date.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Perform search logic (placeholder)
            MessageBox.Show($"Searching for tasks with:\n\nKeyword: {keyword}\nStart Date: {startDate?.ToShortDateString()}\nEnd Date: {endDate?.ToShortDateString()}",
                            "Search Results", MessageBoxButton.OK, MessageBoxImage.Information);

            // Add actual search functionality here as needed
        }
    }
}
