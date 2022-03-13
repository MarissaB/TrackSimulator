using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls;
using TrackSimulator.Controls;

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
            CategoryList.SelectedIndex = 0;
            AddQueuePair();
            AddQueuePair();
            AddQueuePair();
            AddQueuePair();
        }

        private void SetRacingState(bool setState)
        {
            RacingStarted = setState;
            ToggleSettingsControls();
            if (RacingStarted)
            {
                StartRacing.Visibility = Visibility.Collapsed;
                StopRacing.Visibility = Visibility.Visible;
                PushPair.IsEnabled = true;
                Status.Text = "Racing has started. Enter vehicles in the queue and operate the races. Click 'Stop Racing' to change settings.";
            }
            else
            {
                StartRacing.Visibility = Visibility.Visible;
                StopRacing.Visibility = Visibility.Collapsed;
                PushPair.IsEnabled = false;
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

        private void RoundInput_ValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args)
        {
            if (RoundInput.Value < 1)
            {
                RoundInput.Value = 1;
            }
        }

        /// <summary>
        /// Creates a new QueuePair at the end of the rank list and moves the others up
        /// </summary>
        private void AddQueuePair()
        {
            QueuePair pair = new QueuePair();
            pair.Category = SelectedCategory;
            int maxPairs = 4;
            foreach (QueuePair queuePair in QueueWrapper.Children)
            {
                int rank = queuePair.Rank - 1;
                queuePair.UpdateRank(rank);
            }
            pair.UpdateRank(maxPairs);
            QueueWrapper.Children.Add(pair);
        }

        /// <summary>
        /// Pushes the top pair to the starting line
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PushPair_Click(object sender, RoutedEventArgs e)
        {
            QueuePair firstPair = (QueuePair)QueueWrapper.Children[0];
            if (firstPair.LeftValid && firstPair.RightValid)
            {
                LeftDriver.Text = firstPair.LeftDriver.FullName();
                RightDriver.Text = firstPair.RightDriver.FullName();
                QueueWrapper.Children.RemoveAt(0);
                AddQueuePair();
            }
        }
    }
}
