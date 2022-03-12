using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace TrackSimulator
{
    public sealed partial class ReportsPage : Page
    {
        public List<Run> RunTable { get; set; }
        public List<string> CategoryNames { get; set; }
        public ReportsPage()
        {
            this.InitializeComponent();
            RunTable = new List<Run>();
            RunTable = DBHelper.GetRecentRuns();
            CategoryNames = new List<string>();
            StartDateInput.Date = DateTime.Now.AddDays(-1);
            EndDateInput.Date = DateTime.Now;
            ToggleViewAll.IsChecked = true;

            List<Category> categoryList = DBHelper.GetAllCategories();
            foreach (Category category in categoryList)
            {
                CategoryNames.Add(category.ID + " | " + category.Name);
            }
        }

        private void RefreshSearchResults()
        {
            RunDisplayTable.ItemsSource = null;
            RunDisplayTable.ItemsSource = RunTable;
        }

        private void EditRun_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GenerateRace_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SetPastDay_Click(object sender, RoutedEventArgs e)
        {
            EndDateInput.Date = DateTime.Now;
            StartDateInput.Date = DateTime.Now.AddDays(-1);
        }

        private void SetPastMonth_Click(object sender, RoutedEventArgs e)
        {
            EndDateInput.Date = DateTime.Now;
            StartDateInput.Date = DateTime.Now.AddMonths(-1);
        }

        private void SetPastYear_Click(object sender, RoutedEventArgs e)
        {
            EndDateInput.Date = DateTime.Now;
            StartDateInput.Date = DateTime.Now.AddYears(-1);
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RunDisplayTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
