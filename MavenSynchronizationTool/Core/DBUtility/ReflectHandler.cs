using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MavenSynchronizationTool.Core.DBUtility
{
    /// <summary>
    /// 反射句柄类：DataTable与实体类相互转换
    /// </summary>
    /// <typeparam name="T">实体类</typeparam>
    public class ReflectHandler<T> where T : new()
    {
        #region DataTable转换成实体类

        /// <summary>
        /// 填充对象列表：用DataSet的第一个表填充实体类
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public List<T> FillModel(DataSet ds)
        {
            if(ds == null || ds.Tables[0] == null || ds.Tables[0].Rows.Count == 0)
            {
                return null;
            }
            else
            {
                return FillModel(ds.Tables[0]);
            }
        }

        /// <summary>
        /// 填充对象列表：用DataSet的第tableIndex个表填充实体类
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="tableIndex">表索引</param>
        /// <returns></returns>
        public List<T> FillModel(DataSet ds, int tableIndex)
        {
            if(ds == null || ds.Tables.Count <= tableIndex || ds.Tables[tableIndex].Rows.Count == 0)
            {
                return null;
            }
            else
            {
                return FillModel(ds.Tables[tableIndex]);
            }
        }

        public List<T> FillModel(DataTable dt)
        {
            if(dt == null || dt.Rows.Count == 0)
            {
                return null;
            }

            List<T> modelList = new List<T>();
            foreach(DataRow dr in dt.Rows)
            {
                T model = new T();
                for(int loopi = 0;loopi < dr.Table.Columns.Count;loopi++)
                {
                    PropertyInfo propertyInfo = model.GetType().GetProperty(dr.Table.Columns[loopi].ColumnName);
                    if(propertyInfo != null && dr[loopi] != DBNull.Value)
                    {
                        propertyInfo.SetValue(model, dr[loopi], null);
                    }
                }

                modelList.Add(model);
            }

            return modelList;
        }

        #endregion
    }
}
