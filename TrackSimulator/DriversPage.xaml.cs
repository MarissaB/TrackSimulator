using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System;

namespace TrackSimulator
{
    public sealed partial class DriversPage : Page
    {
        public List<Driver> DriverTable { get; set; } = new List<Driver>();
        public DriversPage()
        {
            InitializeComponent();
            DriverTable = new List<Driver>();
            DriverTable = DBHelper.GetAllDrivers();
        }

        private void ResetSearchFields()
        {
            FirstName.Text = "";
            LastName.Text = "";
            DriverNumber.Text = "";
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            Driver findDriver = new Driver(FirstName.Text, LastName.Text, DriverNumber.Text);
            DriverTable = DBHelper.SearchDrivers(findDriver, IncludeInactives.IsChecked.Value);
            RefreshSearchResults();
        }

        private async void NewDriver_Click(object sender, RoutedEventArgs e)
        {
            Dialog_NewDriver dialog = new Dialog_NewDriver();
            ContentDialogResult contentDialogResult = await dialog.ShowAsync();
            if (contentDialogResult == ContentDialogResult.Primary)
            {
                ResetSearchFields();
                Search_Click(null, null);
            }
        }

        private void RefreshSearchResults()
        {
            DriverDisplayTable.ItemsSource = null;
            DriverDisplayTable.ItemsSource = DriverTable;
        }

        private async void EditDriver_Click(object sender, RoutedEventArgs e)
        {
            Dialog_EditDriver dialog = new Dialog_EditDriver((Driver)DriverDisplayTable.SelectedItem);
            ContentDialogResult contentDialogResult = await dialog.ShowAsync();
            if (contentDialogResult == ContentDialogResult.Primary)
            {
                ResetSearchFields();
                Search_Click(null, null);
            }
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
