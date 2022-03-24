using System;
using Windows.UI.Xaml.Controls;

namespace TrackSimulator
{
    public sealed partial class Dialog_EditCategory : ContentDialog
    {
        public Category EditingCategory { get; set; }
        public Dialog_EditCategory(Category categoryToEdit)
        {
            this.InitializeComponent();
            EditingCategory = categoryToEdit;
            PopulateFields();
        }

        private void PopulateFields()
        {
            NameEntry.Text = EditingCategory.Name;
            LengthEntry.ItemsSource = Category.GetLengthOptions();
            LengthEntry.SelectedItem = EditingCategory.Length;
            QualifyingEntry.ItemsSource = Category.GetQualifyingOptions();
            QualifyingEntry.SelectedItem = EditingCategory.Qualifying;
            LadderEntry.ItemsSource = Category.GetLadderOptions();
            LadderEntry.SelectedItem = EditingCategory.Ladder;
            LightEntry.ItemsSource = Category.GetLightOptions();
            LightEntry.SelectedItem = EditingCategory.Light;
            ActiveEntry.IsChecked = EditingCategory.Active;
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

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            ErrorText.Text = string.Empty;
            if (ValidateInputs())
            {
                EditingCategory.Name = NameEntry.Text;
                EditingCategory.Length = Convert.ToInt32(LengthEntry.SelectedValue);
                EditingCategory.Qualifying = QualifyingEntry.SelectedValue.ToString();
                EditingCategory.Ladder = LadderEntry.SelectedValue.ToString();
                EditingCategory.Light = LightEntry.SelectedValue.ToString();
                EditingCategory.Active = ActiveEntry.IsChecked.Value;

                _ = DBHelper.UpdateCategory(EditingCategory);
            }
            else
            {
                args.Cancel = true;
            }
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
