﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using SequenceMange.DataBase;
using SequenceMange.Models;
using SqlSugar;
using SqlSugar.Extensions;

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


        public async Task<List<string>> QueryAllMoAsync(string dataname)
        {
            try
            {
                string database = "hts_prod_" + dataname;
                var sqlServerConfig = new ConnectionConfig()
                {
                    ConnectionString = $@"Server=10.10.1.80;Port=3306;Database={database};Uid=1023711;Pwd=HtsUsr.1;CharSet=utf8mb4;",
                    DbType = SqlSugar.DbType.MySql,
                    IsAutoCloseConnection = true,
                    InitKeyType = InitKeyType.Attribute
                };

                using var sqlServerDb = new SqlSugarClient(sqlServerConfig);
                string sql = @"select distinct t.mo from prod_test_rcds t ";

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

        public List<ProductPassRateModel> QueryPassRate(ProductPassRateModel passRateModel)
        {
            try
            {
                List<ProductPassRateModel> infoList = new List<ProductPassRateModel>();
                string database = "hts_prod_" + passRateModel.prod_type;
                var sqlServerConfig = new ConnectionConfig()
                {
                    ConnectionString = $@"Server=10.10.1.80;Port=3306;Database={database};Uid=1023711;Pwd=HtsUsr.1;CharSet=utf8mb4;",
                    DbType = SqlSugar.DbType.MySql,
                    IsAutoCloseConnection = true,
                    InitKeyType = InitKeyType.Attribute
                };
                using var sqlServerDb = new SqlSugarClient(sqlServerConfig);
                //SN数量
                string str = string.Empty;
                string str1 = "AND t.prod_team = '{passRateModel.prod_team}'";
                string str2 = $@" AND DATE_FORMAT(t.finished_stamp,'%Y-%m-%d') = DATE_FORMAT('{passRateModel.finished_stamp}','%Y-%m-%d') ";
               
                if (!string.IsNullOrEmpty(passRateModel.prod_team))
                    str += str1;
                if (passRateModel.finished_stamp != null)
                    str += str2;
                string sql1 = $@"select count(1) from (select DISTINCT t.sn from prod_test_rcds t where t.mo = '{passRateModel.mo}' {str}) s ";
                int allQuantity = sqlServerDb.Ado.SqlQuerySingle<int>(sql1);
                //SN明细
                string sql2 = $@"select DISTINCT t.sn from prod_test_rcds t where t.mo = '{passRateModel.mo}' ";
                if (!string.IsNullOrEmpty(passRateModel.prod_team))
                    sql2 += str1;
                if (passRateModel.finished_stamp != null)
                    sql2 += str2;
                List<string> list = new List<string>();
                list = sqlServerDb.Ado.SqlQuery<string>(sql2);
                //直通数量
                int PassOK = 0;
                foreach (var sn in list)
                {
                    //当前工单，该SN共有过站记录数量
                    string sql3 = $@"select count(1) from prod_test_rcds t where t.mo = '{passRateModel.mo}' and t.sn = '{sn}' and t.model_curr = '{passRateModel.prod_model}' and t.station_curr in {passRateModel.prod_station}";
                    int allsn = sqlServerDb.Ado.SqlQuerySingle<int>(sql3);
                    //当前SN过站PASS数量
                    string sql4 = $@"select count(1) from prod_test_rcds t where t.mo = '{passRateModel.mo}' and t.sn = '{sn}' and t.model_curr = '{passRateModel.prod_model}' and t.`status` = 1 and t.tst_rlt >= 0 and t.station_curr in {passRateModel.prod_station}";
                    int allPass = sqlServerDb.Ado.SqlQuerySingle<int>(sql4);
                    if (allPass == allsn)
                        PassOK++;
                }
                passRateModel.pass_rate = (PassOK * 1.00 / allQuantity * 100).ToString("0") + '%';
                infoList.Add(passRateModel);
                return infoList;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
