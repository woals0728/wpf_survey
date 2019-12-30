using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using System.IO;
using System.Data;
using MahApps.Metro.Controls;
using MahApps.Metro;

namespace SurveyProject
{
    /// <summary>
    /// Login.xaml�� ���� ��ȣ �ۿ� ��
    /// </summary>
    /// 

    public partial class Login : MetroWindow
    {
        public MainWindow main = new MainWindow();
        private static string mysql_str = "server=l.bsks.ac.kr;Database=p201606023;Uid=p201606023;Pwd=pp201606023;Charset=utf8";
        MySqlConnection conn = new MySqlConnection(mysql_str);

        MySqlCommand cmd;
        MySqlDataReader reader;


        public Login()
        {
            while (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            InitializeComponent();
        }

        private void DarkMode(object sender, RoutedEventArgs e)
        {
            Tuple<AppTheme, Accent> appStyle = ThemeManager.DetectAppStyle(Application.Current);
            ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent("Mauve"), ThemeManager.GetAppTheme("BaseDark"));
            menuHeader.Foreground = new SolidColorBrush(Colors.White);
            menu1.Foreground = new SolidColorBrush(Colors.White);
            menu2.Foreground = new SolidColorBrush(Colors.White);
        }

        private void LightMode(object sender, RoutedEventArgs e)
        {
            Tuple<AppTheme, Accent> appStyle = ThemeManager.DetectAppStyle(Application.Current);
            ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent("Cobalt"), ThemeManager.GetAppTheme("BaseLight"));
            menuHeader.Foreground = new SolidColorBrush(Colors.White);
            menu1.Foreground = new SolidColorBrush(Colors.Black);
            menu2.Foreground = new SolidColorBrush(Colors.Black);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {


            if (idBox.Text == "")
            {
                MessageBox.Show("���̵� �Է��ϼ���.");
                return;
            }
            if (passBox.Password == "")
            {
                MessageBox.Show("��й�ȣ�� �Է��ϼ���.");
                return;
            }

            String sql1 = "select * from Admin where ADMIN_ID = '" + idBox.Text + "'";

            cmd = new MySqlCommand(sql1, conn);

            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                reader.Close();
                String sql2 = "select * from Admin where ADMIN_PASS = '" + passBox.Password + "' and ADMIN_ID = '" + idBox.Text + "'";

                cmd = new MySqlCommand(sql2, conn);
                reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    String admin_name = reader.GetString(2);
                    reader.Close();
                    MessageBox.Show(admin_name + " �� �α��� �Ǿ����ϴ�.", "");
                    passBox.Password = "";
                    idBox.Text = "";
                    Hide();
                    MainWindow win1 = new MainWindow();
                    win1.ShowDialog();
                    win1 = null;
                    Show();

                }
                else
                {
                    reader.Close();
                    MessageBox.Show("��й�ȣ�� Ȯ�����ּ���.");
                }
            }
            else
            {
                reader.Close();
                MessageBox.Show("���̵� �����ϴ�.");
            }
        }

        //  protected override void OnClosed(EventArgs e)
        //  {
        //      base.OnClosed(e);
        //      Application.Current.Shutdown();
        //  }

        private void Login_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}