using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SequenceMange.DataBase;
using SqlSugar;

namespace SequenceMange.Service
{
    internal class TableService : IDisposable
    {
        private readonly DbContext _db;

        public TableService() => _db = new DbContext();
        public void Dispose() => _db?.Dispose();

        public async Task<List<string>> QueryMachineKind()
        {
            try
            {
                string sql = $@"SELECT t.`code` FROM hts_pcs.prod_type t WHERE t.CODE > 'A001' ORDER BY t.code";
                var dataTable = await _db.Instance.Ado.GetDataTableAsync(sql).ConfigureAwait(false);
                return dataTable.Rows.Cast<DataRow>().Select(row => row[0].ToString()).ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<string> QueryModules(string machineKind)
        {
            try
            {
                string sql = $@"SELECT t.`code` FROM hts_pcs.prod_module t WHERE t.prod_type = @prod_type ORDER BY `code`";
                return _db.Instance.Ado.SqlQuery<string>(sql, new { prod_type = machineKind }).ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<string> QueryProcesses(string machineKind)
        {
            try
            {
                string sql = $@"SELECT t.`code` FROM hts_pcs.prod_model t WHERE t.prod_type = @prod_type  ORDER BY `code`";
                return _db.Instance.Ado.SqlQuery<string>(sql, new { prod_type = machineKind }).ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<string> QueryStations(string machineKind,string module,string process)
        {
            try
            {
                string sql = $@"SELECT t.prod_station FROM hts_pcs.vw_eq_cfg_stn_distribute t WHERE t.prod_type = @prod_type AND t.prod_module  = @prod_module AND t.prod_model = @prod_model AND next_cond >= 0";
                return _db.Instance.Ado.SqlQuery<string>(sql, new { prod_type = machineKind, prod_module = module, prod_model = process }).ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }


        public async Task<List<string>> QueryAllMoAsync()
        {
            try
            {
                var sqlServerConfig = new ConnectionConfig()
                {
                    ConnectionString = ConfigurationManager.ConnectionStrings["ErpSQLServer"].ConnectionString,
                    DbType = SqlSugar.DbType.SqlServer,
                    IsAutoCloseConnection = true,
                    InitKeyType = InitKeyType.Attribute
                };

                using var sqlServerDb = new SqlSugarClient(sqlServerConfig);
                string sql = @"SELECT t.MO FROM HS_MO t 
              WHERE F_DATE >= DATEADD(DAY, -7, GETDATE()) 
              ORDER BY MO ASC ";

                var dataTable = await sqlServerDb.Ado.GetDataTableAsync(sql).ConfigureAwait(false);
                return dataTable.Rows.Cast<DataRow>().Select(row => row["MO"].ToString()).ToList();
            }
            catch (Exception)
            {
                // 异常处理（建议记录日志）
                throw;
            }
        }

        public async Task<List<string>> QueryAllTeam()
        {
            try
            {
                var sqlServerConfig = new ConnectionConfig()
                {
                    ConnectionString = ConfigurationManager.ConnectionStrings["TeamSQLServer"].ConnectionString,
                    DbType = SqlSugar.DbType.SqlServer,
                    IsAutoCloseConnection = true,
                    InitKeyType = InitKeyType.Attribute
                };
                using var sqlServerDb = new SqlSugarClient(sqlServerConfig);

                string sql = $@"select t.TeamName from MES_PRODUCT_LINE t";
                var dt = await sqlServerDb.Ado.GetDataTableAsync(sql).ConfigureAwait(false);
                return dt.Rows.Cast<DataRow>().Select(row => row[0].ToString()).ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
