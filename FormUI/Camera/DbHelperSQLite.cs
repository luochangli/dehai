using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace FormUI.Camera
{
    /// <summary>
    ///     Copyright (C) 2011 Maticsoft
    ///     数据访问基础类(基于SQLite)
    ///     可以用户可以修改满足自己项目的需要。
    /// </summary>
    public abstract class DbHelperSQLite
    {
        //数据库连接字符串(app.config来配置)，可以动态更改connectionString支持多数据库.		
        //public static string connectionString = PubConstant.ConnectionString;
        public static string connectionString = string.Format(@"Data Source ={0}\iVMS-4200Client\EncodeDevice.db;Version=3",
                                                              Environment.CurrentDirectory);

        /// <summary>
        /// 功能: 执行sql返回deteset
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <returns></returns>
        public static DataSet ExecDataSet(string sqlStr)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                SQLiteCommand cmd = conn.CreateCommand();
                cmd.CommandText = sqlStr;
                cmd.CommandType = CommandType.Text;
                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
        }

        
    }
}