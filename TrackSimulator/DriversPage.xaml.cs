using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System.Collections.ObjectModel;
using System;

namespace TrackSimulator
{
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

        private async void NewDriver_Click(object sender, RoutedEventArgs e)
        {
            Dialog_NewDriver dialog = new Dialog_NewDriver();
            _ = await dialog.ShowAsync();
        }

        private void RefreshSearchResults()
        {
            DriverDisplayTable.ItemsSource = null;
            DriverDisplayTable.ItemsSource = DriverTable;
        }

        private void EditDriver_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DriverDisplayTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DriverDisplayTable.SelectedItems.Count == 1)
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
