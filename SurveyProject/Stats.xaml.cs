using LiveCharts;
using LiveCharts.Wpf;
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
    /// Stats.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Stats : Page
    {
        public Stats()
        {
            while (conn.State == System.Data.ConnectionState.Closed)
            {
                conn.Open();
            }
            InitializeComponent();
            list();
        }

        private static string mysql_str = "server=l.bsks.ac.kr;Database=p201606023;Uid=p201606023;Pwd=pp201606023;Charset=utf8";
        MySqlConnection conn = new MySqlConnection(mysql_str);

        MySqlCommand cmd;
        MySqlDataReader reader;

        List<TOPSUB> topsub = new List<TOPSUB>();
        List<VIEW_LIST> view_list = new List<VIEW_LIST>();

        public class TOPSUB
        {
            public String NUM { get; set; }
            public String TOP_IDX { get; set; }
            public String TOPSUBJECT { get; set; }
            public String START { get; set; }
            public String END { get; set; }
            public String INFO { get; set; }
            public String DAY { get; set; }
            public String JOIN { get; set; }
            public String SIGN { get; set; }
            public String MEM { get; set; }
        }

        public class VIEW_LIST
        {
            public String NUM { get; set; }
            public String SUBJECT { get; set; }
            public String CH1 { get; set; }
            public String CH2 { get; set; }
            public String CH3 { get; set; }
            public String CH4 { get; set; }
            public String CH5 { get; set; }
        }

        private void list()
        {
            int nullCheck = 0;
            string sql = "select topsub.num, topsub.top_idx, topsubject, start, end, " +
                "convert(concat(to_days(date_format(end, '%Y-%m-%d'))-to_days(date_format(now(), '%Y-%m-%d')),'일') using 'utf8'), " +
                "concat(round((count(sm.stdno) / (select count(stdno) from survey_member)) * 100),'%'), " +
                "sign(to_days(date_format(end, '%Y-%m-%d')) - to_days(date_format(now(), '%Y-%m-%d')))," +
                "convert(concat((select count(stdno) from survey_member),'명') using 'utf8')" +
                "from topsub left outer join survey_answer sa on topsub.top_idx = sa.top_idx " +
                "left outer join survey_member sm on sa.stdno = sm.stdno group by topsubject order by num";
            cmd = new MySqlCommand(sql, conn);
            reader = cmd.ExecuteReader();
            topsub.Clear();
            while (reader.Read())
            {
                nullCheck = 1;
                if(reader[7].ToString() == "-1")
                {
                    topsub.Add(new TOPSUB()
                    {
                        NUM = reader[0].ToString(),
                        TOP_IDX = reader[1].ToString(),
                        TOPSUBJECT = reader[2].ToString(),
                        START = reader[3].ToString().Substring(0, 10),
                        END = reader[4].ToString().Substring(0, 10),
                        DAY = "기간 종료",
                        JOIN = reader[6].ToString(),
                        SIGN = reader[7].ToString(),
                        MEM = reader[8].ToString()
                    }); 
                }
                else
                {
                    topsub.Add(new TOPSUB()
                    {
                        NUM = reader[0].ToString(),
                        TOP_IDX = reader[1].ToString(),
                        TOPSUBJECT = reader[2].ToString(),
                        START = reader[3].ToString().Substring(0, 10),
                        END = reader[4].ToString().Substring(0, 10),
                        DAY = reader[5].ToString(),
                        JOIN = reader[6].ToString(),
                        SIGN = reader[7].ToString(),
                        MEM = reader[8].ToString()
                    });
                }
                
            }
            if (nullCheck == 0)
            {
                topsub.Add(new TOPSUB()
                {
                    TOPSUBJECT = "항목이 존재하지 않습니다."
                });
            }
            list1.ItemsSource = topsub;
            list1.Items.Refresh();
            reader.Close();
        }

        private void listview(string txt)
        {
            string sql1 = "select idx, subject, convert(concat(ch1, '(', ch1_cn, ')') using 'utf8'), " +
                "convert(concat(ch2, '(', ch2_cn, ')') using 'utf8'), convert(concat(ch3, '(', ch3_cn, ')') using 'utf8'), " +
                "convert(concat(ch4, '(', ch4_cn, ')') using 'utf8'), " +
                "convert(concat(ch5, '(', ch5_cn, ')') using 'utf8') from obj_list where top_idx = " + txt;

            cmd = new MySqlCommand(sql1, conn);
            reader = cmd.ExecuteReader();
            view_list.Clear();
            int nullCheck = 0;
            while (reader.Read())
            {
                nullCheck = 1;
                if(reader[6].ToString() == "(0)")
                {
                    if(reader[5].ToString() == "(0)")
                    {
                        view_list.Add(new VIEW_LIST()
                        {
                            NUM = reader[0].ToString(),
                            SUBJECT = reader[1].ToString(),
                            CH1 = reader[2].ToString(),
                            CH2 = reader[3].ToString(),
                            CH3 = reader[4].ToString()
                        });
                    }
                    else
                    {
                        view_list.Add(new VIEW_LIST()
                        {
                            NUM = reader[0].ToString(),
                            SUBJECT = reader[1].ToString(),
                            CH1 = reader[2].ToString(),
                            CH2 = reader[3].ToString(),
                            CH3 = reader[4].ToString(),
                            CH4 = reader[5].ToString()
                        });
                    }
                }
                else
                {
                    view_list.Add(new VIEW_LIST()
                    {
                        NUM = reader[0].ToString(),
                        SUBJECT = reader[1].ToString(),
                        CH1 = reader[2].ToString(),
                        CH2 = reader[3].ToString(),
                        CH3 = reader[4].ToString(),
                        CH4 = reader[5].ToString(),
                        CH5 = reader[6].ToString()
                    });
                }
                list2.IsEnabled = true;
            }
            if (nullCheck == 0)
            {
                view_list.Add(new VIEW_LIST()
                {
                    SUBJECT = "항목이 존재하지 않습니다."
                });
                list2.IsEnabled = false;
            }
            list2.ItemsSource = view_list;
            list2.Items.Refresh();
            reader.Close();
        }

        private void list1_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var item = (ListView)sender;
            var topsub = (TOPSUB)item.SelectedItem;
            if (topsub == null)
            {
                return;
            }
            string top_idx = topsub.TOP_IDX;
            listview(top_idx);

        }

        private void list2_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
