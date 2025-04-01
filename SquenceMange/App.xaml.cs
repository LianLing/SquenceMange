using System.Windows;
using SquenceMange.Service;
using SquenceMange.Views;

namespace SquenceMange
{
    public partial class App : Application
    {
        public NavigationService NavigationService { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // 先创建主窗口实例
            var mainWindow = new MainWindow();

            // 初始化导航服务
            var navService = new NavigationService();
            navService.Initialize(mainWindow.MainFrame);

            // 将服务存入Application属性
            Properties["NavigationService"] = navService;

            // 显示窗口并导航
            //mainWindow.Show();
            //navService.NavigateTo(new SaveRecordPage());
        }
    }
}