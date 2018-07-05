using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;


namespace T4
{
    public class Config
    {
        //数据库名
        public static readonly string Server = "";//"127.0.0.1";
        public static readonly string DbDatabase = "Test";
        public static readonly string Uid = "sa";
        public static readonly string Pwd = "123123";
        //数据库类型
        public static readonly string DbType = "MsSql";//MsSql\MySql

        //数据库连接	
        public static readonly string ConnectionString = string.Format("server={0};database={1};uid={2};pwd={3}", Server, DbDatabase, Uid, Pwd);
        //命名空间
        public static readonly string Namespace = "DapperTest";
        //需要生成的表名，多张表用,分割
        public static readonly string Tables = "Table1";//"users,rows,titles"
    }
    public class DbFactory
    {
        public static BaseDbHelper CreatDb(string dbType)
        {
            switch (dbType)
            {
                case "MsSql": { return new MsSqlDbHelper(); }
               // case "MySql": { return new MySqlDbHelper(); }
                default: return null;
            }
        }
    }
    public abstract class BaseDbHelper
    {
        public abstract string PreParameter { get; set; }
        public abstract List<DbTable> GetDbTables();
        public abstract List<DbColumn> GetDbColumns(string tableName, string schema = null);
        public abstract DataTable GetDataTable(string commandText, params IDataParameter[] parms);
        public abstract string GetParStr(List<DbColumn> dbComList);

        public string GetFileStr(List<DbColumn> dbColList)
        {
            StringBuilder fields = new StringBuilder();
            foreach (DbColumn column in dbColList)
            {

                if (!column.IsIdentity)
                {
                    fields.Append(column.ColumnName);
                    fields.Append(",");
                }

            }
            fields.Remove(fields.Length - 1, 1);
            return fields.ToString();
        }

        public string GetFileStrWithPrimaryKey(List<DbColumn> dbColList)
        {
            StringBuilder fields = new StringBuilder();
            foreach (DbColumn column in dbColList)
            {

                fields.Append(column.ColumnName);
                fields.Append(",");
            }
            fields.Remove(fields.Length - 1, 1);
            return fields.ToString();
        }
        protected string BGetParStr(List<DbColumn> dbColList, string parStr)
        {
            StringBuilder fields = new StringBuilder();
            foreach (DbColumn column in dbColList)
            {
                if (!column.IsIdentity)
                {
                    fields.Append(parStr + column.ColumnName);
                    fields.Append(",");
                }
            }
            fields.Remove(fields.Length - 1, 1);
            return fields.ToString();
        }
        public List<DbColumn> DtColToList(DataTable dt, ISqlServerDbTypeMap sqlDbTypeMap)
        {
            IEnumerable<DataRow> dr = dt.Rows.Cast<DataRow>();
            List<DbColumn> dbList = new List<DbColumn>();
            DbColumn dbCoModel = null;
            foreach (var row in dr)
            {
                dbCoModel = new DbColumn();
                if (row["ColumnID"] != null && row["ColumnID"].ToString() != "")
                {
                    dbCoModel.ColumnID = int.Parse(row["ColumnID"].ToString());
                }
                if (row["IsPrimaryKey"] != null && row["IsPrimaryKey"].ToString() != "")
                {
                    dbCoModel.IsPrimaryKey = StrToBool(row["IsPrimaryKey"].ToString());
                }
                if (row["ColumnName"] != null)
                {
                    dbCoModel.ColumnName = row["ColumnName"].ToString();
                }
                if (row["ColumnType"] != null)
                {
                    dbCoModel.ColumnType = row["ColumnType"].ToString();
                }
                if (row["IsIdentity"] != null && row["IsIdentity"].ToString() != "")
                {
                    dbCoModel.IsIdentity = StrToBool(row["IsIdentity"].ToString());
                }
                if (row["IsNullable"] != null && row["IsNullable"].ToString() != "")
                {
                    dbCoModel.IsNullable = StrToBool(row["IsNullable"].ToString());
                }
                if (row["ByteLength"] != null && row["ByteLength"].ToString() != "")
                {
                    dbCoModel.ByteLength = int.Parse(row["ByteLength"].ToString());
                }
                if (row["CharLength"] != null && row["CharLength"].ToString() != "")
                {
                    dbCoModel.CharLength = int.Parse(row["CharLength"].ToString());
                }
                if (row["Scale"] != null && row["Scale"].ToString() != "")
                {
                    dbCoModel.Scale = int.Parse(row["Scale"].ToString());
                }
                if (row["Remark"] != null)
                {
                    dbCoModel.Remark = row["Remark"].ToString();
                }
                dbCoModel.CSharpType = sqlDbTypeMap.MapCsharpType(dbCoModel.ColumnType);
                dbCoModel.IsNullCSharpType = sqlDbTypeMap.ChangSqlSqDbTypeMap(dbCoModel.CSharpType, dbCoModel.IsNullable);
                dbCoModel.CommonType = sqlDbTypeMap.MapCommonType(dbCoModel.ColumnType);

                dbList.Add(dbCoModel);
            }
            return dbList;
        }

        private bool StrToBool(string strBol)
        {
            string strbool = strBol.ToLower();
            switch (strbool)
            {
                case "1":
                case "true": { return true; }
                case "0":
                case "false": { return false; }
                default: return false;
            }
        }
    }

    public interface ISqlServerDbTypeMap
    {
        string MapCsharpType(string dbtype);
        Type MapCommonType(string dbtype);
        string ChangSqlSqDbTypeMap(string csharp, bool isNullable);
    }

    #region DbTable

    /// <summary>
    /// 表结构
    /// </summary>
    public sealed class DbTable
    {
        /// <summary>
        /// 表名称
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// 表的架构
        /// </summary>
        public string SchemaName { get; set; }
        /// <summary>
        /// 表的记录数
        /// </summary>
        public int Rows { get; set; }
        /// <summary>
        /// 是否含有主键
        /// </summary>
        public bool HasPrimaryKey { get; set; }
    }
    #endregion

    #region DbColumn
    /// <summary>
    /// 表字段结构
    /// </summary>
    public sealed class DbColumn
    {
        /// <summary>
        /// 字段ID
        /// </summary>
        public int ColumnID { get; set; }
        /// <summary>
        /// 是否主键
        /// </summary>
        public bool IsPrimaryKey { get; set; }
        /// <summary>
        /// 字段名称
        /// </summary>
        public string ColumnName { get; set; }
        /// <summary>
        /// 字段类型
        /// </summary>
        public string ColumnType { get; set; }
        /// <summary>
        /// 数据库类型对应的C#类型
        /// </summary>
        public string CSharpType { get; set; }
        /// <summary>
        /// 数据库类型对应的C#类型
        /// </summary>
        public string IsNullCSharpType { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public Type CommonType { get; set; }
        /// <summary>
        /// 字节长度
        /// </summary>
        public int ByteLength { get; set; }
        /// <summary>
        /// 字符长度
        /// </summary>
        public int CharLength { get; set; }
        /// <summary>
        /// 小数位
        /// </summary>
        public int Scale { get; set; }
        /// <summary>
        /// 是否自增列
        /// </summary>
        public bool IsIdentity { get; set; }
        /// <summary>
        /// 是否允许空
        /// </summary>
        public bool IsNullable { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; }
    }
    #endregion


}
