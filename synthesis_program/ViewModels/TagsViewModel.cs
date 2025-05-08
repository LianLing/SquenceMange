﻿using System.ComponentModel;
using System.Collections.ObjectModel;
using SequenceMange.Models;
using System.Windows.Data;

namespace SequenceMange.ViewModels
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

        public ObservableCollection<TagsModel> TagsCollection { get; } = new ObservableCollection<TagsModel>();

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
        }

        private bool FilterTags(object obj)
        {
            if (obj is not TagsModel tag) return false;

            if (string.IsNullOrWhiteSpace(SearchText))
                return true;

            return (tag.MaterialId?.Contains(SearchText) ?? false) ||
                   (tag.Creater?.Contains(SearchText) ?? false);
        }

        

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}