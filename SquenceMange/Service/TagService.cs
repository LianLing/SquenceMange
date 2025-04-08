using System.Windows.Media.Media3D;
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
                .Where(t => t.BatchNo.Contains(keyword) || t.SequenceNoStart.Contains(keyword)).Where(p => p.IsValid == 1)
                .ToList();
        }

        public List<TagsModel> SearchSequence(string keyword)
        {
            return _db.Instance.Queryable<TagsModel>()
                .Where(t => t.MaterialId.Contains(keyword) || t.SequenceNoStart.Contains(keyword))
                .ToList();
        }

        public List<TagsModel> SearchAllTags()
        {
            return _db.Instance.Queryable<TagsModel>().Where( p => p.IsValid == 1).ToList();
        }

        public bool DeleteTag(int id)
        {
            string sql = $@"update tags set isvalid = 0,EditTime = NOW() where id = @Id";
            var result = _db.Instance.Ado.ExecuteCommand(sql, new {Id = id});
            if (result > 0)
                return true;
            else 
                return false;
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
               // _db.Instance.Insertable(sequence).ExecuteCommand();
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
        //public bool MaterialIdExists(string materialId)
        //{
        //    return _db.Instance.Queryable<TagsModel>()
        //        .Where(t => t.MaterialId == materialId)
        //        .Any();
        //}
        public bool MaterialIdExists(string materialId)
        {
            string sql = $@"select 1 from tags t where t.MaterialId = @MaterialId limit 1";
            var result = _db.Instance.Ado.SqlQuerySingle<int>(sql,new { MaterialId = materialId });
            if (result > 0)
                return true;
            else 
                return false;
        }

        public bool CheckRepeatMaterial(string materialId,int id)
        {
            string sql = $@"select 1 from tags t where t.MaterialId = @MaterialId and id = @Id limit 1";
            var result = _db.Instance.Ado.SqlQuerySingle<int>(sql, new { MaterialId = materialId,Id = id });
            if (result > 0)     //MaterialId未修改
                return true;
            else
            {
                string sql1 = $@"select 1 from tags t where t.MaterialId <> @MaterialId and id = @Id limit 1";
                var result1 = _db.Instance.Ado.SqlQuerySingle<int>(sql1, new { MaterialId = materialId, Id = id });
                if (result1 > 0)    //MaterialId修改了
                {
                    string sql2 = $@"select 1 from tags t where t.MaterialId = @MaterialId and id <> @Id limit 1";
                    var result2 = _db.Instance.Ado.SqlQuerySingle<int>(sql2, new { MaterialId = materialId, Id = id });
                    if (result2>0)  //MaterialId修改了,并且料号重复
                    {
                        return false;
                    }else
                    {
                        return true;
                    }
                }
                else
                    return true;
                
            }
              
        }

        public TagsModel GetLatestData(TagsModel tagsModel)
        {
            string sql = $@"select t.* from tags t where t.MachineKind = @machineKind order by t.createtime desc limit 1";
            return _db.Instance.Ado.SqlQuerySingle<TagsModel>(sql,new { machineKind  = tagsModel.MachineKind});
        }

    }
}