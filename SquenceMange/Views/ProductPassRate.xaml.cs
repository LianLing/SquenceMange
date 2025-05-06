using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Linq;
using SequenceMange.Service;
using SequenceMange.Models;
using System.Windows.Media;
using System.Reflection;
using System.Diagnostics;

namespace SequenceManage.Views
{
    public partial class ProductPassRate : Page
    {
        //站点
        public ObservableCollection<CheckBoxItem> Stations { get; set; } = new ObservableCollection<CheckBoxItem>();
        //机型
        public ObservableCollection<string> MachineKindSingle { get; set; } = new ObservableCollection<string>();
        //模组
        public ObservableCollection<string> Modules { get; set; } = new ObservableCollection<string>();
        public string ModuleSingle {  get; set; }
        //工艺
        public ObservableCollection<string> Processes { get; set; } = new ObservableCollection<string>();
        public string processSingle {  get; set; }
        //班组
        public ObservableCollection<string> Teams { get; set; } = new ObservableCollection<string>();
        //机型数据源
        public ObservableCollection<string> allMachineKind { get; set; } = new ObservableCollection<string>();
        //工单
        public ObservableCollection<string> AllMo {  get; set; } = new ObservableCollection<string> { };

        //表单
        public ObservableCollection<ProductPassRateModel> RateList { get; set; } = new ObservableCollection<ProductPassRateModel> { };
        TableService tableService = new TableService();
        ProductPassRateModel rateModel = new ProductPassRateModel();
        private TextBox _comboBoxTextBox;


        public ProductPassRate()
        {
            InitializeComponent();
            DataContext = this;
            InitializeModels();
            
        }

        private async Task InitializeModels()
        {
            // 初始化机型数据
            var result = await tableService.QueryMachineKind();
            forechAdd(result, allMachineKind);
            //初始化工单信息
            var allMo = await tableService.QueryAllMoAsync(prod_type.SelectedItem.ToString());
            forechAdd(allMo, AllMo);
            //初始化班组信息
            var allTeam = await tableService.QueryAllTeam();
            Teams.Add("");
            forechAdd(allTeam, Teams);
        }

        public void forechAdd (List<string> source,ObservableCollection<string> obj)
        {
            foreach (var item in source)
            {
                obj.Add(item);
            }
        }

        private void QueryButton_Click(object sender, RoutedEventArgs e)
        {
            string stationstr = "(";
            var selectedStations = Stations.Where(s => s.IsChecked)
                                           .Select(s => s.Name)
                                           .ToList();
            if (selectedStations.Count == 0)
            {
                throw new Exception("未选择站点");
            }
            if (selectedStations.Count != 0)
            {
                foreach (var item in selectedStations)
                {
                    stationstr = stationstr+"'" +item +"',";
                }
                stationstr = stationstr.Substring(0, stationstr.Length - 1)+")";
            }
            // 构建查询参数对象
            ProductPassRateModel passRateModel = new ProductPassRateModel()
            {
                prod_type = prod_type.SelectedItem?.ToString(),
                prod_module = prod_module.SelectedItem?.ToString(),
                prod_model = prod_model.SelectedItem?.ToString(),
                mo = mo.SelectedItem?.ToString(),
                finished_stamp = datePick.SelectedDate,
                prod_team = team.SelectedItem?.ToString(),
                prod_station = stationstr,
                pass_rate = "0"
            };

            var list = tableService.QueryPassRate(passRateModel);
            foreach (var item in list)
            {
                RateList.Add(item);
            }
        }

      

        private void ModelComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox == null) return;

            // 查找内部的 TextBox
            _comboBoxTextBox = comboBox.Template.FindName("PART_EditableTextBox", comboBox) as TextBox;
            if (_comboBoxTextBox != null)
            {
                _comboBoxTextBox.TextChanged += ComboBoxTextBox_TextChanged;
            }
        }

        private void ComboBoxTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // 过滤逻辑
            TextBox textBox = sender as TextBox;
            string searchText = textBox.Text?.Trim();

            ComboBox parentComboBox = FindParentComboBox(textBox);
            if (parentComboBox == null) return;

            if (string.IsNullOrEmpty(searchText))
            {
                parentComboBox.ItemsSource = allMachineKind;
                parentComboBox.IsDropDownOpen = false;
            }
            else
            {
                var filteredItems = allMachineKind
                    .Where(item => item.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();
                parentComboBox.ItemsSource = filteredItems;
                parentComboBox.IsDropDownOpen = true;
            }
        }

        // 辅助方法：通过控件树查找父级 ComboBox
        private ComboBox FindParentComboBox(DependencyObject child)
        {
            DependencyObject parent = VisualTreeHelper.GetParent(child);
            while (parent != null && !(parent is ComboBox))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }
            return parent as ComboBox;
        }

        private void MachineSelectChanged(object sender, SelectionChangedEventArgs e)
        {
            Modules.Clear();
            if (prod_type.SelectedItem != null)
            {
                //查询模组数据
                var result = tableService.QueryModules(prod_type.SelectedItem.ToString());
                foreach (var module in result)
                {
                    Modules.Add(module);
                }
            }
            
        }

        private void prod_module_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Processes.Clear();
            if (prod_type.SelectedItem != null)
            {
                //查询工艺数据
                var result = tableService.QueryProcesses(prod_type.SelectedItem.ToString());
                foreach (var module in result)
                {
                    Processes.Add(module);
                }
            }
               
        }

        private async void prod_model_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Stations.Clear();
            AllMo.Clear();
            if (prod_type.SelectedItem != null && prod_module.SelectedItem != null && prod_model.SelectedItem != null)
            {
                //查询站点数据
                var result = tableService.QueryStations(prod_type.SelectedItem.ToString(), prod_module.SelectedItem.ToString(), prod_model.SelectedItem.ToString());
                foreach (var module in result)
                {
                    CheckBoxItem checkBoxItem = new CheckBoxItem();
                    checkBoxItem.Name = module;
                    Stations.Add(checkBoxItem);
                }
            }
            //初始化工单信息
            if (prod_type.SelectedItem != null)
            {
                var allMo = await tableService.QueryAllMoAsync(prod_type.SelectedItem.ToString());
                if (allMo != null)
                {
                    forechAdd(allMo, AllMo);
                }
            } 
        }

        private void mo_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox == null) return;

            // 查找内部的 TextBox
            _comboBoxTextBox = comboBox.Template.FindName("PART_EditableTextBox", comboBox) as TextBox;
            if (_comboBoxTextBox != null)
            {
                _comboBoxTextBox.TextChanged += MoTextBox_TextChanged;
            }
        }

        private async void MoTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            //textBox.Text = "";
            string searchText = textBox.Text?.Trim();

            ComboBox parentComboBox = FindParentComboBox(textBox);
            if (parentComboBox == null) return;

            if (string.IsNullOrEmpty(searchText))
            {
                parentComboBox.ItemsSource = AllMo;
                parentComboBox.IsDropDownOpen = false;
            }
            else
            {
                var filteredItems = AllMo
                    .Where(item => item.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();
                parentComboBox.ItemsSource = filteredItems;
                parentComboBox.IsDropDownOpen = true;
            }
        }


    }

    // 站点选择辅助类
    public class CheckBoxItem
    {
        public string Name { get; set; }
        public bool IsChecked { get; set; }
    }
}
