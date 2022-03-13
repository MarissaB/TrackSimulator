using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace TrackSimulator
{
    public sealed partial class RacingPage : Page
    {
        public List<string> CategoryNames { get; set; }
        public List<Category> Categories { get; set; }
        public bool RacingStarted { get; set; }
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
            EliminationToggle.IsEnabled = !RacingStarted;
        }

        private void StartRacing_Click(object sender, RoutedEventArgs e)
        {
            SetRacingState(true);
        }

        private void StopRacing_Click(object sender, RoutedEventArgs e)
        {
            SetRacingState(false);
        }
    }
}
