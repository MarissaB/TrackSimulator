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
    public sealed partial class Dialog_EditDriver : ContentDialog
    {
        public Driver EditingDriver { get; set; }
        public Dialog_EditDriver(Driver driverToEdit)
        {
            this.InitializeComponent();
            EditingDriver = driverToEdit;
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
