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

        private void NewCategory_Click(object sender, RoutedEventArgs e)
        {
            Dialog_NewCategory dialog = new Dialog_NewCategory();
            _ = dialog.ShowAsync();
        }

        private void EditCategory_Click(object sender, RoutedEventArgs e)
        {

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
