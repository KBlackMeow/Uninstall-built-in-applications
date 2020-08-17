using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
namespace AppUninstall
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //MessageBox.Show(Adb.RunCommand("adb version"));
            AppList.ItemsSource = DataProcess.GetApps().DefaultView;
            //AppList.ItemsSource = DataProcess.GetNameList("huawei_package_list.csv").DefaultView;
        }

        private void AppList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
         
        }

        private void CheckButton_Click(object sender, RoutedEventArgs e)
        {
            foreach(var a in AppList.SelectedItems)
            {
                DataRowView view = (DataRowView)a;
                view.Row[3]= "Checked";
            }
        }

        private void UncheckButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var a in AppList.SelectedItems)
            {
                DataRowView view = (DataRowView)a;
                view.Row[3] = "";
            }
        }

        private void UninstallButton_Click(object sender, RoutedEventArgs e)
        {

            DataTable dt = new DataTable("Uninstall App List");
            dt.Columns.Add("Package Name", typeof(String));
            dt.Columns.Add("App Name", typeof(String));
            dt.Columns.Add(new DataColumn("Check", typeof(String)));
            foreach (DataRowView a in AppList.ItemsSource)
            {
                DataRowView view = a;
                if (view.Row[3].ToString().Equals("Checked"))
                {
                    DataRow dr = dt.NewRow();
                    dr["Package Name"] = view["Package Name"];
                    dr["App Name"] = view["App Name"];
                    dr["Check"] = view["Check"];
                    dt.Rows.Add(dr);
                }
            }

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("没有选中任何程序");
                return;
            }
            MessageBoxResult ret = MessageBox.Show("确认删除"+dt.Rows.Count.ToString()+"项程序吗？", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (ret == MessageBoxResult.OK)
            {
                String log = DataProcess.UninstallAppList(dt);
                //String log = "";
                //MessageBox.Show(dt.Rows.Count.ToString() + "项已删除");
                LogOutput.AppendText(log);
                AppList.ItemsSource = DataProcess.GetApps().DefaultView;
            }
            
        }

        private void InstallBox_Drop(object sender, DragEventArgs e)
        {
            List<String> paths = new List<String>();
            foreach (var a in (System.Array)e.Data.GetData(DataFormats.FileDrop))
            {
                paths.Add("\""+a.ToString()+"\"");
            }
            LogOutput.AppendText(DataProcess.InstallApps(paths));
            AppList.ItemsSource = DataProcess.GetApps().DefaultView;
        }

        private void LogOutput_TextChanged(object sender, TextChangedEventArgs e)
        {
            LogOutput.ScrollToEnd();
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            String help = "1 首先确认手机打开调试模式（不会请百度）\n" +
                "2 手机使用usb链接电脑之后点击【刷新】按钮\n" +
                "3 鼠标选择要卸载的程序，并点击上面的 【选中】 按钮（第三列的【checked】表示选中，【空白】表示没选中），如果选错可以点击【取消选中】\n" +
                "4 点击【确认卸载】即可卸载软件包\n" + 
                "5 右侧的蓝色区域可以用于安装apk";
            MessageBox.Show(help);
        }

        private void Fresh_Click(object sender, RoutedEventArgs e)
        {
            AppList.ItemsSource = DataProcess.GetApps().DefaultView;
        }
    }
}
