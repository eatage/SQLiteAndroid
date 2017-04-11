﻿using Mono.Data.Sqlite;
using System;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Text;

namespace erp.app
{
    /// <summary>
    /// 操作Sqlite数据库
    /// </summary>
    public class SqliteHelper
    {
        /// <summary>
        /// Sqlite连接字符串
        /// 默认在可执行目录下 数据库文件名为setting.db
        /// <para>在Android平台，用户根目录为/sdcard</para>
        /// </summary>
        public static string dbConnection = "Data Source =" + System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal) + "/my.db;Version=3;";
        private static SqliteConnection conn = null;
        /// <summary>
        /// 是否区分大小写(默认区分大小写)
        /// Sqlite在进行字符串比较的时候，默认对大小写是敏感的
        /// </summary>
        public static Boolean isCaseSensitive = true;
        /// <summary>
        /// 执行sql错误提示 无错误时返回string.Empty
        /// </summary>
        public static string SqlErr = string.Empty;
        /// <summary>
        /// 执行命令返回受影响行数
        /// </summary>
        /// <param name="sql">sql字符串</param>
        /// <param name="as_param">参数 如as_param1="@id=123"</param>
        /// <returns>返回受影响行数 错误时返回-1</returns>
        public static int ExecuteNonQuery(string sql, params string[] as_param)
        {
            if (!isCaseSensitive)
                sql += " COLLATE NOCASE";
            SqlErr = string.Empty;
            try
            {
                using (conn = new SqliteConnection(dbConnection))
                {
                    SqliteCommand cmd = new SqliteCommand(sql, conn);
                    conn.Open();
                    SqliteParameter[] P = GetSqliteParameter(as_param);
                    if (P != null)
                        cmd.Parameters.AddRange(P);
                    return cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                SqlErr = ex.Message;
                SqlLog.WriteLine("sql", SqlErr, sql, as_param);
                return -1;
            }
        }
        /// <summary>
        /// 执行命令返回受影响行数
        /// </summary>
        /// <param name="type">sql命令类型</param>
        /// <param name="as_connStr">连接字符串</param>
        /// <param name="sql">sql字符串</param>
        /// <param name="as_param">参数 如as_param1="@id=123"</param>
        /// <returns>返回受影响行数 错误时返回-1</returns>
        public static int ExecuteNonQuery(CommandType type, string as_connStr, string sql, params string[] as_param)
        {
            if (!isCaseSensitive)
                sql += " COLLATE NOCASE";
            SqlErr = string.Empty;
            try
            {
                using (conn = new SqliteConnection(as_connStr))
                {
                    SqliteCommand cmd = new SqliteCommand(sql, conn);
                    cmd.CommandType = type;
                    conn.Open();
                    SqliteParameter[] P = GetSqliteParameter(as_param);
                    if (P != null)
                        cmd.Parameters.AddRange(P);
                    return cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                SqlErr = ex.Message;
                SqlLog.WriteLine("sql", SqlErr, sql, as_param);
                return -1;
            }
        }
        /// <summary>
        /// 执行命令返回受影响行数
        /// </summary>
        /// <param name="sql">sql字符串</param>
        /// <param name="as_params">参数</param>
        /// <returns>返回受影响行数 错误时返回-1</returns>
        public static int ExecuteNonQuery(string sql, SqliteParameter[] as_params)
        {
            if (!isCaseSensitive)
                sql += " COLLATE NOCASE";
            SqlErr = string.Empty;
            try
            {
                using (conn = new SqliteConnection(dbConnection))
                {
                    SqliteCommand cmd = new SqliteCommand(sql, conn);
                    conn.Open();
                    SqliteParameter[] P = as_params;
                    if (P != null)
                        cmd.Parameters.AddRange(P);
                    return cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                SqlErr = ex.Message;
                SqlLog.WriteLine("sql", SqlErr, sql, as_params);
                return -1;
            }
        }
        /// <summary>
        /// 执行命令返回受影响行数
        /// </summary>
        /// <param name="type">sql命令类型</param>
        /// <param name="as_connStr">连接字符串</param>
        /// <param name="sql">sql字符串</param>
        /// <param name="as_params">参数</param>
        /// <returns>返回受影响行数 错误时返回-1</returns>
        public static int ExecuteNonQuery(CommandType type, string as_connStr, string sql, SqliteParameter[] as_params)
        {
            if (!isCaseSensitive)
                sql += " COLLATE NOCASE";
            SqlErr = string.Empty;
            try
            {
                using (conn = new SqliteConnection(as_connStr))
                {
                    SqliteCommand cmd = new SqliteCommand(sql, conn);
                    cmd.CommandType = type;
                    conn.Open();
                    SqliteParameter[] P = as_params;
                    if (P != null)
                        cmd.Parameters.AddRange(P);
                    return cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                SqlErr = ex.Message;
                SqlLog.WriteLine("sql", SqlErr, sql, as_params);
                return -1;
            }
        }
        /// <summary>
        /// 返回查询字符串第一个匹配项
        /// </summary>
        /// <param name="sql">sql字符串</param>
        /// <param name="as_param">参数 如as_param1="@id=123"</param>
        /// <returns></returns>
        public static object ExecuteScalarObj(string sql, params string[] as_param)
        {
            if (!isCaseSensitive)
                sql += " COLLATE NOCASE";
            SqlErr = string.Empty;
            try
            {
                using (conn = new SqliteConnection(dbConnection))
                {
                    SqliteCommand cmd = new SqliteCommand(sql, conn);
                    conn.Open();
                    SqliteParameter[] P = GetSqliteParameter(as_param);
                    if (P != null)
                        cmd.Parameters.AddRange(P);
                    return cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                SqlErr = ex.Message;
                SqlLog.WriteLine("sql", SqlErr, sql, as_param);
                return null;
            }
        }
        /// <summary>
        /// 返回查询字符串第一个匹配项
        /// </summary>
        /// <param name="type">sql命令类型</param>
        /// <param name="as_connStr">连接字符串</param>
        /// <param name="sql">sql字符串</param>
        /// <param name="as_param">参数 如as_param1="@id=123"</param>
        /// <returns></returns>
        public static object ExecuteScalarObj(CommandType type, string as_connStr, string sql, params string[] as_param)
        {
            if (!isCaseSensitive)
                sql += " COLLATE NOCASE";
            SqlErr = string.Empty;
            try
            {
                using (conn = new SqliteConnection(as_connStr))
                {
                    SqliteCommand cmd = new SqliteCommand(sql, conn);
                    cmd.CommandType = type;
                    conn.Open();
                    SqliteParameter[] P = GetSqliteParameter(as_param);
                    if (P != null)
                        cmd.Parameters.AddRange(P);
                    return cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                SqlErr = ex.Message;
                SqlLog.WriteLine("sql", SqlErr, sql, as_param);
                return null;
            }
        }
        /// <summary>
        /// 返回查询字符串第一个匹配项
        /// </summary>
        /// <param name="sql">sql字符串</param>
        /// <param name="as_params">参数</param>
        /// <returns></returns>
        public static object ExecuteScalarObj(string sql, SqliteParameter[] as_params)
        {
            if (!isCaseSensitive)
                sql += " COLLATE NOCASE";
            SqlErr = string.Empty;
            try
            {
                using (conn = new SqliteConnection(dbConnection))
                {
                    SqliteCommand cmd = new SqliteCommand(sql, conn);
                    conn.Open();
                    SqliteParameter[] P = as_params;
                    if (P != null)
                        cmd.Parameters.AddRange(P);
                    return cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                SqlErr = ex.Message;
                SqlLog.WriteLine("sql", SqlErr, sql, as_params);
                return null;
            }
        }
        /// <summary>
        /// 返回查询字符串第一个匹配项
        /// </summary>
        /// <param name="type">sql命令类型</param>
        /// <param name="as_connStr">连接字符串</param>
        /// <param name="sql">sql字符串</param>
        /// <param name="as_params">参数</param>
        /// <returns></returns>
        public static object ExecuteScalarObj(CommandType type, string as_connStr, string sql, SqliteParameter[] as_params)
        {
            if (!isCaseSensitive)
                sql += " COLLATE NOCASE";
            SqlErr = string.Empty;
            try
            {
                using (conn = new SqliteConnection(as_connStr))
                {
                    SqliteCommand cmd = new SqliteCommand(sql, conn);
                    cmd.CommandType = type;
                    conn.Open();
                    SqliteParameter[] P = as_params;
                    if (P != null)
                        cmd.Parameters.AddRange(P);
                    return cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                SqlErr = ex.Message;
                SqlLog.WriteLine("sql", SqlErr, sql, as_params);
                return null;
            }
        }
        /// <summary>
        /// 返回查询字符串第一个匹配项
        /// </summary>
        /// <param name="sql">sql字符串</param>
        /// <param name="as_param">参数 如as_param1="@id=123"</param>
        /// <returns></returns>
        public static string ExecuteScalar(string sql, params string[] as_param)
        {
            var o = ExecuteScalarObj(sql, as_param);
            if (o != null)
                return o.ToString();
            return null;
        }
        /// <summary>
        /// 返回查询字符串第一个匹配项
        /// </summary>
        /// <param name="type">sql命令类型</param>
        /// <param name="as_connStr">连接字符串</param>
        /// <param name="sql">sql字符串</param>
        /// <param name="as_param">参数 如as_param1="@id=123"</param>
        /// <returns></returns>
        public static string ExecuteScalar(CommandType type, string as_connStr, string sql, params string[] as_param)
        {
            var o = ExecuteScalarObj(type, as_connStr, sql, as_param);
            if (o != null)
                return o.ToString();
            return null;
        }
        /// <summary>
        /// 返回查询字符串第一个匹配项
        /// </summary>
        /// <param name="sql">sql字符串</param>
        /// <param name="as_params">参数</param>
        /// <returns></returns>
        public static string ExecuteScalar(string sql, SqliteParameter[] as_params)
        {
            var o = ExecuteScalarObj(sql, as_params);
            if (o != null)
                return o.ToString();
            return null;
        }
        /// <summary>
        /// 返回查询字符串第一个匹配项
        /// </summary>
        /// <param name="type">sql命令类型</param>
        /// <param name="as_connStr">连接字符串</param>
        /// <param name="sql">sql字符串</param>
        /// <param name="as_params">参数</param>
        /// <returns></returns>
        public static string ExecuteScalar(CommandType type, string as_connStr, string sql, SqliteParameter[] as_params)
        {
            var o = ExecuteScalarObj(type, as_connStr, sql, as_params);
            if (o != null)
                return o.ToString();
            return null;
        }
        /// <summary>
        /// 返回整数
        /// </summary>
        /// <param name="sql">sql字符串</param>
        /// <param name="as_param">参数 如as_param1="@id=123"</param>
        /// <returns>返回整数 错误时返回-1</returns>
        public static int ExecuteScalarNum(string sql, params string[] as_param)
        {
            try { return Convert.ToInt32(ExecuteScalarObj(sql, as_param)); }
            catch { return -1; }
        }
        /// <summary>
        /// 返回整数
        /// </summary>
        /// <param name="type">sql命令类型</param>
        /// <param name="as_connStr">连接字符串</param>
        /// <param name="sql">sql字符串</param>
        /// <param name="as_param">参数 如as_param1="@id=123"</param>
        /// <returns>返回整数 错误时返回-1</returns>
        public static int ExecuteScalarNum(CommandType type, string as_connStr, string sql, params string[] as_param)
        {
            try { return Convert.ToInt32(ExecuteScalarObj(type, as_connStr, sql, as_param)); }
            catch { return -1; }
        }
        /// <summary>
        /// 返回整数
        /// </summary>
        /// <param name="sql">sql字符串</param>
        /// <param name="as_params">参数</param>
        /// <returns>返回整数 错误时返回-1</returns>
        public static int ExecuteScalarNum(string sql, SqliteParameter[] as_params)
        {
            try { return Convert.ToInt32(ExecuteScalarObj(sql, as_params)); }
            catch { return -1; }
        }
        /// <summary>
        /// 返回整数
        /// </summary>
        /// <param name="type">sql命令类型</param>
        /// <param name="as_connStr">连接字符串</param>
        /// <param name="sql">sql字符串</param>
        /// <param name="as_params">参数</param>
        /// <returns>返回整数 错误时返回-1</returns>
        public static int ExecuteScalarNum(CommandType type, string as_connStr, string sql, SqliteParameter[] as_params)
        {
            try { return Convert.ToInt32(ExecuteScalarObj(type, as_connStr, sql, as_params)); }
            catch { return -1; }
        }
        /// <summary>
        /// 返回SqliteDataReader
        /// </summary>
        /// <param name="sql">sql字符串</param>
        /// <param name="as_param">参数 如as_param1="@id=123"</param>
        public static SqliteDataReader ExecuteDataReader(string sql, params string[] as_param)
        {
            if (!isCaseSensitive)
                sql += " COLLATE NOCASE";
            SqlErr = string.Empty;
            try
            {
                using (conn = new SqliteConnection(dbConnection))
                {
                    SqliteCommand cmd = new SqliteCommand(sql, conn);
                    conn.Open();
                    SqliteParameter[] P = GetSqliteParameter(as_param);
                    if (P != null)
                        cmd.Parameters.AddRange(P);
                    SqliteDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                        return reader;
                    else
                        return null;
                }
            }
            catch (Exception ex)
            {
                SqlErr = ex.Message;
                SqlLog.WriteLine("sql", SqlErr, sql, as_param);
                return null;
            }
        }
        /// <summary>
        /// 返回SqliteDataReader
        /// </summary>
        /// <param name="type">sql命令类型</param>
        /// <param name="as_connStr">连接字符串</param>
        /// <param name="sql">sql字符串</param>
        /// <param name="as_param">参数 如as_param1="@id=123"</param>
        public static SqliteDataReader ExecuteDataReader(CommandType type, string as_connStr, string sql, params string[] as_param)
        {
            if (!isCaseSensitive)
                sql += " COLLATE NOCASE";
            SqlErr = string.Empty;
            try
            {
                using (conn = new SqliteConnection(as_connStr))
                {
                    SqliteCommand cmd = new SqliteCommand(sql, conn);
                    cmd.CommandType = type;
                    conn.Open();
                    SqliteParameter[] P = GetSqliteParameter(as_param);
                    if (P != null)
                        cmd.Parameters.AddRange(P);
                    SqliteDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                        return reader;
                    else
                        return null;
                }
            }
            catch (Exception ex)
            {
                SqlErr = ex.Message;
                SqlLog.WriteLine("sql", SqlErr, sql, as_param);
                return null;
            }
        }
        /// <summary>
        /// 返回SqliteDataReader
        /// </summary>
        /// <param name="sql">sql字符串</param>
        /// <param name="as_params">参数</param>
        public static SqliteDataReader ExecuteDataReader(string sql, SqliteParameter[] as_params)
        {
            if (!isCaseSensitive)
                sql += " COLLATE NOCASE";
            SqlErr = string.Empty;
            try
            {
                using (conn = new SqliteConnection(dbConnection))
                {
                    SqliteCommand cmd = new SqliteCommand(sql, conn);
                    conn.Open();
                    SqliteParameter[] P = as_params;
                    if (P != null)
                        cmd.Parameters.AddRange(P);
                    SqliteDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                        return reader;
                    else
                        return null;
                }
            }
            catch (Exception ex)
            {
                SqlErr = ex.Message;
                SqlLog.WriteLine("sql", SqlErr, sql, as_params);
                return null;
            }
        }
        /// <summary>
        /// 返回SqliteDataReader
        /// </summary>
        /// <param name="type">sql命令类型</param>
        /// <param name="as_connStr">连接字符串</param>
        /// <param name="sql">sql字符串</param>
        /// <param name="as_params">参数</param>
        public static SqliteDataReader ExecuteDataReader(CommandType type, string as_connStr, string sql, SqliteParameter[] as_params)
        {
            if (!isCaseSensitive)
                sql += " COLLATE NOCASE";
            SqlErr = string.Empty;
            try
            {
                using (conn = new SqliteConnection(as_connStr))
                {
                    SqliteCommand cmd = new SqliteCommand(sql, conn);
                    cmd.CommandType = type;
                    conn.Open();
                    SqliteParameter[] P = as_params;
                    if (P != null)
                        cmd.Parameters.AddRange(P);
                    SqliteDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                        return reader;
                    else
                        return null;
                }
            }
            catch (Exception ex)
            {
                SqlErr = ex.Message;
                SqlLog.WriteLine("sql", SqlErr, sql, as_params);
                return null;
            }
        }
        /// <summary>
        /// 返回Datatable
        /// </summary>
        /// <param name="sql">sql字符串</param>
        /// <param name="as_param">参数 如as_param1="@id=123"</param>
        /// <returns></returns>
        public static DataTable ExecuteDataTable(string sql, params string[] as_param)
        {
            if (!isCaseSensitive)
                sql += " COLLATE NOCASE";
            SqlErr = string.Empty;
            try
            {
                DataTable ldt = new DataTable();
                using (conn = new SqliteConnection(dbConnection))
                {
                    SqliteCommand cmd = new SqliteCommand(sql, conn);
                    conn.Open();
                    SqliteParameter[] P = GetSqliteParameter(as_param);
                    if (P != null)
                        cmd.Parameters.AddRange(P);
                    using (SqliteDataAdapter sda = new SqliteDataAdapter(cmd))
                    {
                        sda.Fill(ldt);
                    }
                    return ldt;
                    /*
                    //返回DataTable方法
                    //1、如上
                    //2、SqliteDataReader
                    //SqliteDataReader ss = cmd.ExecuteReader();
                    //ldt.Load(ss);
                    //3、SqliteDataReader
                    //DataColumn col;
                    //DataRow row;
                    //int i;
                    //for (i = 0; i < reader.FieldCount; i++)
                    //{
                    //    col = new DataColumn();
                    //    col.ColumnName = reader.GetName(i);
                    //    col.DataType = reader.GetFieldType(i);
                    //    dt.Columns.Add(col);
                    //}
                    //while (reader.Read())
                    //{
                    //    row = dt.NewRow();
                    //    for (i = 0; i < reader.FieldCount; i++)
                    //    {
                    //        row[i] = reader[i];
                    //    }
                    //    dt.Rows.Add(row);
                    //}
                    */
                }
            }
            catch (Exception ex)
            {
                SqlErr = ex.Message;
                SqlLog.WriteLine("sql", SqlErr, sql, as_param);
                return null;
            }
        }
        /// <summary>
        /// 返回Datatable
        /// </summary>
        /// <param name="type">sql命令类型</param>
        /// <param name="as_connStr">连接字符串</param>
        /// <param name="sql">sql字符串</param>
        /// <param name="as_param">参数 如as_param1="@id=123"</param>
        /// <returns></returns>
        public static DataTable ExecuteDataTable(CommandType type, string as_connStr, string sql, params string[] as_param)
        {
            if (!isCaseSensitive)
                sql += " COLLATE NOCASE";
            SqlErr = string.Empty;
            try
            {
                DataTable ldt = new DataTable();

                using (conn = new SqliteConnection(as_connStr))
                {
                    SqliteCommand cmd = new SqliteCommand(sql, conn);
                    cmd.CommandType = type;
                    conn.Open();
                    SqliteParameter[] P = GetSqliteParameter(as_param);
                    if (P != null)
                        cmd.Parameters.AddRange(P);
                    using (SqliteDataAdapter sda = new SqliteDataAdapter(cmd))
                    {
                        sda.Fill(ldt);
                    }
                    return ldt;
                }
            }
            catch (Exception ex)
            {
                SqlErr = ex.Message;
                SqlLog.WriteLine("sql", SqlErr, sql, as_param);
                return null;
            }
        }
        /// <summary>
        /// 返回Datatable
        /// </summary>
        /// <param name="sql">sql字符串</param>
        /// <param name="as_params">参数</param>
        /// <returns></returns>
        public static DataTable ExecuteDataTable(string sql, SqliteParameter[] as_params)
        {
            if (!isCaseSensitive)
                sql += " COLLATE NOCASE";
            SqlErr = string.Empty;
            try
            {
                DataTable ldt = new DataTable();

                using (conn = new SqliteConnection(dbConnection))
                {
                    SqliteCommand cmd = new SqliteCommand(sql, conn);
                    conn.Open();
                    SqliteParameter[] P = as_params;
                    if (P != null)
                        cmd.Parameters.AddRange(P);
                    using (SqliteDataAdapter sda = new SqliteDataAdapter(cmd))
                    {
                        sda.Fill(ldt);
                    }
                    return ldt;
                }
            }
            catch (Exception ex)
            {
                SqlErr = ex.Message;
                SqlLog.WriteLine("sql", SqlErr, sql, as_params);
                return null;
            }
        }
        /// <summary>
        /// 返回Datatable
        /// </summary>
        /// <param name="type">sql命令类型</param>
        /// <param name="as_connStr">连接字符串</param>
        /// <param name="sql">sql字符串</param>
        /// <param name="as_params">参数</param>
        /// <returns></returns>
        public static DataTable ExecuteDataTable(CommandType type, string as_connStr, string sql, SqliteParameter[] as_params)
        {
            if (!isCaseSensitive)
                sql += " COLLATE NOCASE";
            SqlErr = string.Empty;
            try
            {
                DataTable ldt = new DataTable();

                using (conn = new SqliteConnection(as_connStr))
                {
                    SqliteCommand cmd = new SqliteCommand(sql, conn);
                    cmd.CommandType = type;
                    conn.Open();
                    SqliteParameter[] P = as_params;
                    if (P != null)
                        cmd.Parameters.AddRange(P);
                    using (SqliteDataAdapter sda = new SqliteDataAdapter(cmd))
                    {
                        sda.Fill(ldt);
                    }
                    return ldt;
                }
            }
            catch (Exception ex)
            {
                SqlErr = ex.Message;
                SqlLog.WriteLine("sql", SqlErr, sql, as_params);
                return null;
            }
        }
        /// <summary>
        /// 返回当前条件下的总行数
        /// </summary>
        private static int ExecuteScalarRows(CommandType type, string as_connStr, string sql, SqliteParameter[] as_params)
        {
            int beg = sql.ToLower().IndexOf("select");
            int end = sql.ToLower().IndexOf("from");
            string param = sql.Substring(beg + 7, end - beg - 7);
            sql = sql.Replace(param, " count(*) ");
            return ExecuteScalarNum(type, as_connStr, sql, as_params);
        }
        /// <summary>
        /// 返回当前条件下的总行数
        /// </summary>
        private static int ExecuteScalarRows(CommandType type, string as_connStr, string sql, params string[] as_param)
        {
            int beg = sql.ToLower().IndexOf("select");
            int end = sql.ToLower().IndexOf("from");
            string param = sql.Substring(beg + 7, end - beg - 7);
            sql = sql.Replace(param, " count(*) ");
            return ExecuteScalarNum(type, as_connStr, sql, as_param);
        }
        /// <summary>
        /// 返回当前条件下的总行数
        /// </summary>
        private static int ExecuteScalarRows(string sql, SqliteParameter[] as_params)
        {
            int beg = sql.ToLower().IndexOf("select");
            int end = sql.ToLower().IndexOf("from");
            string param = sql.Substring(beg + 7, end - beg - 7);
            sql = sql.Replace(param, " count(*) ");
            return ExecuteScalarNum(sql, as_params);
        }
        /// <summary>
        /// 返回当前条件下的总行数
        /// </summary>
        private static int ExecuteScalarRows(string sql, params string[] as_param)
        {
            int beg = sql.ToLower().IndexOf("select");
            int end = sql.ToLower().IndexOf("from");
            string param = sql.Substring(beg + 7, end - beg - 7);
            sql = sql.Replace(param, " count(*) ");
            return ExecuteScalarNum(sql, as_param);
        }
        /// <summary>
        /// 分页返回Datatable
        /// </summary>
        /// <param name="sql">sql字符串</param>
        /// <param name="ai_PageSize">每页行数</param>
        /// <param name="ai_PageIndex">当前页数</param>
        /// <param name="ai_totalrow">总行数</param>
        /// <param name="as_param">参数 如as_param1="@id=123"</param>
        /// <returns></returns>
        public static DataTable ExecuteDataTablePage(string sql, int ai_PageSize, int ai_PageIndex, ref int ai_totalrow, params string[] as_param)
        {
            if (ai_PageSize > 20000)
                ai_PageSize = 20000;
            DataTable dt = new DataTable();
            if (sql.IndexOf("limit") > 0)
                throw new Exception("分页方法内不允许含有limit命令");
            ai_totalrow = ExecuteScalarRows(sql, as_param);
            if (ai_PageIndex == 1)
                sql += " limit 0," + ai_PageSize;
            else
                sql += " limit " + (ai_PageIndex * ai_PageSize - 1) + "," + ai_PageSize;
            dt = ExecuteDataTable(sql, as_param);
            return dt;
        }
        /// <summary>
        /// 分页返回Datatable
        /// </summary>
        /// <param name="type">sql命令类型</param>
        /// <param name="as_connStr">连接字符串</param>
        /// <param name="sql">sql字符串</param>
        /// <param name="ai_PageSize">每页行数</param>
        /// <param name="ai_PageIndex">当前页数</param>
        /// <param name="ai_totalrow">总行数</param>
        /// <param name="as_param">参数 如as_param1="@id=123"</param>
        /// <returns></returns>
        public static DataTable ExecuteDataTablePage(CommandType type, string as_connStr, string sql, int ai_PageSize, int ai_PageIndex, ref int ai_totalrow, params string[] as_param)
        {
            if (ai_PageSize > 20000)
                ai_PageSize = 20000;
            DataTable dt = new DataTable();
            if (sql.IndexOf("limit") > 0)
                throw new Exception("分页方法内不允许含有limit命令");
            ai_totalrow = ExecuteScalarRows(type, as_connStr, sql, as_param);
            if (ai_PageIndex == 1)
                sql += " limit 0," + ai_PageSize;
            else
                sql += " limit " + (ai_PageIndex * ai_PageSize - 1) + "," + ai_PageSize;
            dt = ExecuteDataTable(type, as_connStr, sql, as_param);
            return dt;
        }
        /// <summary>
        /// 分页返回Datatable
        /// </summary>
        /// <param name="sql">sql字符串</param>
        /// <param name="ai_PageSize">每页行数</param>
        /// <param name="ai_PageIndex">当前页数</param>
        /// <param name="ai_totalrow">总行数</param>
        /// <param name="as_params">参数</param>
        /// <returns></returns>
        public static DataTable ExecuteDataTablePage(string sql, int ai_PageSize, int ai_PageIndex, ref int ai_totalrow, SqliteParameter[] as_params)
        {
            if (ai_PageSize > 20000)
                ai_PageSize = 20000;
            DataTable dt = new DataTable();
            if (sql.IndexOf("limit") > 0)
                throw new Exception("分页方法内不允许含有limit命令");
            ai_totalrow = ExecuteScalarRows(sql, as_params);
            if (ai_PageIndex == 1)
                sql += " limit 0," + ai_PageSize;
            else
                sql += " limit " + (ai_PageIndex * ai_PageSize - 1) + "," + ai_PageSize;
            dt = ExecuteDataTable(sql, as_params);
            return dt;
        }
        /// <summary>
        /// 分页返回Datatable
        /// </summary>
        /// <param name="type">sql命令类型</param>
        /// <param name="as_connStr">连接字符串</param>
        /// <param name="sql">sql字符串</param>
        /// <param name="ai_PageSize">每页行数</param>
        /// <param name="ai_PageIndex">当前页数</param>
        /// <param name="ai_totalrow">总行数</param>
        /// <param name="as_params">参数</param>
        /// <returns></returns>
        public static DataTable ExecuteDataTablePage(CommandType type, string as_connStr, string sql, int ai_PageSize, int ai_PageIndex, ref int ai_totalrow, SqliteParameter[] as_params)
        {
            if (ai_PageSize > 20000)
                ai_PageSize = 20000;
            DataTable dt = new DataTable();
            if (sql.IndexOf("limit") > 0)
                throw new Exception("分页方法内不允许含有limit命令");
            ai_totalrow = ExecuteScalarRows(type, as_connStr, sql, as_params);
            if (ai_PageIndex == 1)
                sql += " limit 0," + ai_PageSize;
            else
                sql += " limit " + (ai_PageIndex * ai_PageSize - 1) + "," + ai_PageSize;
            dt = ExecuteDataTable(type, as_connStr, sql, as_params);
            return dt;
        }
        /// <summary>
        /// 执行事务sql
        /// </summary>
        /// <param name="SqlList">多条sql</param>
        public static void TransaNonQuery(string[] SqlList)
        {
            SqlErr = "";
            if (SqlList == null)
                throw new ArgumentNullException("Sql语句不能为空");
            if (SqlList.Length == 0)
                throw new ArgumentNullException("Sql语句不能为空");
            try
            {
                using (SqliteConnection conn = new SqliteConnection(dbConnection))//创建连接  
                {
                    conn.Open();//打开连接  
                    using (SqliteTransaction tran = conn.BeginTransaction())//实例化一个事务  
                    {
                        try
                        {
                            foreach (string ls_sql in SqlList)
                            {
                                SqliteCommand cmd = new SqliteCommand(conn);//实例化SQL命令  
                                cmd.Transaction = tran;
                                cmd.CommandText = ls_sql;//设置带参SQL语句  
                                cmd.ExecuteNonQuery();//执行查询  
                            }
                            tran.Commit();//提交  
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                            SqlErr = ex.Message;
                            SqlLog.WriteLine("sql", SqlErr, "", "");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SqlErr = ex.Message;
                SqlLog.WriteLine("sql", SqlErr, "", "");
            }
        }
        /// <summary>
        /// 执行事务sql
        /// </summary>
        /// <param name="type">sql命令类型</param>
        /// <param name="as_connStr">连接字符串</param>
        /// <param name="SqlList">多条sql</param>
        public static void TransaNonQuery(CommandType type, string as_connStr, string[] SqlList)
        {
            SqlErr = "";
            if (SqlList == null)
                throw new ArgumentNullException("Sql语句不能为空");
            if (SqlList.Length == 0)
                throw new ArgumentNullException("Sql语句不能为空");
            try
            {
                using (SqliteConnection conn = new SqliteConnection(as_connStr))//创建连接  
                {
                    conn.Open();//打开连接  
                    using (SqliteTransaction tran = conn.BeginTransaction())//实例化一个事务  
                    {
                        try
                        {
                            foreach (string ls_sql in SqlList)
                            {
                                SqliteCommand cmd = new SqliteCommand(conn);//实例化SQL命令  
                                cmd.CommandType = type;
                                cmd.Transaction = tran;
                                cmd.CommandText = ls_sql;//设置带参SQL语句  
                                cmd.ExecuteNonQuery();//执行查询  
                            }
                            tran.Commit();//提交  
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                            SqlErr = ex.Message;
                            SqlLog.WriteLine("sql", SqlErr, "", "");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SqlErr = ex.Message;
                SqlLog.WriteLine("sql", SqlErr, "", "");
            }
        }
        /// <summary>
        /// 使用事务进行批量插入
        /// DataTable的列数必须与表的列数(包括顺序)对应
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="table">DataTable数据</param>
        public static void DoTran(string tableName, DataTable table)
        {
            SqlErr = "";
            DbProviderFactory factory = SqliteFactory.Instance;
            using (DbConnection conn = factory.CreateConnection())
            {
                // 连接数据库  
                conn.ConnectionString = dbConnection;
                conn.Open();
                DbTransaction trans = conn.BeginTransaction();
                DbCommand cmd = conn.CreateCommand();
                try
                {
                    // 将整个DataTable插入到数据库  
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        //生成sql语句
                        string ls_sql = "INSERT INTO " + tableName + " VALUES(";
                        for (int j = 0; j < table.Columns.Count; j++)
                        {
                            ls_sql += "'" + table.Rows[i][j] + "',";
                        }
                        ls_sql += " )";
                        ls_sql = ls_sql.Replace("', )", "' )");
                        cmd.CommandText = ls_sql;
                        //cmd.Parameters[0].Value = i.ToString();
                        cmd.ExecuteNonQuery();
                    }
                    //事务提交
                    trans.Commit();
                }
                catch (Exception e)
                {
                    SqlErr = e.Message;
                    SqlLog.WriteLine("sql", SqlErr, "TranTableName:" + tableName, "");
                    //事务回滚
                    trans.Rollback();
                }
            }
        }
        /// <summary>
        /// 使用事务进行批量插入
        /// DataTable的列数必须与表的列数(包括顺序)对应
        /// </summary>
        /// <param name="type">sql命令类型</param>
        /// <param name="as_connStr">连接字符串</param>
        /// <param name="tableName">表名</param>
        /// <param name="table">DataTable数据</param>
        public static void DoTran(CommandType type, string as_connStr, string tableName, DataTable table)
        {
            SqlErr = "";
            DbProviderFactory factory = SqliteFactory.Instance;
            using (DbConnection conn = factory.CreateConnection())
            {
                // 连接数据库  
                conn.ConnectionString = as_connStr;
                conn.Open();
                DbTransaction trans = conn.BeginTransaction();
                DbCommand cmd = conn.CreateCommand();
                cmd.CommandType = type;
                try
                {
                    // 将整个DataTable插入到数据库  
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        //生成sql语句
                        string ls_sql = "INSERT INTO " + tableName + " VALUES(";
                        for (int j = 0; j < table.Columns.Count; j++)
                        {
                            ls_sql += "'" + table.Rows[i][j] + "',";
                        }
                        ls_sql += " )";
                        ls_sql = ls_sql.Replace("', )", "' )");
                        cmd.CommandText = ls_sql;
                        //cmd.Parameters[0].Value = i.ToString();
                        cmd.ExecuteNonQuery();
                    }
                    //事务提交
                    trans.Commit();
                }
                catch (Exception e)
                {
                    SqlErr = e.Message;
                    SqlLog.WriteLine("sql", SqlErr, "TranTableName:" + tableName, "");
                    //事务回滚
                    trans.Rollback();
                }
            }
        }
        /// <summary>
        /// 使用事务进行批量插入
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="columns">需要插入的字段，按顺序用,分隔</param>
        /// <param name="table">导入的DataTable</param>
        /// <param name="isTrim">是否去除前后空格 默认去除</param>
        public static void DoTran(string tableName, string columns, DataTable table, bool isTrim = true)
        {
            SqlErr = "";
            DbProviderFactory factory = SqliteFactory.Instance;
            using (DbConnection conn = factory.CreateConnection())
            {
                // 连接数据库  
                conn.ConnectionString = dbConnection;
                conn.Open();
                DbTransaction trans = conn.BeginTransaction();
                DbCommand cmd = conn.CreateCommand();
                try
                {
                    // 将整个DataTable插入到数据库  
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        //生成sql语句
                        string ls_sql = "INSERT INTO " + tableName + "(";
                        string[] list = columns.Split(',');
                        for (int j = 0; j < list.Length; j++)
                        {
                            ls_sql += list[j] + ",";
                        }
                        ls_sql += " )VALUES(";
                        for (int j = 0; j < list.Length; j++)
                        {
                            string value = "";
                            //去空格
                            if (isTrim)
                                value = table.Rows[i][j] == null ? null : table.Rows[i][j].ToString().Trim();
                            else
                                value = table.Rows[i][j] == null ? null : table.Rows[i][j].ToString();
                            if (value != null)
                                ls_sql += "'" + value + "',";
                            else
                                ls_sql += "null,";
                        }
                        ls_sql += " )";
                        ls_sql = ls_sql.Replace(", )", " )");

                        cmd.CommandText = ls_sql;
                        //cmd.Parameters[0].Value = i.ToString();

                        cmd.ExecuteNonQuery();
                    }
                    //事务提交
                    trans.Commit();
                }
                catch (Exception e)
                {
                    SqlErr = e.Message;
                    SqlLog.WriteLine("sql", SqlErr, "TranTableName:" + tableName, "");
                    //事务回滚
                    trans.Rollback();
                }
            }
        }
        /// <summary>
        /// 使用事务进行批量插入
        /// </summary>
        /// <param name="type">sql命令类型</param>
        /// <param name="as_connStr">连接字符串</param>
        /// <param name="tableName">表名</param>
        /// <param name="columns">需要插入的字段，按顺序用,分隔</param>
        /// <param name="table">导入的DataTable</param>
        /// <param name="isTrim">是否去除前后空格 默认去除</param>
        public static void DoTran(CommandType type, string as_connStr, string tableName, string columns, DataTable table, bool isTrim = true)
        {
            SqlErr = "";
            DbProviderFactory factory = SqliteFactory.Instance;
            using (DbConnection conn = factory.CreateConnection())
            {
                // 连接数据库  
                conn.ConnectionString = as_connStr;
                conn.Open();
                DbTransaction trans = conn.BeginTransaction();
                DbCommand cmd = conn.CreateCommand();
                cmd.CommandType = type;
                try
                {
                    // 将整个DataTable插入到数据库  
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        //生成sql语句
                        string ls_sql = "INSERT INTO " + tableName + "(";
                        string[] list = columns.Split(',');
                        for (int j = 0; j < list.Length; j++)
                        {
                            ls_sql += list[j] + ",";
                        }
                        ls_sql += " )VALUES(";
                        for (int j = 0; j < list.Length; j++)
                        {
                            string value = "";
                            //去空格
                            if (isTrim)
                                value = table.Rows[i][j] == null ? null : table.Rows[i][j].ToString().Trim();
                            else
                                value = table.Rows[i][j] == null ? null : table.Rows[i][j].ToString();
                            if (value != null)
                                ls_sql += "'" + value + "',";
                            else
                                ls_sql += "null,";
                        }
                        ls_sql += " )";
                        ls_sql = ls_sql.Replace(", )", " )");
                        cmd.CommandText = ls_sql;
                        //cmd.Parameters[0].Value = i.ToString();

                        cmd.ExecuteNonQuery();
                    }
                    //事务提交
                    trans.Commit();
                }
                catch (Exception e)
                {
                    SqlErr = e.Message;
                    SqlLog.WriteLine("sql", SqlErr, "TranTableName:" + tableName, "");
                    //事务回滚
                    trans.Rollback();
                }
            }
        }
        /// <summary>
        /// 是否存在表
        /// </summary>
        /// <param name="tableName">表名</param>
        public static Boolean of_ExistTable(string tableName)
        {
            string sql = "SELECT count(*) FROM sqlite_master WHERE type='table' AND name=@tablename";
            int row = ExecuteScalarNum(sql, "@tablename=" + tableName);
            if (row == 1)
                return true;
            else
                return false;
        }
        /// <summary>
        /// 是否存在表
        /// </summary>
        /// <param name="type">sql命令类型</param>
        /// <param name="as_connStr">连接字符串</param>
        /// <param name="tableName">表名</param>
        public static Boolean of_ExistTable(CommandType type, string as_connStr, string tableName)
        {
            string sql = "SELECT count(*) FROM sqlite_master WHERE type='table' AND name=@tablename";
            int row = ExecuteScalarNum(type, as_connStr, sql, "@tablename=" + tableName);
            if (row == 1)
                return true;
            else
                return false;
        }
        /// <summary>
        /// 判断是否存在某个字段
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="as_ColName">字段名</param>
        public static Boolean of_ExistCol(string tableName, string as_ColName)
        {
            SqlErr = "";
            string sql = string.Format("pragma table_info('{0}')", tableName);
            DataTable ldt_tb = new DataTable();
            ldt_tb = ExecuteDataTable(sql);
            if (ldt_tb == null)
                return false;
            DataRow[] l_row = ldt_tb.Select("name='" + as_ColName + "'");
            if (l_row.Length == 0)
                return false;
            return true;
        }
        /// <summary>
        /// 判断是否存在某个字段
        /// </summary>
        /// <param name="type">sql命令类型</param>
        /// <param name="as_connStr">连接字符串</param>
        /// <param name="tableName">表名</param>
        /// <param name="as_ColName">字段名</param>
        public static Boolean of_ExistCol(CommandType type, string as_connStr, string tableName, string as_ColName)
        {
            SqlErr = "";
            string sql = string.Format("pragma table_info('{0}')", tableName);
            DataTable ldt_tb = new DataTable();
            ldt_tb = ExecuteDataTable(type, as_connStr, sql);
            if (ldt_tb == null)
                return false;
            DataRow[] l_row = ldt_tb.Select("name='" + as_ColName + "'");
            if (l_row.Length == 0)
                return false;
            return true;
        }
        /// <summary>
        /// 将参数转为SqliteParameter对象
        /// </summary>
        /// <param name="as_param">参数 如as_param1="@id=123"</param>
        private static SqliteParameter[] GetSqliteParameter(params string[] as_param)
        {
            if (as_param == null)
                return null;
            if (as_param.Length == 0)
                return null;
            SqliteParameter[] P = new SqliteParameter[as_param.Length];
            for (int i = 0; i < as_param.Length; i++)
            {
                string ls_str = as_param[i];
                if (string.IsNullOrEmpty(ls_str))
                    throw new ArgumentNullException("传入参数不允许为空,正确格式为：@id=123");
                if (ls_str.IndexOf("=") == -1)
                    throw new ArgumentNullException("传入参数错误,正确格式为：@id=123");
                string[] ls_strlist = ls_str.Split(new string[] { "=" }, StringSplitOptions.None);
                if (ls_strlist.Length > 2)
                    throw new ArgumentNullException("传入参数错误,正确格式为：@id=123");
                string ls_param = ls_strlist[0].Trim();
                string ls_value = ls_strlist[1];
                P[i] = new SqliteParameter(ls_param, ls_value);
            }
            return P;
        }
        /// <summary>
        /// SqlServer(供参考)
        /// </summary>
        /// <param name="as_param">参数 如as_param1="@id=123"</param>
        private System.Data.SqlClient.SqlParameter[] GetSqlParameter(params string[] as_param)
        {
            if (as_param == null)
                return null;
            if (as_param.Length == 0)
                return null;
            System.Data.SqlClient.SqlParameter[] P = new System.Data.SqlClient.SqlParameter[as_param.Length];

            for (int i = 0; i < as_param.Length; i++)
            {
                string ls_str = as_param[i];
                if (string.IsNullOrEmpty(ls_str))
                    throw new ArgumentNullException("传入参数不允许为空,正确格式为：@id=123");
                if (ls_str.IndexOf("=") == -1)
                    throw new ArgumentNullException("传入参数错误,正确格式为：@id=123");
                string[] ls_strlist = ls_str.Split(new string[] { "=" }, StringSplitOptions.None);
                if (ls_strlist.Length != 2)
                    throw new ArgumentNullException("传入参数错误,正确格式为：@id=123");
                string ls_param = ls_strlist[0].Trim();
                string ls_value = ls_strlist[1];

                P[i] = new System.Data.SqlClient.SqlParameter(ls_param, ls_value);
            }
            return P;
        }
    }
    /// <summary>
    /// insert or update
    /// </summary>
    public class n_create_sql
    {
        private string tableName = string.Empty;
        private System.Collections.ArrayList Col = new System.Collections.ArrayList();
        private System.Collections.ArrayList Set = new System.Collections.ArrayList();
        private string dbconnStr = SqliteHelper.dbConnection;
        /// <summary>
        /// 执行sql错误提示 无错误时返回string.Empty
        /// </summary>
        public string SqlErr = string.Empty;
        /// <summary>
        /// 执行sql的字符串
        /// </summary>
        public string SqlStr = string.Empty;

        public n_create_sql()
        {

        }
        public n_create_sql(string as_tableName)
        {
            tableName = as_tableName;
        }
        public n_create_sql(CommandType type, string as_connStr, string as_tableName)
        {
            dbconnStr = as_connStr;
            tableName = as_tableName;
        }
        /// <summary>
        /// 设置表名
        /// </summary>
        /// <param name="as_tableName">表名</param>
        public void of_SetTable(string as_tableName)
        {
            tableName = as_tableName;
        }
        /// <summary>
        /// 插入参数
        /// </summary>
        /// <param name="as_Col">字段名</param>
        /// <param name="as_Set">值</param>
        public void of_AddCol(string as_Col, object as_Set)
        {
            Col.Add(as_Col);
            Set.Add(as_Set);
        }
        /// <summary>
        /// 执行inset
        /// </summary>
        public Boolean of_execute()
        {
            SqlErr = "";
            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException("tableName is not value");
            if (Col.Count == 0)
                throw new ArgumentNullException("columns is not value");
            if (Set.Count != Col.Count)
                throw new ArgumentNullException("columns.Length != values.Length");
            string query = "INSERT INTO " + tableName + "(";
            foreach (object a in Col)
            {
                query += "," + a;
            }
            query += ") VALUES (";
            foreach (object a in Set)
            {
                query += ", '" + a + "'";
            }
            query += ")";
            query = query.Replace("(,", "(");
            SqlStr = query;
            int li_row = SqliteHelper.ExecuteNonQuery(CommandType.Text, dbconnStr, query);
            SqlErr = SqliteHelper.SqlErr;
            if (li_row >= 0)
                return true;
            else
                return false;
        }
        /// <summary>
        /// 执行update
        /// </summary>
        /// <param name="as_where">id=@id</param>
        /// <param name="as_param">参数 如as_param1="@id=123"</param>
        public Boolean of_execute(string as_where, params string[] as_param)
        {
            SqlErr = "";
            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException("tableName is not value");
            if (Col.Count == 0)
                throw new ArgumentNullException("columns is not value");
            if (Set.Count != Col.Count)
                throw new ArgumentNullException("columns.Length != values.Length");
            string query = "update " + tableName + " set ";
            for (int i = 0; i < Col.Count; i++)
            {
                query += Col[i] + "='" + Set[i] + "',";
            }
            query += "where " + as_where;
            query = query.Replace(",where", " where");
            SqlStr = query;
            int li_row = SqliteHelper.ExecuteNonQuery(CommandType.Text, dbconnStr, query, as_param);
            SqlErr = SqliteHelper.SqlErr;
            if (li_row >= 0)
                return true;
            else
                return true;
        }
        /// <summary>
        /// 执行update或insert
        /// 当条件存在时执行update 不存在时执行insert
        /// </summary>
        /// <param name="as_where">id=@id</param>
        /// <param name="as_param">参数 如as_param1="@id=123"</param>
        public Boolean of_executeInsertOrUpdate(string as_where, params string[] as_param)
        {
            string ls_sql = @"select count(*) from " + tableName + " " + as_where;
            int li_row = SqliteHelper.ExecuteScalarNum(ls_sql, as_param);
            if (li_row > 0)
                return of_execute(as_where, as_param);
            else
                return of_execute();
        }
        /// <summary>
        /// 获取sql字符串
        /// </summary>
        public string of_GetSql()
        {
            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException("tableName is not value");
            if (Col.Count == 0)
                throw new ArgumentNullException("columns is not value");
            if (Set.Count != Col.Count)
                throw new ArgumentNullException("columns.Length != values.Length");
            string query = "INSERT INTO " + tableName + "(";
            foreach (object a in Col)
            {
                query += "," + a;
            }
            query += ") VALUES (";
            foreach (object a in Set)
            {
                query += ", '" + a + "'";
            }
            query += ")";
            query = query.Replace("(,", "(");
            return query;
        }
        /// <summary>
        /// 获取sql字符串
        /// </summary>
        /// <param name="as_where">id=@id</param>
        /// <param name="as_param">参数 如as_param1="@id=123"</param>
        public string of_GetSql(string as_where, params string[] as_param)
        {
            SqlErr = "";
            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException("tableName is not value");
            if (Col.Count == 0)
                throw new ArgumentNullException("columns is not value");
            if (Set.Count != Col.Count)
                throw new ArgumentNullException("columns.Length != values.Length");
            string query = "update " + tableName + " set ";
            for (int i = 0; i < Col.Count; i++)
            {
                query += Col[i] + "='" + Set[i] + "',";
            }
            query += "where " + as_where;
            query = query.Replace(",where", " where");
            return query;
        }
    }
    /// <summary>
    /// 系统参数表类 表名mysysset
    /// </summary>
    public class publicfuns
    {
        /// <summary>
        /// 执行sql错误提示 无错误时返回string.Empty
        /// </summary>
        public static string SqlErr = string.Empty;
        /// <summary>
        /// 创建mysysset表
        /// </summary>
        public static void CreateSysParamTable()
        {
            if (!SqliteHelper.of_ExistTable("mysysset"))
            {
                string ls_sql = @"CREATE TABLE mysysset ( 
                                    itemtype  VARCHAR(50) NOT NULL,
                                    itemname  VARCHAR(50) NOT NULL,
                                    itemvalue VARCHAR(200),
                                    memo      VARCHAR(200),
                                    primary key (itemtype,itemname) 
                                    );  ";
                SqliteHelper.ExecuteNonQuery(ls_sql);
            }
        }
        /// <summary>
        /// 创建mysysset表
        /// </summary>
        public static void CreateSysParamTable(string dbconn)
        {
            if (!SqliteHelper.of_ExistTable(CommandType.Text, dbconn, "mysysset"))
            {
                string ls_sql = @"CREATE TABLE mysysset ( 
                                    itemtype  VARCHAR(50) NOT NULL,
                                    itemname  VARCHAR(50) NOT NULL,
                                    itemvalue VARCHAR(200),
                                    memo      VARCHAR(200),
                                    primary key (itemtype,itemname) 
                                    );  ";
                SqliteHelper.ExecuteNonQuery(CommandType.Text, dbconn, ls_sql);
            }
        }
        /// <summary>
        /// 写入itemvalue
        /// </summary>
        public static bool of_SetMySysSet(string as_type, string as_item, string as_value, string as_memo = "")
        {
            CreateSysParamTable();
            SqlErr = "";
            int li_row = 0;
            string ls_sql = "select count(*) from mysysset where itemtype=@itemtype and itemname=@itemname";
            li_row = SqliteHelper.ExecuteScalarNum(ls_sql, "@itemtype=" + as_type, "@itemname=" + as_item);
            n_create_sql lnv_sql = new n_create_sql();
            lnv_sql.of_SetTable("mysysset");
            lnv_sql.of_AddCol("itemvalue", as_value);
            if (!string.IsNullOrEmpty(as_memo))
            {
                lnv_sql.of_AddCol("memo", as_memo);
            }
            bool li_rc = false;
            if (li_row <= 0)
            {
                lnv_sql.of_AddCol("itemtype", as_type);
                lnv_sql.of_AddCol("itemname", as_item);
                li_rc = lnv_sql.of_execute();
            }
            if (li_row > 0)
            {
                li_rc = lnv_sql.of_execute("itemtype=@itemtype and itemname=@itemname", "@itemtype=" + as_type, "@itemname=" + as_item);
            }
            SqlErr = lnv_sql.SqlErr;
            return li_rc;
        }
        /// <summary>
        /// 写入itemvalue
        /// </summary>
        public static bool of_SetMySysSet(CommandType type, string dbconn, string as_type, string as_item, string as_value, string as_memo = "")
        {
            CreateSysParamTable(dbconn);
            SqlErr = "";
            int li_row = 0;
            string ls_sql = "select count(*) from mysysset where itemtype=@itemtype and itemname=@itemname";
            li_row = SqliteHelper.ExecuteScalarNum(type, dbconn, ls_sql, "@itemtype=" + as_type, "@itemname=" + as_item);
            n_create_sql lnv_sql = new n_create_sql(type, dbconn, "mysysset");
            lnv_sql.of_AddCol("itemvalue", as_value);
            if (!string.IsNullOrEmpty(as_memo))
            {
                lnv_sql.of_AddCol("memo", as_memo);
            }
            bool li_rc = false;
            if (li_row <= 0)
            {
                lnv_sql.of_AddCol("itemtype", as_type);
                lnv_sql.of_AddCol("itemname", as_item);
                li_rc = lnv_sql.of_execute();
            }
            if (li_row > 0)
            {
                li_rc = lnv_sql.of_execute("itemtype=@itemtype and itemname=@itemname", "@itemtype=" + as_type, "@itemname=" + as_item);
            }
            SqlErr = lnv_sql.SqlErr;
            return li_rc;
        }
        /// <summary>
        /// 写入memo(当前行必须存在) 
        /// </summary>
        public static bool of_SetMySysSetMemo(string as_type, string as_item, string as_memo)
        {
            CreateSysParamTable();
            SqlErr = "";
            int li_row = 0;
            string ls_sql = "select count(*) from mysysset where itemtype=@itemtype and itemname=@itemname";
            li_row = SqliteHelper.ExecuteScalarNum(ls_sql, "@itemtype=" + as_type, "@itemname=" + as_item);
            if (li_row <= 0)
                return false;
            n_create_sql lnv_sql = new n_create_sql();
            lnv_sql.of_SetTable("mysysset");
            lnv_sql.of_AddCol("memo", as_memo);
            bool li_rc = false;
            if (li_row > 0)
            {
                li_rc = lnv_sql.of_execute("itemtype=@itemtype and itemname=@itemname", "@itemtype=" + as_type, "@itemname=" + as_item);
            }
            SqlErr = lnv_sql.SqlErr;
            return li_rc;
        }
        /// <summary>
        /// 写入memo(当前行必须存在) 
        /// </summary>
        public static bool of_SetMySysSetMemo(CommandType type, string dbconn, string as_type, string as_item, string as_memo)
        {
            CreateSysParamTable();
            SqlErr = "";
            int li_row = 0;
            string ls_sql = "select count(*) from mysysset where itemtype=@itemtype and itemname=@itemname";
            li_row = SqliteHelper.ExecuteScalarNum(type, dbconn, ls_sql, "@itemtype=" + as_type, "@itemname=" + as_item);
            if (li_row <= 0)
                return false;
            n_create_sql lnv_sql = new n_create_sql(type, dbconn, "mysysset");
            lnv_sql.of_AddCol("memo", as_memo);
            bool li_rc = false;
            li_rc = lnv_sql.of_execute("itemtype=@itemtype and itemname=@itemname", "@itemtype=" + as_type, "@itemname=" + as_item);
            SqlErr = lnv_sql.SqlErr;
            return li_rc;
        }
        /// <summary>
        /// 返回itemvalue
        /// </summary>
        public static string of_GetMySysSet(string as_type, string as_item)
        {
            CreateSysParamTable();
            SqlErr = "";
            string ls_value = "";
            string ls_sql = "select itemvalue from mysysset where itemtype=@itemtype and itemname=@itemname";
            ls_value = SqliteHelper.ExecuteScalar(ls_sql, "@itemtype=" + as_type, "@itemname=" + as_item);
            SqlErr = SqliteHelper.SqlErr;
            return ls_value;
        }
        /// <summary>
        /// 返回itemvalue
        /// </summary>
        public static string of_GetMySysSet(CommandType type, string dbconn, string as_type, string as_item)
        {
            CreateSysParamTable(dbconn);
            SqlErr = "";
            string ls_value = "";
            string ls_sql = "select itemvalue from mysysset where itemtype=@itemtype and itemname=@itemname";
            ls_value = SqliteHelper.ExecuteScalar(type, dbconn, ls_sql, "@itemtype=" + as_type, "@itemname=" + as_item);
            SqlErr = SqliteHelper.SqlErr;
            return ls_value;
        }
        /// <summary>
        /// 返回memo
        /// </summary>
        public static string of_GetMySysSetMemo(string as_type, string as_item)
        {
            CreateSysParamTable();
            SqlErr = "";
            string ls_value = "";
            string ls_sql = "select memo from mysysset where itemtype=@itemtype and itemname=@itemname";
            ls_value = SqliteHelper.ExecuteScalar(ls_sql, "@itemtype=" + as_type, "@itemname=" + as_item);
            SqlErr = SqliteHelper.SqlErr;
            return ls_value;
        }
        /// <summary>
        /// 返回memo
        /// </summary>
        public static string of_GetMySysSetMemo(CommandType type, string dbconn, string as_type, string as_item)
        {
            CreateSysParamTable(dbconn);
            SqlErr = "";
            string ls_value = "";
            string ls_sql = "select memo from mysysset where itemtype=@itemtype and itemname=@itemname";
            ls_value = SqliteHelper.ExecuteScalar(type, dbconn, ls_sql, "@itemtype=" + as_type, "@itemname=" + as_item);
            SqlErr = SqliteHelper.SqlErr;
            return ls_value;
        }
    }
    /// <summary>
    /// 日志类
    /// </summary>
    public class SqlLog
    {
        /// <summary>
        /// 日志必须放在用户目录下，以便查看与清理
        /// </summary>
        public const string _Path = "/sdcard/mylog/";
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="strAction">标题</param>
        /// <param name="strText">内容</param>
        /// <param name="FolderName">文件夹</param>
        public static void WriteLine(string strAction, string strText, string as_sql, params string[] as_param)
        {
            string path = _Path + "Log/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            string fileFullPath = path + "sqlitedberror.txt";
            StringBuilder str = new StringBuilder();
            str.Append("Action :" + strAction + "\r\n");
            str.Append("Time   :  " + DateTime.Now.ToString("HH:mm:ss.fff") + "\r\n");
            str.Append("Message:" + strText + "\r\n");
            str.Append("Sql:" + as_sql + "\r\n");
            if (as_param != null)
            {
                if (as_param.Length > 0)
                {
                    string ls_param = string.Empty;
                    foreach (string _param in as_param)
                    {
                        ls_param += _param + " ";
                    }
                    str.Append("Param:" + ls_param + "\r\n");
                }
            }
            str.Append("-----------------------" + "\r\n");
            StreamWriter sw = default(StreamWriter);
            if (!File.Exists(fileFullPath))
                sw = File.CreateText(fileFullPath);
            else
                sw = File.AppendText(fileFullPath);
            sw.WriteLine(str.ToString());
            sw.Close();
        }
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="strAction">标题</param>
        /// <param name="strText">内容</param>
        /// <param name="FolderName">文件夹</param>
        public static void WriteLine(string strAction, string strText, string as_sql, SqliteParameter[] as_params)
        {
            string path = _Path + "Log/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            string fileFullPath = path + "sqlitedberror.txt";
            StringBuilder str = new StringBuilder();
            str.Append("Action :" + strAction);
            str.Append("Time   :" + DateTime.Now.ToString("HH:mm:ss.fff") + "\r\n");
            str.Append("Message:" + strText + "\r\n");
            str.Append("Sql:" + as_sql + "\r\n");
            if (as_params != null)
            {
                if (as_params.Length > 0)
                {
                    string ls_param = string.Empty;
                    for (int i = 0; i < as_params.Length; i++)
                    {
                        SqliteParameter _param = as_params[i];
                        ls_param += _param.ParameterName + "=" + _param.Value + " ";
                    }
                    str.Append("Param:" + ls_param + "\r\n");
                }
            }
            str.Append("-----------------------" + "\r\n");
            StreamWriter sw = default(StreamWriter);
            if (!File.Exists(fileFullPath))
                sw = File.CreateText(fileFullPath);
            else
                sw = File.AppendText(fileFullPath);
            sw.WriteLine(str.ToString());
            sw.Close();
        }
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="strAction">标题</param>
        /// <param name="strText">内容</param>
        /// <param name="as_Path">路径</param>
        /// <param name="FolderName">文件夹</param>
        public static void Write(string strAction, string strText, string as_Path, string FolderName = "Log")
        {
            string path = _Path + FolderName + "/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            string fileFullPath = path + "sqlitedberror.txt";
            StringBuilder str = new StringBuilder();
            str.Append("Action :" + strAction);
            str.Append("Time   :" + DateTime.Now.ToString("HH:mm:ss.fff") + "\r\n");
            str.Append("Message:" + strText + "\r\n");
            str.Append("-----------------------" + "\r\n");
            StreamWriter sw = default(StreamWriter);
            if (!File.Exists(fileFullPath))
                sw = File.CreateText(fileFullPath);
            else
                sw = File.AppendText(fileFullPath);
            sw.WriteLine(str.ToString());
            sw.Close();
        }
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="strAction">标题</param>
        /// <param name="strText">内容</param>
        /// <param name="FolderName">文件夹</param>
        public static void Write(string strAction, string strText, string FolderName = "Log")
        {
            string path = _Path + FolderName + "/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            string fileFullPath = path + "sqlitedberror.txt";
            StringBuilder str = new StringBuilder();
            str.Append("Action :" + strAction);
            str.Append("Time   :" + DateTime.Now.ToString("HH:mm:ss.fff") + "\r\n");
            str.Append("Message:" + strText + "\r\n");
            str.Append("-----------------------" + "\r\n");
            StreamWriter sw = default(StreamWriter);
            if (!File.Exists(fileFullPath))
                sw = File.CreateText(fileFullPath);
            else
                sw = File.AppendText(fileFullPath);
            sw.WriteLine(str.ToString());
            sw.Close();
        }
    }
}
