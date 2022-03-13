using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TrackSimulator.Controls
{
    public sealed partial class QueuePair : UserControl
    {
        public Category Category { get; set; }
        public Driver LeftDriver { get; set; }
        public Driver RightDriver { get; set; }
        public int Rank { get; set; }
        public bool LeftValid { get; set; }
        public bool RightValid { get; set; }
        public QueuePair()
        {
            this.InitializeComponent();
            LeftValid = false;
            RightValid = false;
        }

        /// <summary>
        /// Used for setting the rank order from the parent RacingPage
        /// </summary>
        /// <param name="rank"></param>
        public void UpdateRank(int rank)
        {
            Rank = rank;
            QueueRank.Text = rank.ToString();
        }

        public void SetDriverByNumber(string driverNumber, bool isLeftLane)
        {
            Driver driver = new Driver
            {
                DriverNumber = driverNumber
            };
            Driver foundDriver = DBHelper.SearchDrivers(driver, 1, false).FirstOrDefault();
            if (foundDriver != null)
            {
                if (isLeftLane)
                {
                    LeftValid = true;
                    LeftDriver = foundDriver;
                    LeftDriverFullName.Text = foundDriver.FullName();
                }
                else
                {
                    RightValid = true;
                    RightDriver = foundDriver;
                    RightDriverFullName.Text = foundDriver.FullName();
                }
            }
            else
            {
                if (isLeftLane)
                {
                    LeftValid = false;
                    LeftDriver = driver;
                    LeftDriverFullName.Text = "No driver found";
                }
                else
                {
                    RightValid = false;
                    RightDriver = driver;
                    RightDriverFullName.Text = "No driver found";
                }
            }
        }

        private void Swap_Click(object sender, RoutedEventArgs e)
        {
            Driver placeholder = LeftDriver;
            LeftDriver = RightDriver;
            RightDriver = placeholder;

            bool placeholderValid = LeftValid;
            LeftValid = RightValid;
            RightValid = placeholderValid;

            if (LeftValid)
            {
                LeftDriverNumberEntry.Text = LeftDriver.DriverNumber;
            }
            else
            {
                LeftDriverNumberEntry.Text = string.Empty;
            }
            if (RightValid)
            {
                RightDriverNumberEntry.Text = RightDriver.DriverNumber;
            }
            else
            {
                RightDriverNumberEntry.Text = string.Empty;
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LeftDriverNumberEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            SetDriverByNumber(LeftDriverNumberEntry.Text, true);
        }

        private void RightDriverNumberEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            SetDriverByNumber(RightDriverNumberEntry.Text, false);
        }
    }
}
