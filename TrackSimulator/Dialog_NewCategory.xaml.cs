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
    public sealed partial class Dialog_NewCategory : ContentDialog
    {
        public Dialog_NewCategory()
        {
            this.InitializeComponent();
            PopulateFields();
        }

        private void PopulateFields()
        {
            LengthEntry.ItemsSource = Category.GetLengthOptions();
            LengthEntry.SelectedIndex = 0;

            
            QualifyingEntry.ItemsSource = Category.GetQualifyingOptions();
            QualifyingEntry.SelectedIndex = 0;

            LightEntry.ItemsSource = Category.GetLightOptions();
            LightEntry.SelectedIndex = 0;


            LadderEntry.ItemsSource = Category.GetLadderOptions();
            LadderEntry.SelectedIndex = 0;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            ErrorText.Text = string.Empty;
            if (ValidateInputs())
            {
                Category newCategory = new Category
                {
                    Name = NameEntry.Text,
                    Length = Convert.ToInt32(LengthEntry.SelectedValue),
                    Qualifying = QualifyingEntry.SelectedValue.ToString(),
                    Ladder = LadderEntry.SelectedValue.ToString(),
                    Light = LightEntry.SelectedValue.ToString(),
                    Active = true

                };
                _ = DBHelper.CreateCategory(newCategory);
            }
            else
            {
                args.Cancel = true;
            }
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(NameEntry.Text))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
