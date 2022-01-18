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
    public sealed partial class Dialog_NewDriver : ContentDialog
    {
        public Dialog_NewDriver()
        {
            this.InitializeComponent();
        }


        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            ErrorText.Text = string.Empty;
            if (ValidateInputs())
            {
                Driver newDriver = new Driver
                {
                    FirstName = FirstNameEntry.Text,
                    LastName = LastNameEntry.Text,
                    DriverNumber = DriverNumberEntry.Text,
                    City = CityEntry.Text,
                    State = StateEntry.Text,
                    Car_Make = CarMakeEntry.Text,
                    Car_Model = CarModelEntry.Text,
                    Active = true
                };
                if (!string.IsNullOrWhiteSpace(CarYearEntry.Text))
                {
                    newDriver.Car_Year = int.Parse(CarYearEntry.Text);
                }
                _ = DBHelper.CreateDriver(newDriver);
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
