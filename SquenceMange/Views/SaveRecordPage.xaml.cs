using System.Windows;
using System.Windows.Controls;
using SquenceMange.Models;
using System.Collections.ObjectModel;
using SquenceMange.Service;
using SqlSugar;
using SquenceMange.DataBase;

namespace SquenceMange.Views
{
    public partial class SaveRecordPage : Page
    {
        private readonly TagService _tagService = new TagService();
        public ObservableCollection<TagsModel> TagsList { get; } = new ObservableCollection<TagsModel>();

        public SaveRecordPage()
        {
            InitializeComponent();
            Loaded += (s, e) =>
            {
                if (!IsInitialized)
                {
                    // 初始化数据加载
                    LoadData();
                    IsInitialized = true;
                }
            };
        }
        private bool IsInitialized = false;

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
            if (tagsDataGrid.SelectedItem is TagsModel selected && selected.Id > 0)
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
            try
            {
                // 从TextBox获取搜索条件
                string searchKeyword = searchBox.Text.Trim();

                using (var tagService = new TagService())
                {
                    // 执行模糊查询（匹配料号或创建人）
                    var results = tagService.SearchTags(searchKeyword);

                    // 清空现有数据
                    TagsList.Clear();

                    // 绑定查询结果到UI
                    foreach (var tag in results)
                    {
                        TagsList.Add(tag);
                    }

                    // 如果没有结果提示
                    if (!results.Any())
                    {
                        MessageBox.Show("未找到匹配的记录", "提示",
                                       MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (SqlSugar.SqlSugarException ex)
            {
                MessageBox.Show($"数据库查询失败：{ex.Message}", "错误",
                               MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var page = new SquenceMange.Views.AddRecordPage();
            page.ShowsNavigationUI = true;
        }


        private void EditButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}