using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System.Collections.ObjectModel;

namespace TrackSimulator
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DriversPage : Page
    {
        public List<Driver> DriverTable { get; set; }
        public DriversPage()
        {
            InitializeComponent();
            DriverTable = new List<Driver>();
            DriverTable = DBHelper.GetAllDrivers();
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            Driver findDriver = new Driver(FirstName.Text, LastName.Text, RaceNumber.Text);
            DriverTable = DBHelper.SearchDrivers(findDriver, IncludeInactives.IsChecked.Value);
            RefreshSearchResults();
        }

        private void NewDriver_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RefreshSearchResults()
        {
            SearchResults.ItemsSource = null;
            SearchResults.ItemsSource = DriverTable;
        }

        private void EditDriver_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SearchResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SearchResults.SelectedItems.Count == 1)
            {
                EditDriver.IsEnabled = true;
            }
            else
            {
                EditDriver.IsEnabled = false;
            }
        }
    }
}
