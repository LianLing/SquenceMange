using SqlSugar;
using SquenceMange.DataBase;
using SquenceMange.Models;

namespace SquenceMange.Service
{
    public class TagService:IDisposable
    {

        private readonly DbContext _db;

        public TagService() => _db = new DbContext();

        public List<TagsModel> SearchTags(string keyword)
        {
            return _db.Instance.Queryable<TagsModel>()
                .Where(t => t.MaterialId.Contains(keyword) || t.Creater.Contains(keyword))
                .ToList();
        }

        public bool DeleteTag(int id)
        {
            return _db.Instance.Deleteable<TagsModel>(id).ExecuteCommand() > 0;
        }

        public bool UpdateTag(TagsModel tag)
        {
            return _db.Instance.Updateable(tag).ExecuteCommand() > 0;
        }
        public void Dispose() => _db?.Dispose();

        public bool InsertTag(TagsModel tag)
        {
            try
            {
                _db.Instance.Ado.BeginTran();
                var result = _db.Instance.Insertable(tag).ExecuteCommand() > 0;
                _db.Instance.Ado.CommitTran();
                return result;
            }
            catch
            {
                _db.Instance.Ado.RollbackTran();
                throw;
            }
        }

        // 检查料号是否存在的专用方法
        public bool MaterialIdExists(string materialId)
        {
            return _db.Instance.Queryable<TagsModel>()
                .Where(t => t.MaterialId == materialId)
                .Any();
        }
    }
}