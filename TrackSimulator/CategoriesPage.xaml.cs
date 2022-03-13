using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace TrackSimulator
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CategoriesPage : Page
    {
        public List<Category> CategoryTable { get; set; }
        public CategoriesPage()
        {
            this.InitializeComponent();
            CategoryTable = new List<Category>();
            CategoryTable = DBHelper.GetAllCategories();
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            Category findCategory = new Category();
            findCategory.Name = CategoryName.Text;
            CategoryTable = DBHelper.SearchCategories(findCategory, IncludeInactives.IsChecked.Value);
            RefreshSearchResults();
        }

        private async void NewCategory_Click(object sender, RoutedEventArgs e)
        {
            Dialog_NewCategory dialog = new Dialog_NewCategory();
            ContentDialogResult contentDialogResult = await dialog.ShowAsync();
            if (contentDialogResult == ContentDialogResult.Primary)
            {
                CategoryName.Text = "";
                Search_Click(null, null);
            }
        }

        private async void EditCategory_Click(object sender, RoutedEventArgs e)
        {
            Dialog_EditCategory dialog = new Dialog_EditCategory((Category)CategoryDisplayTable.SelectedItem);
            ContentDialogResult contentDialogResult = await dialog.ShowAsync();
            if (contentDialogResult == ContentDialogResult.Primary)
            {
                CategoryName.Text = "";
                Search_Click(null, null);
            }
        }

        private void RefreshSearchResults()
        {
            CategoryDisplayTable.ItemsSource = null;
            CategoryDisplayTable.ItemsSource = CategoryTable;
        }

        private void CategoryDisplayTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CategoryDisplayTable.SelectedItems.Count == 1)
            {
                EditCategory.IsEnabled = true;
            }
            else
            {
                EditCategory.IsEnabled = false;
            }
        }
    }
}
