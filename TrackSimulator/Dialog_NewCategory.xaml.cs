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

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace TrackSimulator
{
    public sealed partial class Dialog_NewCategory : ContentDialog
    {
        public Dialog_NewCategory()
        {
            this.InitializeComponent();
        }

        private void PopulateFields()
        {
            List<int> lengths = new List<int>();
            lengths.Add(1320);
            lengths.Add(990);
            lengths.Add(660);
            lengths.Add(330);
            lengths.Add(60);
            LengthEntry.ItemsSource = lengths;

            List<string> qualifyingTypes = new List<string>();
            qualifyingTypes.Add("Best Elapsed Time");
            qualifyingTypes.Add("Best Reaction Time");
            QualifyingEntry.ItemsSource = qualifyingTypes;

            List<string> lights = new List<string>();
            lights.Add("Sportsman");
            lights.Add("Pro");

            List<string> ladderTypes = new List<string>();
            ladderTypes.Add("Best Elapsed Time");
            ladderTypes.Add("Best Reaction Time");
            LadderEntry.ItemsSource = ladderTypes;
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
            if (string.IsNullOrWhiteSpace(NameEntry.Text) || LengthEntry.SelectedIndex < 0 || QualifyingEntry.SelectedIndex < 0
                || LadderEntry.SelectedIndex < 0 || LightEntry.SelectedIndex < 0)
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
