using SqlSugar;
using SquenceMange.DataBase;
using SquenceMange.Models;

namespace SquenceMange.DataBase
{
    public class TagService
    {
        public List<Tags> SearchTags(string keyword)
        {
            return DbContext.Instance.Queryable<Tags>()
                .Where(t => t.MaterialId.Contains(keyword) || t.Creater.Contains(keyword))
                .ToList();
        }

        public bool DeleteTag(int id)
        {
            return DbContext.Instance.Deleteable<Tags>(id).ExecuteCommand() > 0;
        }

        public bool UpdateTag(Tags tag)
        {
            return DbContext.Instance.Updateable(tag).ExecuteCommand() > 0;
        }
    }
}