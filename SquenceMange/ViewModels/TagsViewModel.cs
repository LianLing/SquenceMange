using System.ComponentModel;
using System.Collections.ObjectModel;
using SquenceMange.Models;
using System.Windows.Data;

namespace SquenceMange.ViewModels
{
    public class TagsViewModel : INotifyPropertyChanged
    {
        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                FilteredTags.Refresh();
            }
        }

        public ObservableCollection<Tags> TagsCollection { get; } = new ObservableCollection<Tags>();

        private ICollectionView _filteredTags;
        public ICollectionView FilteredTags
        {
            get => _filteredTags ??= CollectionViewSource.GetDefaultView(TagsCollection);
            set
            {
                _filteredTags = value;
                OnPropertyChanged(nameof(FilteredTags));
            }
        }

        public TagsViewModel()
        {
            // 初始化过滤条件
            FilteredTags.Filter = FilterTags;
            LoadSampleData();
        }

        private bool FilterTags(object obj)
        {
            if (obj is not Tags tag) return false;

            if (string.IsNullOrWhiteSpace(SearchText))
                return true;

            return (tag.MaterialId?.Contains(SearchText) ?? false) ||
                   (tag.Creater?.Contains(SearchText) ?? false);
        }

        private void LoadSampleData()
        {
            // 测试数据（实际应连接数据库）
            TagsCollection.Add(new Tags
            {
                Id = 1,
                MaterialId = "MAT-001",
                Creater = "张三",
                ConnectMachine = "Model-X",
                IsValid = true
            });

            TagsCollection.Add(new Tags
            {
                Id = 2,
                MaterialId = "MAT-002",
                Creater = "李四",
                ConnectMachine = "Model-Y",
                IsValid = false
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}