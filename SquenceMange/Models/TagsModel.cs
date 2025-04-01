using SqlSugar;
using System;

namespace SquenceMange.Models
{
    /// <summary>
    /// 标签数据实体类
    /// </summary>
    [SugarTable("tags")]  // 映射数据库表
    public class TagsModel
    {
        /// <summary>
        /// 主键ID（自增）
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 料号（唯一标识）
        /// </summary>
        [SugarColumn(ColumnName = "MaterialId", Length = 50)]
        public string MaterialId { get; set; }

        /// <summary>
        /// 图纸文件路径
        /// </summary>
        [SugarColumn(ColumnName = "PictureAddress", Length = 255)]
        public string PictureAddress { get; set; }

        /// <summary>
        /// 模板名称（最大长度50）
        /// </summary>
        [SugarColumn(ColumnName = "ModelName", Length = 50)]
        public string ModelName { get; set; }

        /// <summary>
        /// 生效状态（默认true）
        /// </summary>
        [SugarColumn(ColumnName = "IsValid")]
        public int IsValid { get; set; }

        /// <summary>
        /// 序列号生成状态
        /// </summary>
        [SugarColumn(ColumnName = "IsCreated")]
        public int IsCreated { get; set; }

        /// <summary>
        /// 打印次数统计（默认0）
        /// </summary>
        [SugarColumn(ColumnName = "PrintTimes")]
        public int PrintTimes { get; set; } = 0;

        /// <summary>
        /// 关联机型（最大长度100）
        /// </summary>
        [SugarColumn(ColumnName = "ConnectMachine", Length = 100)]
        public string ConnectMachine { get; set; }

        /// <summary>
        /// 备注信息（文本类型）
        /// </summary>
        [SugarColumn(ColumnName = "Remark")]
        public string Remark { get; set; }

        /// <summary>
        /// 创建人（最大长度50）
        /// </summary>
        [SugarColumn(ColumnName = "Creater", Length = 50)]
        public string Creater { get; set; }

        /// <summary>
        /// 创建时间（自动记录）
        /// </summary>
        [SugarColumn(ColumnName = "CreateTime")]
        public DateTime CreateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 最后修改人
        /// </summary>
        [SugarColumn(ColumnName = "Editor", Length = 50)]
        public string Editor { get; set; }

        /// <summary>
        /// 最后修改时间（自动更新）
        /// </summary>
        [SugarColumn(ColumnName = "EditTime")]
        public DateTime EditTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 扩展字段（预留）
        /// </summary>
        [SugarColumn(ColumnName = "ExtendValue", Length = 255)]
        public string ExtendValue { get; set; }
    }
}