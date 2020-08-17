using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AppUninstall
{
    class Adb
    {
        public static String RunCommand(String CMD)
        {

            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;    //是否使用操作系统shell启动
            p.StartInfo.RedirectStandardInput = true;//接受来自调用程序的输入信息
            p.StartInfo.RedirectStandardOutput = true;//由调用程序获取输出信息
            p.StartInfo.RedirectStandardError = true;//重定向标准错误输出
            p.StartInfo.CreateNoWindow = true;//不显示程序窗口
            p.Start();//启动程序

            //向cmd窗口发送输入信息
            p.StandardInput.WriteLine(CMD+">CmdPipe.pip 2>&1 &exit");
            p.StandardInput.AutoFlush = true;
            p.WaitForExit();//等待程序执行完退出进程
            p.Close();
            string output = File.ReadAllText("CmdPipe.pip");
            return output;
        }

        public static String UninstallApp(AndroidApp app)
        {
            String cmd = "adb shell pm uninstall --user 0 " + app.GetPackageName();
            String ret = RunCommand(cmd);
            return DateTime.Now.ToLocalTime().ToString()+" "+app.GetPackageName()+" Uninstall: "+ret+"\n";
        }
        
        public static String InstallApp(String apppath)
        {
            String cmd = "adb install -r " + apppath;
            //MessageBox.Show(cmd);
            //String ret = "";
            string ret = Adb.RunCommand(cmd);
            return DateTime.Now.ToLocalTime().ToString() + " " + apppath + " Install: " + ret+"\n";
        }

        public static List<AndroidApp> GetAllAndroidApps()
        {
            List<AndroidApp> apps=new List<AndroidApp>();
            String packages = RunCommand("adb shell pm list package");
            String [] temstr = packages.Split("\r\n".ToCharArray(),StringSplitOptions.RemoveEmptyEntries);
            DataTable NameList = DataProcess.GetNameList("package_list.csv");
            foreach(String pcknm in temstr)
            {
                String pckname = pcknm.Replace("package:", "");
                pckname = pckname.Replace("\r", "");
                DataRow  dr = NameList.Rows.Find(pckname);

                String name="";
                String suggestion = "";
                if (dr != null)
                {
                    name = dr.ItemArray[1].ToString();
                    suggestion = dr.ItemArray[2].ToString();
                }
                AndroidApp temapp = new AndroidApp(pckname, name ,suggestion);
         
                apps.Add(temapp);
            }
            return apps;
        }
        
    }
}