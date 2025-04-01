using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SquenceMange.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Frame MainFrame => this.mainFrame;
        public MainWindow()
        {
            InitializeComponent();
            
        }

        // 保存记录菜单点击事件
        private void SaveRecord_Click(object sender, RoutedEventArgs e)
        {
            //var page = new SquenceMange.Views.SaveRecordPage(); 

            //var hostWindow = new Window
            //{
            //    Title = "标签管理",
            //    WindowState = WindowState.Maximized,          // 关键设置1：最大化窗口
            //    //WindowStyle = WindowStyle.None,              // 关键设置2：隐藏标题栏
            //    WindowStartupLocation = WindowStartupLocation.CenterScreen,
            //    Content = page,
            //    SizeToContent = SizeToContent.Manual,        // 关键设置3：禁用自动尺寸
            //    //ResizeMode = ResizeMode.NoResize             // 禁止调整窗口大小
            //};

            //// 适配多显示器配置
            //hostWindow.Width = SystemParameters.VirtualScreenWidth;
            //hostWindow.Height = SystemParameters.VirtualScreenHeight;

            //hostWindow.ShowDialog();

            mainFrame.Navigate(new SaveRecordPage());
        }

        // 生成序列号菜单点击事件
        private void GenerateSerialNumber_Click(object sender, RoutedEventArgs e)
        {
            // 在这里添加生成序列号的代码
            MessageBox.Show("生成序列号功能已触发");
        }
    }
}