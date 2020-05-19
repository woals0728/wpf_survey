using MySql.Data.MySqlClient;
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

namespace SurveyProject
{
    /// <summary>
    /// Survey_manage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Survey_manage : Page
    {
        public Survey_manage()
        {
            while (conn.State == System.Data.ConnectionState.Closed)
            {
                conn.Open();
            }
            InitializeComponent();
            list();
            buttonEnable();
            insert_button.IsEnabled = true;
        }

        private static string mysql_str = "server=l.bsks.ac.kr;Database=p201606023;Uid=p201606023;Pwd=pp201606023;Charset=utf8";
        MySqlConnection conn = new MySqlConnection(mysql_str);

        MySqlCommand cmd;
        MySqlDataReader reader;

        List<TOPSUB> topsub = new List<TOPSUB>();

        public class TOPSUB
        {
            public String NUM { get; set; }
            public String TOP_IDX { get; set; }
            public String TOPSUBJECT { get; set; }
            public String START { get; set; }
            public String END { get; set; }
            public String INFO { get; set; }
        }

        private void list()
        {
            int nullCheck = 0;
            string sql = "select * from topsub";
            cmd = new MySqlCommand(sql, conn);
            reader = cmd.ExecuteReader();
            topsub.Clear();
            while (reader.Read())
            {
                nullCheck = 1;
                topsub.Add(new TOPSUB()
                {
                    NUM = reader[0].ToString(),
                    TOP_IDX = reader[1].ToString(),
                    TOPSUBJECT = reader[2].ToString(),
                    START = reader[3].ToString().Substring(0, 10),
                    END = reader[4].ToString().Substring(0, 10),
                    INFO = reader[5].ToString()
                });
            }
            if (nullCheck == 0)
            {
                topsub.Add(new TOPSUB()
                {
                    TOPSUBJECT = "항목이 존재하지 않습니다."
                });
            }
            subj_list.ItemsSource = topsub;
            subj_list.Items.Refresh();
            reader.Close();
        }

        private void buttonEnable()
        {
            insert_button.IsEnabled = false;
            update_button.IsEnabled = false;
            delete_button.IsEnabled = false;
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (date1.SelectedDate.HasValue)
            {
                date2.DisplayDateStart = date1.SelectedDate.Value;
            }

            if (date2.SelectedDate.HasValue)
            {
                date1.DisplayDateEnd = date2.SelectedDate.Value;
            }
        }

        private void subj_list_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var item = (ListView)sender;
            var topsub = (TOPSUB)item.SelectedItem;
            if (topsub == null)
            {
                return;
            }
            String topidx = topsub.TOP_IDX;
            subject.Text = topsub.TOPSUBJECT;
            date1.Text = topsub.START;
            date2.Text = topsub.END;
            top_idx.Text = topsub.TOP_IDX;
            info.Text = topsub.INFO;
            insert_button.IsEnabled = false;
            delete_button.IsEnabled = true;
            update_button.IsEnabled = true;
        }

        private void update_button_Click(object sender, RoutedEventArgs e)
        {
            if (date1.Text == "")
            {
                MessageBox.Show("시작 날짜를 선택 해 주세요.");
                return;
            }
            if (date2.Text == "")
            {
                MessageBox.Show("종료 날짜를 선택 해 주세요.");
                return;
            }
            if (subject.Text == "")
            {
                MessageBox.Show("설문 주제를 입력 해 주세요.");
                return;
            }
            if (info.Text == "")
            {
                MessageBox.Show("도움말을 입력 해 주세요.");
                return;
            }
            string sql1 = "select * from topsub where top_idx = " + top_idx.Text;
            string sql2 = "update topsub set topsubject = '" + subject.Text + "', start = " +
                "'" + date1.Text + "', end = '" + date2.Text + "', info = '" + info.Text + "' where top_idx = " + top_idx.Text;
            cmd = new MySqlCommand(sql1, conn);
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                if (MessageBox.Show("설문 주제를 수정 하시겠습니까?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    reader.Close();
                    cmd = new MySqlCommand(sql2, conn);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("수정완료");
                    topsub.Clear();
                    subj_list.ItemsSource = null;
                    subj_list.Items.Refresh();
                    list();
                    buttonEnable();
                    enableText();
                }
                else
                {
                    reader.Close();
                }
            }
            subj_list.Items.Refresh();
        }
        


        private void insert_button_Click(object sender, RoutedEventArgs e)
        {
            if (date1.Text == "")
            {
                MessageBox.Show("시작 날짜를 선택 해 주세요.");
                return;
            }
            if (date2.Text == "")
            {
                MessageBox.Show("종료 날짜를 선택 해 주세요.");
                return;
            }
            if (subject.Text == "")
            {
                MessageBox.Show("설문 주제를 입력 해 주세요.");
                return;
            }
            if (info.Text == "")
            {
                MessageBox.Show("도움말을 입력 해 주세요.");
                return;
            }
            string sql1 = "select * from topsub where topsubject = '" + subject.Text + "'";
            string sql2 = "select max(top_idx) from topsub";
            int new_idx;

            cmd = new MySqlCommand(sql1, conn);
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {

                MessageBox.Show("동일한 설문 주제가 이미 존재합니다.");
                reader.Close();
            }
            else
            {
                reader.Close();
                cmd = new MySqlCommand(sql2, conn);
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    new_idx = (Int32)reader["max(top_idx)"] + 1;
                    string sql3 = "insert into topsub(top_idx, topsubject, start, end, info) " +
                        "values ( " + new_idx + ", '" + subject.Text + "', '" + date1.Text + "', '" + date2.Text + "', '" + info.Text + "')";
                    if (MessageBox.Show("설문 주제를 추가 하시겠습니까?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        reader.Close();
                        cmd = new MySqlCommand(sql3, conn);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("추가되었습니다.");
                        topsub.Clear();
                        subj_list.ItemsSource = null;
                        subj_list.Items.Refresh();
                        list();
                        buttonEnable();
                        enableText();
                    }
                    else
                    {
                        reader.Close();
                    }
                }
            }
            subj_list.Items.Refresh();
        }

        private void delete_button_Click(object sender, RoutedEventArgs e)
        {
            if (date1.Text == "")
            {
                MessageBox.Show("항목을 다시 선택 해 주세요.");
                return;
            }
            if (date2.Text == "")
            {
                MessageBox.Show("항목을 다시 선택 해 주세요.");
                return;
            }
            if (subject.Text == "")
            {
                MessageBox.Show("항목을 다시 선택 해 주세요.");
                return;
            }
            if(info.Text == "")
            {
                MessageBox.Show("항목을 다시 선택 해 주세요.");
                return;
            }
            string sql1 = "select * from topsub where top_idx = " + top_idx.Text;
            string sql2 = "delete from topsub where top_idx = " + top_idx.Text;
            string sql3 = "alter table topsub auto_increment = 1";
            string sql4 = "set @count = 0;";
            string sql5 = "update topsub set num = @count:=@count+1";
            cmd = new MySqlCommand(sql1, conn);
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                if (MessageBox.Show("설문 주제를 삭제 하시겠습니까?" + Environment.NewLine + "선택한 주제의 항목이 모두 삭제됩니다.", "", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    reader.Close();
                    cmd = new MySqlCommand(sql2, conn);
                    cmd.ExecuteNonQuery();
                    cmd = new MySqlCommand(sql3, conn);
                    cmd.ExecuteNonQuery();
                    cmd = new MySqlCommand(sql4, conn);
                    cmd.ExecuteNonQuery();
                    cmd = new MySqlCommand(sql5, conn);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("삭제되었습니다.");
                    topsub.Clear();
                    subj_list.ItemsSource = null;
                    subj_list.Items.Refresh();
                    list();
                    buttonEnable();
                    enableText();
                }
                else
                {
                    reader.Close();
                }
            }
            subj_list.Items.Refresh();
        }

        private void enableText()
        {
            subject.Text = "";
            date1.Text = "";
            date2.Text = "";
            info.Text = "";
        }

        private void cancel_button_Click(object sender, RoutedEventArgs e)
        {
            enableText();
            insert_button.IsEnabled = true;
            update_button.IsEnabled = false;
            delete_button.IsEnabled = false;
        }
    }
}
