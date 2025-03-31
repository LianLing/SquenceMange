using System.Windows;
using System.Windows.Controls;
using SquenceMange.Models;
using System.Collections.ObjectModel;
using SquenceMange.DataBase;

namespace SquenceMange.Views
{
    public partial class SaveRecordPage : Page
    {
        private readonly TagService _tagService = new TagService();
        public ObservableCollection<Tags> TagsList { get; } = new ObservableCollection<Tags>();

        public SaveRecordPage()
        {
            InitializeComponent();
            DataContext = this;
            LoadData();
        }

        private void LoadData(string keyword = "")
        {
            TagsList.Clear();
            foreach (var tag in _tagService.SearchTags(keyword))
            {
                TagsList.Add(tag);
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadData(searchBox.Text);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (tagsDataGrid.SelectedItem is Tags selected && selected.Id > 0)
            {
                if (_tagService.DeleteTag(selected.Id))
                {
                    TagsList.Remove(selected);
                }
            }
        }

        private void ShowAsDialog()
        {
            var window = new Window
            {
                Content = this,
                SizeToContent = SizeToContent.WidthAndHeight
            };
            window.ShowDialog();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}