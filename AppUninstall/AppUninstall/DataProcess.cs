using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AppUninstall
{
    class DataProcess
    {
        public static DataTable GetApps()
        {
            DataTable dt = new DataTable("App List");
            dt.Columns.Add(new DataColumn("Package Names",typeof(String)));
            dt.Columns.Add(new DataColumn("App Names", typeof(String)));
            dt.Columns.Add(new DataColumn("Suggestion", typeof(String)));
            dt.Columns.Add(new DataColumn("Check", typeof(String)));
            var keys = new DataColumn[1];
            keys[0] = dt.Columns["Package Name"];
            dt.PrimaryKey = keys;
            List<AndroidApp> apps = Adb.GetAllAndroidApps();
            foreach(AndroidApp app in apps)
            {
                DataRow dr = dt.NewRow();
                dr["App Names"] = app.GetName();
                dr["Package Names"] = app.GetPackageName();
                dr["Suggestion"] = app.GetSuggestion();
                dt.Rows.Add(dr);
            }

            return dt;
        }

        public static DataTable GetNameList(String filePath,int n=1)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            StreamReader reader = new StreamReader(filePath, System.Text.Encoding.Default, false);
            int m = 0;
            while (!reader.EndOfStream)
            {
                m = m + 1;
                string str = reader.ReadLine();
                string[] split = str.Split(',');
                if (m == n)
                {
                    System.Data.DataColumn column; //列名
                    for (int c = 0; c < split.Length; c++)
                    {
                        column = new System.Data.DataColumn();
                        column.DataType = System.Type.GetType("System.String");
                        column.ColumnName = split[c];
                        if (dt.Columns.Contains(split[c]))                 //重复列名处理
                            column.ColumnName = split[c] + c;
                        dt.Columns.Add(column);
                    }
                }
                if (m >= n + 1)
                {
                    System.Data.DataRow dr = dt.NewRow();
                    for (int i = 0; i < split.Length; i++)
                    {
                        dr[i] = split[i];
                    }
                    dt.Rows.Add(dr);
                }
            }
            var keys = new DataColumn[1];
            keys[0] = dt.Columns["Package Names"];
            dt.PrimaryKey = keys;
            
            reader.Close();
            return dt;
        }

        public static String UninstallAppList(DataTable apps)
        {
            String log = "";
            List<AndroidApp> applist = ToAndroidAppList(apps);
            foreach (AndroidApp a in applist)
            {
                log += Adb.UninstallApp(a);
            }
            return log+"\n";
        }
        
        public static List<AndroidApp> ToAndroidAppList(DataTable dt)
        {
            List<AndroidApp> apps = new List<AndroidApp>();
            foreach (DataRow dr in dt.Rows)
            {
                AndroidApp app = new AndroidApp(dr[0].ToString(), dr[1].ToString());
                apps.Add(app);
            }
            return apps;
        }

        public static String InstallApps(List<String> paths)
        {
            String log = "";
            foreach (String path in paths)
            {
                log += Adb.InstallApp(path);
            }
            return log;
        }
    }
}
