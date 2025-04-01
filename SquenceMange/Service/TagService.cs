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
            return DbContext.Instance.Queryable<TagsModel>()
                .Where(t => t.MaterialId.Contains(keyword) || t.Creater.Contains(keyword))
                .ToList();
        }

        public bool DeleteTag(int id)
        {
            return DbContext.Instance.Deleteable<TagsModel>(id).ExecuteCommand() > 0;
        }

        public bool UpdateTag(TagsModel tag)
        {
            return DbContext.Instance.Updateable(tag).ExecuteCommand() > 0;
        }
        public void Dispose() => _db?.Dispose();

        public bool InsertTag(TagsModel tag)
        {
            return DbContext.Instance.Insertable(tag).ExecuteCommand() > 0;
        }
    }
}