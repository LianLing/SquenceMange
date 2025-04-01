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
                // 第一步：前端验证必填字段
                if (string.IsNullOrWhiteSpace(txtMaterialId.Text))
                {
                    MessageBox.Show("料号不能为空", "验证错误",
                                  MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string materialId = txtMaterialId.Text.Trim();

                // 第二步：业务层重复性检查（防止绕过前端提交重复数据）
                using (var service = new TagService())
                {
                    if (service.MaterialIdExists(materialId))
                    {
                        MessageBox.Show($"料号 {materialId} 已存在", "验证失败",
                                      MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }

                // 第三步：构建实体
                var newTag = new TagsModel
                {
                    MaterialId = materialId,
                    PictureAddress = txtPicture.Text?.Trim(),
                    ModelName = txtModel.Text?.Trim(),
                    ConnectMachine = txtMachine.Text?.Trim(),
                    Remark = txtRemark.Text?.Trim(),
                    IsValid = 1,
                    CreateTime = DateTime.Now,
                    EditTime = DateTime.Now
                };

                // 第四步：保存数据（数据库约束为最终保障）
                using (var service = new TagService())
                {
                    if (service.InsertTag(newTag))
                    {
                        MessageBox.Show("添加成功", "提示",
                                      MessageBoxButton.OK, MessageBoxImage.Information);
                        NavigationService?.GoBack();
                    }
                }
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