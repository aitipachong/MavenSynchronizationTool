using MavenSynchronizationTool.Core.DBUtility;
using MavenSynchronizationTool.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MavenSynchronizationTool.BLL
{
    public class SQLiteFactory
    {
        public bool InsertCentralRespository(CentralRepository centralRepository)
        {
            bool result = false;
            if (centralRepository == null) return result;
            string commandText = string.Empty;
            //生成SQL语句
            //INSERT INTO table_name (列1, 列2,...) VALUES (值1, 值2,....)
            commandText = "INSERT INTO CentralRepository (CentralName, CentralUrl, LocalFolder, SynchState) VALUES(?,?,?,?);";
            SQLiteParameter[] paraList = new SQLiteParameter[4];
            paraList[0] = new SQLiteParameter("@CentralName", DbType.String);
            paraList[0].Value = centralRepository.CentralName;
            paraList[1] = new SQLiteParameter("@CentralUrl", DbType.String);
            paraList[1].Value = centralRepository.CentralUrl;
            paraList[2] = new SQLiteParameter("@LocalFolder", DbType.String);
            paraList[2].Value = centralRepository.LocalFolder;
            paraList[3] = new SQLiteParameter("@SynchState", DbType.Int32);
            paraList[3].Value = centralRepository.SynchState;

            try
            {
                Int64 value = SQLiteHelper.ExecuteInsert(Program.SQLiteConnectionString, commandText, paraList);
                if(value > 0)
                {
                    centralRepository.ID = value;
                }
                result = true;
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return result;
        }
    }
}

/*
              public static int insert(Model.EbookDataModel p)
{
            string sql = "insert into 小说数据(中文字段名1,中文字段名2,中文字段名3)values(?,?,?)";
SQLiteParameter[] parameters = new SQLiteParameter[3];
            parameters[0] = new SQLiteParameter("@中文字段名1", DbType.String);
parameters[0].Value=p.A中文字段名1;
            parameters[1] = new SQLiteParameter("@中文字段名2", DbType.Date);
parameters[1].Value=p.A中文字段名2;
            parameters[2] = new SQLiteParameter("@中文字段名3", DbType.Int32);
parameters[2].Value=p.A中文字段名3;
return SqliteDBUtil.ExecuteInsert(sql,parameters);
}

            return result;
*/
