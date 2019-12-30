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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using Tamir.SharpSsh;
using System.Data;

using System.Windows.Forms;

namespace SurveyProject
{
    /// <summary>
    /// Main.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Main : Page
    {
        private static string mysql_str = "server=l.bsks.ac.kr;Database=p201606023;Uid=p201606023;Pwd=pp201606023;Charset=utf8";
        MySqlConnection conn = new MySqlConnection(mysql_str);

        MySqlCommand cmd;
        MySqlDataReader reader;

        List<Admin> admin = new List<Admin>();

        public class Admin
        {
            public String ID { get; set; }
            public String PASS { get; set; }
            public String NAME { get; set; }
            public String LEVEL { get; set; }
        }

        public Main()
        {
            while(conn.State == System.Data.ConnectionState.Closed)
            {
                conn.Open();
            }
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string sql1 = "select * from Admin";
            cmd = new MySqlCommand(sql1, conn);
            reader = cmd.ExecuteReader();
            list1.Items.Clear();

            while(reader.Read())
            {
                admin.Add(new Admin()
                {
                    ID = reader[0].ToString(),
                    PASS = reader[1].ToString(),
                    NAME = reader[2].ToString(),
                    LEVEL = reader[3].ToString(),
                });

                list1.ItemsSource = admin;
                list1.Items.Refresh();
            }
            reader.Close();
            
        }



    }
}
