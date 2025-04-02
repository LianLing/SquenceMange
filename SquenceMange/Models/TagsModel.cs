using SqlSugar;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SquenceMange.Models
{
    /// <summary>
    /// 标签数据实体类
    /// </summary>
    [SugarTable("tags")]  // 映射数据库表
    public class TagsModel : INotifyPropertyChanged
    {
        /// <summary>
        /// 主键ID（自增）
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 料号（唯一标识）
        /// </summary>
        private string _materialId;

        [SugarColumn( ColumnDescription = "唯一料号", ColumnName = "MaterialId")]
        public string MaterialId
        {
            get => _materialId;
            set
            {
                if (_materialId != value)
                {
                    _materialId = value;
                    IsModified = true;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// 图纸文件路径
        /// </summary>
        private string _pictureAddress;
        [SugarColumn(ColumnName = "PictureAddress", Length = 255)]
        public string PictureAddress
        {
            get => _pictureAddress;
            set
            {
                if (_pictureAddress != value)
                {
                    _pictureAddress = value;
                    IsModified = true;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// 模板名称（最大长度50）
        /// </summary>
        private string _modelName;
        [SugarColumn(ColumnName = "ModelName", Length = 50)]
        public string ModelName
        {
            get => _modelName;
            set
            {
                if (_modelName != value)
                {
                    _modelName = value;
                    IsModified = true;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// 生效状态（默认true）
        /// </summary>
        private int _isValid;
        [SugarColumn(ColumnName = "IsValid")]
        public int IsValid
        {
            get => _isValid;
            set
            {
                if (_isValid != value)
                {
                    _isValid = value;
                    IsModified = true;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// 序列号生成状态
        /// </summary>
        private int _isCreated;
        [SugarColumn(ColumnName = "IsCreated")]
        public int IsCreated
        {
            get => _isCreated;
            set
            {
                if (_isCreated != value)
                {
                    _isCreated = value;
                    IsModified = true;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// 打印次数统计（默认0）
        /// </summary>
        private int _printTimes = 0;
        [SugarColumn(ColumnName = "PrintTimes")]
        public int PrintTimes
        {
            get => _printTimes;
            set
            {
                if (_printTimes != value)
                {
                    _printTimes = value;
                    IsModified = true;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// 关联机型（最大长度100）
        /// </summary>
        private string _connectMachine;
        [SugarColumn(ColumnName = "ConnectMachine", Length = 100)]
        public string ConnectMachine
        {
            get => _connectMachine;
            set
            {
                if (_connectMachine != value)
                {
                    _connectMachine = value;
                    IsModified = true;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// 备注信息（文本类型）
        /// </summary>
        private string _remark;
        [SugarColumn(ColumnName = "Remark")]
        public string Remark
        {
            get => _remark;
            set
            {
                if (_remark != value)
                {
                    _remark = value;
                    IsModified = true;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// 创建人（最大长度50）
        /// </summary>
        private string _creater;
        [SugarColumn(ColumnName = "Creater", Length = 50)]
        public string Creater
        {
            get => _creater;
            set
            {
                if (_creater != value)
                {
                    _creater = value;
                    IsModified = true;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// 创建时间（自动记录）
        /// </summary>
        [SugarColumn(ColumnName = "CreateTime")]
        public DateTime CreateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 最后修改人
        /// </summary>
        private string _editor;
        [SugarColumn(ColumnName = "Editor", Length = 50)]
        public string Editor
        {
            get => _editor;
            set
            {
                if (_editor != value)
                {
                    _editor = value;
                    IsModified = true;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// 最后修改时间（自动更新）
        /// </summary>
        private DateTime _editTime;
        [SugarColumn(ColumnName = "EditTime")]
        public DateTime EditTime
        {
            get => _editTime;
            set
            {
                if (_editTime != value)
                {
                    _editTime = value;
                    IsModified = true;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// 扩展字段（预留）
        /// </summary>
        private string _extendValue;
        [SugarColumn(ColumnName = "ExtendValue", Length = 255)]
        public string ExtendValue
        {
            get => _extendValue;
            set
            {
                if (_extendValue != value)
                {
                    _extendValue = value;
                    IsModified = true;
                    OnPropertyChanged();
                }
            }
        }

        // 状态跟踪字段（不映射到数据库）
        [SugarColumn(IsIgnore = true)]
        public bool IsModified { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}