using System.Linq;
using Windows.UI.Xaml.Controls;

namespace TrackSimulator
{
    public sealed partial class Dialog_EditDriver : ContentDialog
    {
        public Driver EditingDriver { get; set; }
        public Dialog_EditDriver(Driver driverToEdit)
        {
            this.InitializeComponent();
            EditingDriver = driverToEdit;
            PopulateFields();
        }

        private void PopulateFields()
        {
            FirstNameEntry.Text = EditingDriver.FirstName;
            LastNameEntry.Text = EditingDriver.LastName;
            DriverNumberEntry.Text = EditingDriver.DriverNumber;
            CityEntry.Text = EditingDriver.City;
            StateEntry.Text = EditingDriver.State;
            CarMakeEntry.Text = EditingDriver.Car_Make;
            CarModelEntry.Text = EditingDriver.Car_Model;
            CarYearEntry.Text = EditingDriver.Car_Year.ToString();
            ActiveEntry.IsChecked = EditingDriver.Active;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            ErrorText.Text = string.Empty;
            if (ValidateInputs())
            {
                EditingDriver.FirstName = FirstNameEntry.Text;
                EditingDriver.LastName = LastNameEntry.Text;
                EditingDriver.DriverNumber = DriverNumberEntry.Text;
                EditingDriver.City = CityEntry.Text;
                EditingDriver.State = StateEntry.Text;
                EditingDriver.Car_Make = CarMakeEntry.Text;
                EditingDriver.Car_Model = CarModelEntry.Text;
                EditingDriver.Active = ActiveEntry.IsChecked.Value;
                
                if (!string.IsNullOrWhiteSpace(CarYearEntry.Text))
                {
                    EditingDriver.Car_Year = int.Parse(CarYearEntry.Text);
                }
                _ = DBHelper.UpdateDriver(EditingDriver);
            }
            else
            {
                args.Cancel = true;
            }
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(FirstNameEntry.Text) || string.IsNullOrWhiteSpace(LastNameEntry.Text) || string.IsNullOrWhiteSpace(DriverNumberEntry.Text))
            {
                ErrorText.Text = "First Name, Last Name, and Driver Number are required.";
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Only allows digits to be entered for the car year
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void CarYearEntry_BeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            args.Cancel = args.NewText.Any(c => !char.IsDigit(c));
        }
    }
}
