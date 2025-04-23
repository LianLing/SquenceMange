using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Linq;

namespace SequenceManage.Views
{
    public partial class ProductPassRate : Page
    {
        //站点
        public ObservableCollection<CheckBoxItem> Stations { get; set; } = new ObservableCollection<CheckBoxItem>();
        //机型
        public ObservableCollection<string> MachineKind { get; set; } = new ObservableCollection<string>();
        //模组
        public ObservableCollection<string> Modules { get; set; } = new ObservableCollection<string>();
        //工艺
        public ObservableCollection<string> Processes { get; set; } = new ObservableCollection<string>();
        //班组
        public ObservableCollection<string> Teams { get; set; } = new ObservableCollection<string>();
        //
        private ObservableCollection<string> allMachineKind;

        public ProductPassRate()
        {
            InitializeComponent();
            DataContext = this;
            InitializeModels();
        }

        private void InitializeModels()
        {
            // 模拟添加一些机型数据
            allMachineKind = new ObservableCollection<string> { "机型1", "机型2", "机型3", "机型4" };
            MachineKind = new ObservableCollection<string>(allMachineKind);
        }

        private void ModelComboBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            string searchText = comboBox.Text;

            if (string.IsNullOrEmpty(searchText))
            {
                MachineKind = new ObservableCollection<string>(allMachineKind);
            }
            else
            {
                var filteredModels = allMachineKind.Where(model => model.Contains(searchText)).ToList();
                MachineKind = new ObservableCollection<string>(filteredModels);
            }

            comboBox.ItemsSource = MachineKind;
        }

        private void QueryButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedStations = Stations.Where(s => s.IsChecked)
                                           .Select(s => s.Name)
                                           .ToList();

            // 构建查询参数对象
            var queryParams = new
            {
                //Model = ModelCombo.SelectedItem?.ToString(),
                //Module = ModuleCombo.SelectedItem?.ToString(),
                //Process = ProcessCombo.SelectedItem?.ToString(),
                //WorkOrder = DatePicker.SelectedDate,
                //Team = TeamCombo.SelectedItem?.ToString(),
                Stations = selectedStations
            };

            // TODO: 调用数据服务获取结果
            MessageBox.Show($"执行查询：{queryParams}");
        }
    }

    // 站点选择辅助类
    public class CheckBoxItem
    {
        public string Name { get; set; }
        public bool IsChecked { get; set; }
    }
}
