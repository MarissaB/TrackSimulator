using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TrackSimulator
{
    public sealed partial class RacingPage : Page
    {
        public List<string> CategoryNames { get; set; }
        public List<Category> Categories { get; set; }
        public bool RacingStarted { get; set; }
        public Category SelectedCategory { get; set; }
        public RacingPage()
        {
            this.InitializeComponent();
            CategoryNames = new List<string>();
            Categories = DBHelper.GetAllCategories();
            foreach (Category category in Categories)
            {
                CategoryNames.Add(category.ID + " | " + category.Name);
            }
            SetRacingState(false); // Start with the settings enabled and queue disabled.
        }

        private void SetRacingState(bool setState)
        {
            RacingStarted = setState;
            ToggleSettingsControls();
            if (RacingStarted)
            {
                StartRacing.Visibility = Visibility.Collapsed;
                StopRacing.Visibility = Visibility.Visible;
                Status.Text = "Racing has started. Enter vehicles in the queue and operate the races. Click 'Stop Racing' to change settings.";
            }
            else
            {
                StartRacing.Visibility = Visibility.Visible;
                StopRacing.Visibility = Visibility.Collapsed;
                Status.Text = "Racing has stopped. Change race settings above if needed. Click 'Start Racing' when finished to unlock the queue.";
            }
        }

        /// <summary>
        /// Enables/disables the controls for editing category/round/type
        /// </summary>
        private void ToggleSettingsControls()
        {
            CategoryList.IsEnabled = !RacingStarted;
            RoundInput.IsEnabled = !RacingStarted;
            ToggleTimeTrial.IsEnabled = !RacingStarted;
            ToggleElimination.IsEnabled = !RacingStarted;
            ToggleTimeslips.IsEnabled = !RacingStarted;
        }

        private void StartRacing_Click(object sender, RoutedEventArgs e)
        {
            SetRacingState(true);
        }

        private void StopRacing_Click(object sender, RoutedEventArgs e)
        {
            SetRacingState(false);
        }

        private void CategoryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Selected format example: 14 | Quick 16
            string selected = CategoryList.SelectedValue.ToString();
            int id = Convert.ToInt32(selected.Split("|")[0].Trim());
            SelectedCategory = Categories.First(category => category.ID == id);
        }
    }
}
