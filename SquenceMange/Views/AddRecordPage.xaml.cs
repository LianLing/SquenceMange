using System.Windows;
using System.Windows.Controls;
using SquenceMange.Models;
using SquenceMange.Service;
using SqlSugar;

namespace SquenceMange.Views
{
    public partial class AddRecordPage : Page
    {
        private readonly TagService _tagService = new TagService();

        public AddRecordPage()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // 验证必填字段
                if (string.IsNullOrWhiteSpace(txtMaterialId.Text))
                {
                    MessageBox.Show("料号不能为空", "验证错误",
                                   MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var newTag = new TagsModel
                {
                    MaterialId = txtMaterialId.Text.Trim(),
                    PictureAddress = txtPicture.Text.Trim(),
                    ModelName = txtModel.Text.Trim(),
                    ConnectMachine = txtMachine.Text.Trim(),
                    Remark = txtRemark.Text.Trim(),
                    IsValid = 1,
                    CreateTime = DateTime.Now,
                    EditTime = DateTime.Now
                };

                if (_tagService.InsertTag(newTag))
                {
                    MessageBox.Show("添加成功", "提示",
                                   MessageBoxButton.OK, MessageBoxImage.Information);

                    // 返回上一页
                    NavigationService?.GoBack();
                }
            }
            catch (SqlSugarException ex) when (ex.Message.Contains("Duplicate entry"))
            {
                MessageBox.Show($"料号 {txtMaterialId.Text} 已存在", "添加失败",
                               MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存失败：{ex.Message}", "错误",
                               MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }
    }
}