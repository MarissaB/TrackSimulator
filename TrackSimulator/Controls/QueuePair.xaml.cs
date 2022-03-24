using Microsoft.UI.Xaml.Controls;
using Windows.Globalization.NumberFormatting;
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
        public double LeftDial { get; set; }
        public double RightDial { get; set; }
        public QueuePair()
        {
            this.InitializeComponent();
            SetNumberBoxNumberFormatter();
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

        // TODO: Simplify SetDriverByNumber cases
        public void SetDriverByNumber(string driverNumber, bool isLeftLane, bool isBye)
        {
            if (isBye)
            {
                if (isLeftLane)
                {
                    LeftValid = true;
                    LeftDriver = new Driver();
                    LeftDriverFullName.Text = "BYE";
                }
                else
                {
                    RightValid = true;
                    RightDriver = new Driver();
                    RightDriverFullName.Text = "BYE";
                }
            }
            else
            {
                Driver driver = DBHelper.FindDriverByNumber(driverNumber);
                if (driver != null)
                {
                    if (isLeftLane)
                    {
                        LeftValid = true;
                        LeftDriver = driver;
                        LeftDriverFullName.Text = driver.FullName();
                    }
                    else
                    {
                        RightValid = true;
                        RightDriver = driver;
                        RightDriverFullName.Text = driver.FullName();
                    }
                }
            }
        }

        public void SetDialByLane(double dial, bool isLeftLane)
        {
            if (isLeftLane)
            {
                LeftDial = dial;
            }
            else
            {
                RightDial = dial;
            }
        }

        // TODO: Fix swap logic for bye runs. Something needs to be updated since swapping Byes is clunky and not working.
        private void Swap_Click(object sender, RoutedEventArgs e)
        {
            bool byeLeft = false;
            bool byeRight = false;
            if (LeftDriverNumberEntry.Text.ToUpper() == "BYE")
            {
                byeLeft = true;
            }
            if (RightDriverNumberEntry.Text.ToUpper() == "BYE")
            {
                byeRight = true;
            }
            string placeholderDriverNumber = LeftDriverNumberEntry.Text;
            
            SetDriverByNumber(RightDriverNumberEntry.Text, true, byeRight); // Set value of the Right to the Left
            SetDriverByNumber(placeholderDriverNumber, false, byeLeft); // Set value of the Left to the Right

            double placeholderDial = LeftDriverDial.Value;
            SetDialByLane(RightDriverDial.Value, true); // Set the value of the Right to the Left
            SetDialByLane(placeholderDial, false); // Set the value of the Left to the Right
        }

        private void LeftDriverNumberEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool setBye = false;
            if (LeftDriverNumberEntry.Text.ToUpper() == "BYE")
            {
                setBye = true;
            }
            SetDriverByNumber(LeftDriverNumberEntry.Text, true, setBye);
        }

        private void RightDriverNumberEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool setBye = false;
            if (RightDriverNumberEntry.Text.ToUpper() == "BYE")
            {
                setBye = true;
            }
            SetDriverByNumber(RightDriverNumberEntry.Text, false, setBye);
        }

        private void SetNumberBoxNumberFormatter()
        {
            DecimalFormatter formatter = new DecimalFormatter();
            formatter.IntegerDigits = 2;
            formatter.FractionDigits = 2;
            RightDriverDial.NumberFormatter = formatter;
            LeftDriverDial.NumberFormatter = formatter;
        }

        private void LeftDriverDial_ValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args)
        {
            SetDialByLane(LeftDriverDial.Value, true);
        }

        private void RightDriverDial_ValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args)
        {
            SetDialByLane(RightDriverDial.Value, false);
        }

        private void ClearLeft_Click(object sender, RoutedEventArgs e)
        {
            LeftDriverNumberEntry.Text = "BYE";
        }

        private void ClearRight_Click(object sender, RoutedEventArgs e)
        {
            RightDriverNumberEntry.Text = "BYE";
        }
    }
}
