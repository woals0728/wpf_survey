using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Crmf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Obj_list.xaml에 대한 상호 작용 논리
    /// </summary>
    /// 
    public partial class Obj_list : Page
    {
        public Obj_list()
        {
            while (conn.State == System.Data.ConnectionState.Closed)
            {
                conn.Open();
            }
            InitializeComponent();

            listview2();
            disableButton();
            labelHidden();
            
        }

        private static string mysql_str = "server=l.bsks.ac.kr;Database=p201606023;Uid=p201606023;Pwd=pp201606023;Charset=utf8";
        MySqlConnection conn = new MySqlConnection(mysql_str);

        MySqlCommand cmd;
        MySqlDataReader reader;

        List<Obj_List> obj_list = new List<Obj_List>();
        List<Subj_List> subj_list = new List<Subj_List>();
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

        public class Obj_List
        {
            public String NUMBER { get; set; }
            public String TOP_IDX { get; set; }
            public String IDX { get; set; }
            public String SUBJECT { get; set; }
            public String KIND { get; set; }
            public String NUM { get; set; }
            public String VIEW { get; set; }
            public String CH1 { get; set; }
            public String CH2 { get; set; }
            public String CH3 { get; set; }
            public String CH4 { get; set; }
            public String CH5 { get; set; }

        }

        public class Subj_List
        {
            public String NUM { get; set; }
            public String IDX { get; set; }
            public String SUBJECT { get; set; }
            public String KIND { get; set; }
            public String AREA { get; set; }
            public String VIEW { get; set; }
            public String TOP_IDX { get; set; }

        }

        


        private void listview1(string txt)
        {
            string sql1 = "select * from obj_list where top_idx = " + txt;
            cmd = new MySqlCommand(sql1, conn);
            reader = cmd.ExecuteReader();
            obj_list.Clear();
            int nullCheck1 = 0;
            while(reader.Read())
            {
                nullCheck1 = 1;
                obj_list.Add(new Obj_List()
                {
                    NUMBER = reader[0].ToString(),
                    IDX = reader[1].ToString(),
                    TOP_IDX = reader[16].ToString(),
                    SUBJECT = reader[2].ToString(),
                    KIND = reader[3].ToString(),
                    NUM = reader[4].ToString(),
                    VIEW = reader[5].ToString(),
                    CH1 = reader[6].ToString(),
                    CH2 = reader[7].ToString(),
                    CH3 = reader[8].ToString(),
                    CH4 = reader[9].ToString(),
                    CH5 = reader[10].ToString()
                });
                list1.IsEnabled = true;
            }
            if (nullCheck1== 0)
            {
                obj_list.Add(new Obj_List()
                {
                    SUBJECT = "항목이 존재하지 않습니다."
                });
                list1.IsEnabled = false;
            }
            list1.ItemsSource = obj_list;
            list1.Items.Refresh();
            CollectionView view1 = (CollectionView)CollectionViewSource.GetDefaultView(list1.ItemsSource);
            reader.Close();

            string sql2 = "select * from subj_list where top_idx = " + txt;
            cmd = new MySqlCommand(sql2, conn);
            reader = cmd.ExecuteReader();
            subj_list.Clear();
            int nullCheck2 = 0;
            while(reader.Read())
            {
                nullCheck2 = 1;
                subj_list.Add(new Subj_List()
                {
                    NUM = reader[0].ToString(),
                    IDX = reader[1].ToString(),
                    SUBJECT = reader[2].ToString(),
                    KIND = reader[3].ToString(),
                    AREA = reader[4].ToString(),
                    VIEW = reader[5].ToString(),
                    TOP_IDX = reader[6].ToString()
                });
                list3.IsEnabled = true;
            }
            if(nullCheck2 == 0)
            {
                subj_list.Add(new Subj_List()
                {
                    SUBJECT = "항목이 존재하지 않습니다."
                });
                list3.IsEnabled = false;
            }
           
            list3.ItemsSource = subj_list;
            list3.Items.Refresh();
            CollectionView view2 = (CollectionView)CollectionViewSource.GetDefaultView(list3.ItemsSource);
            reader.Close();
        }

        private void listview2()
        {
            int nullCheck = 0;
            string sql = "select * from topsub";
            cmd = new MySqlCommand(sql, conn);
            reader = cmd.ExecuteReader();
            topsub.Clear();
            while(reader.Read())
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
                list3.IsEnabled = true;
                
            }
            if(nullCheck == 0)
            {
                topsub.Add(new TOPSUB()
                {
                    TOPSUBJECT = "항목이 존재하지 않습니다."
                });
                list3.IsEnabled = false;
            }
            list2.ItemsSource = topsub;
            list2.Items.Refresh();
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(list2.ItemsSource);
            reader.Close();
        }

        


        
        

        private void disableButton()
        {
            update.IsEnabled = false;
            delete.IsEnabled = false;
            cancel.IsEnabled = false;
        }

        private void labelHidden()
        {
            subj_plus.Visibility = Visibility.Hidden;
            subj_plus_icon.Visibility = Visibility.Hidden;
            obj_plus.Visibility = Visibility.Hidden;
            obj_plus_icon.Visibility = Visibility.Hidden;
        }

        private void labelVisible()
        {
            subj_plus.Visibility = Visibility.Visible;
            subj_plus_icon.Visibility = Visibility.Visible;
            obj_plus.Visibility = Visibility.Visible;
            obj_plus_icon.Visibility = Visibility.Visible;
        }

        private void enableButton()
        {
            update.IsEnabled = true;
            delete.IsEnabled = true;
            cancel.IsEnabled = true;
        }
           
        private void update_Click(object sender, RoutedEventArgs e)
        {
            if (list1.SelectedIndex == -1 && list3.SelectedIndex != -1)
            {
                if (subj_kind.Text == "")
                {
                    MessageBox.Show("주관식 항목 종류를 선택 해 주세요.");
                    return;
                }
                if (subj_row.Text == "")
                {
                    MessageBox.Show("주관식 항목 가로크기를 작성 해 주세요.");
                    return;
                }
                if (subj_column.Text == "")
                {
                    MessageBox.Show("주관식 항목 세로크기를 작성 해 주세요.");
                    return;
                }
                if (subj_item.Text == "")
                {
                    MessageBox.Show("주관식 항목을 작성 해 주세요.");
                    return;
                }
                if (subj_view.Text == "")
                {
                    MessageBox.Show("주관식 항목 공개여부를 선택 해 주세요.");
                    return;
                }
                string area = subj_row.Text + "x" + subj_column.Text;
                string sql1 = "select * from subj_list where num = " + subj_idx.Text;
                string sql2 = "update subj_list set subject = '" + subj_item.Text + "' , kind = '" + subj_kind.Text + "'" +
                    ", area = '" + area + "', view = '" + subj_view.Text + "' where num =" + subj_idx.Text;
                cmd = new MySqlCommand(sql1, conn);
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    if (MessageBox.Show("선택한 주관식 항목만 수정 하시겠습니까?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        reader.Close();
                        cmd = new MySqlCommand(sql2, conn);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("수정완료");
                        emptyPerform();
                        disableButton();
                    }
                    else
                    {
                        reader.Close();
                    }
                }
            }
            else if (list3.SelectedIndex == -1 && list1.SelectedIndex != -1)
            {
                if (obj_subject.Text == "")
                {
                    MessageBox.Show("객관식 항목을 작성 해 주세요.");
                    return;
                }
                if (obj_kind.Text == "")
                {
                    MessageBox.Show("객관식 항목 종류를 선택 해 주세요.");
                    return;
                }
                if (obj_num.Text == "")
                {
                    MessageBox.Show("객관식 항목 개수를 선택 해 주세요.");
                    return;
                }
                if (obj_view.Text == "")
                {
                    MessageBox.Show("객관식 항목 공개여부를 선택 해 주세요.");
                    return;
                }
                if (item1.Text == "")
                {
                    MessageBox.Show("항목 1 을 작성 해 주세요.");
                    return;
                }
                if (item2.Text == "")
                {
                    MessageBox.Show("항목 2 를 작성 해 주세요.");
                    return;
                }
                if (item3.Text == "")
                {
                    MessageBox.Show("항목 3 을 작성 해 주세요.");
                    return;
                }
                if (item4.Text == "" && item4.IsEnabled == true)
                {
                    MessageBox.Show("항목 4 를 작성 해 주세요.");
                    return;
                }
                if (item5.Text == "" && item5.IsEnabled == true)
                {
                    MessageBox.Show("항목 5 를 작성 해 주세요.");
                    return;
                }
                string sql1 = "select * from obj_list where number = " + obj_idx.Text;
                string sql2 = "update obj_list set subject = '" + obj_subject.Text + "' " +
                    ", kind = '" + obj_kind.Text + "', num = '" + obj_num.Text + "', view = '" + obj_view.Text + "'" +
                    ", ch1 = '" + item1.Text + "', ch2 = '" + item2.Text + "', ch3 = '" + item3.Text + "'" +
                    ", ch4 = '" + item4.Text + "', ch5 = '" + item5.Text + "' where number =" + obj_idx.Text;
                cmd = new MySqlCommand(sql1, conn);
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    if (MessageBox.Show("선택한 객관식 항목만 수정 하시겠습니까?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        reader.Close();
                        cmd = new MySqlCommand(sql2, conn);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("수정완료");
                        emptyPerform();
                        disableButton();
                    }
                    else
                    {
                        reader.Close();
                    }
                }
            }
            else if (list1.SelectedIndex != -1 && list3.SelectedIndex != -1)
            {
                if (obj_subject.Text == "")
                {
                    MessageBox.Show("객관식 항목을 작성 해 주세요.");
                    return;
                }
                if (obj_kind.Text == "")
                {
                    MessageBox.Show("객관식 항목 종류를 선택 해 주세요.");
                    return;
                }
                if (obj_num.Text == "")
                {
                    MessageBox.Show("객관식 항목 개수를 선택 해 주세요.");
                    return;
                }
                if (obj_view.Text == "")
                {
                    MessageBox.Show("객관식 항목 공개여부를 선택 해 주세요.");
                    return;
                }
                if (item1.Text == "")
                {
                    MessageBox.Show("항목 1 을 작성 해 주세요.");
                    return;
                }
                if (item2.Text == "")
                {
                    MessageBox.Show("항목 2 를 작성 해 주세요.");
                    return;
                }
                if (item3.Text == "")
                {
                    MessageBox.Show("항목 3 을 작성 해 주세요.");
                    return;
                }
                if (item4.Text == "" && item4.IsEnabled == true)
                {
                    MessageBox.Show("항목 4 를 작성 해 주세요.");
                    return;
                }
                if (item5.Text == "" && item5.IsEnabled == true)
                {
                    MessageBox.Show("항목 5 를 작성 해 주세요.");
                    return;
                }
                if (subj_kind.Text == "")
                {
                    MessageBox.Show("주관식 항목 종류를 선택 해 주세요.");
                    return;
                }
                if (subj_row.Text == "")
                {
                    MessageBox.Show("주관식 항목 가로크기를 작성 해 주세요.");
                    return;
                }
                if (subj_column.Text == "")
                {
                    MessageBox.Show("주관식 항목 세로크기를 작성 해 주세요.");
                    return;
                }
                if (subj_item.Text == "")
                {
                    MessageBox.Show("주관식 항목을 작성 해 주세요.");
                    return;
                }
                if (subj_view.Text == "")
                {
                    MessageBox.Show("주관식 항목 공개여부를 선택 해 주세요.");
                    return;
                }
                string area = subj_row.Text + "x" + subj_column.Text;
                string sql1 = "update subj_list set subject = '" + subj_item.Text + "' , kind = '" + subj_kind.Text + "'" +
                    ", area = '" + area + "', view = '" + subj_view.Text + "' where num =" + subj_idx.Text;
                string sql2 = "update obj_list set subject = '" + obj_subject.Text + "' " +
                    ", kind = '" + obj_kind.Text + "', num = '" + obj_num.Text + "', view = '" + obj_view.Text + "'" +
                    ", ch1 = '" + item1.Text + "', ch2 = '" + item2.Text + "', ch3 = '" + item3.Text + "'" +
                    ", ch4 = '" + item4.Text + "', ch5 = '" + item5.Text + "' where number =" + obj_idx.Text;
                if (MessageBox.Show("선택한 항목들을 수정 하시겠습니까?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    cmd = new MySqlCommand(sql1, conn);
                    cmd.ExecuteNonQuery();

                    cmd = new MySqlCommand(sql2, conn);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("수정완료");
                    emptyPerform();
                    disableButton();
                }
                else
                {
                    reader.Close();
                }

                
            }
            list1.Items.Refresh();
            list3.Items.Refresh();
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            if (list1.SelectedIndex == -1 && list3.SelectedIndex != -1)
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
                if (subj_kind.Text == "")
                {
                    MessageBox.Show("항목을 다시 선택 해 주세요.");
                    return;
                }
                if (subj_row.Text == "")
                {
                    MessageBox.Show("항목을 다시 선택 해 주세요.");
                    return;
                }
                if (subj_column.Text == "")
                {
                    MessageBox.Show("항목을 다시 선택 해 주세요.");
                    return;
                }
                if (subj_item.Text == "")
                {
                    MessageBox.Show("항목을 다시 선택 해 주세요.");
                    return;
                }
                if (subj_view.Text == "")
                {
                    MessageBox.Show("항목을 다시 선택 해 주세요.8");
                    return;
                }
                string area = subj_row.Text + "x" + subj_column.Text;
                string sql1 = "select * from subj_list where num = " + subj_idx.Text;
                string sql2 = "delete from subj_list where num =" + subj_idx.Text;
                string sql3 = "alter table subj_list auto_increment = 1";
                string sql4 = "set @count = 0";
                string sql5 = "update subj_list set num = @count:=@count+1";
                string sql7 = "set @count = 0";
                string sql6 = "update subj_list set idx = @count:=@count+1 where top_idx = " + top_idx.Text;
                cmd = new MySqlCommand(sql1, conn);
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    if (MessageBox.Show("선택하신 주관식 항목을 삭제 하시겠습니까?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
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
                        cmd = new MySqlCommand(sql7, conn);
                        cmd.ExecuteNonQuery();
                        cmd = new MySqlCommand(sql6, conn);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("삭제되었습니다.");
                        emptyPerform();
                        disableButton();
                    }
                    else
                    {
                        reader.Close();
                    }
                }
            }
            else if (list3.SelectedIndex == -1 && list1.SelectedIndex != -1)
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
                if (obj_subject.Text == "")
                {
                    MessageBox.Show("항목을 다시 선택 해 주세요.");
                    return;
                }
                if (obj_kind.Text == "")
                {
                    MessageBox.Show("항목을 다시 선택 해 주세요.");
                    return;
                }
                if (obj_num.Text == "")
                {
                    MessageBox.Show("항목을 다시 선택 해 주세요.");
                    return;
                }
                if (obj_view.Text == "")
                {
                    MessageBox.Show("항목을 다시 선택 해 주세요.");
                    return;
                }
                if (item1.Text == "")
                {
                    MessageBox.Show("항목을 다시 선택 해 주세요.");
                    return;
                }
                if (item2.Text == "")
                {
                    MessageBox.Show("항목을 다시 선택 해 주세요.");
                    return;
                }
                if (item3.Text == "")
                {
                    MessageBox.Show("항목을 다시 선택 해 주세요.");
                    return;
                }
                if (item4.Text == "" && item4.IsEnabled == true)
                {
                    MessageBox.Show("항목을 다시 선택 해 주세요.");
                    return;
                }
                if (item5.Text == "" && item5.IsEnabled == true)
                {
                    MessageBox.Show("항목을 다시 선택 해 주세요.");
                    return;
                }
                string sql1 = "select * from obj_list where number = " + obj_idx.Text;
                string sql2 = "delete from obj_list where number =" + obj_idx.Text;
                string sql3 = "alter table obj_list auto_increment = 1";
                string sql4 = "set @count = 0";
                string sql5 = "update obj_list set number = @count:=@count+1";
                string sql7 = "set @count = 0";
                string sql6 = "update obj_list set idx = @count:=@count+1 where top_idx = " + top_idx.Text;
                cmd = new MySqlCommand(sql1, conn);
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    if (MessageBox.Show("선택하신 객관식 항목을 삭제 하시겠습니까?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
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
                        cmd = new MySqlCommand(sql7, conn);
                        cmd.ExecuteNonQuery();
                        cmd = new MySqlCommand(sql6, conn);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("삭제되었습니다.");
                        emptyPerform();
                        disableButton();
                    }
                    else
                    {
                        reader.Close();
                    }
                }
            }
            else if (list1.SelectedIndex != -1 && list3.SelectedIndex != -1)
            {
                if (obj_subject.Text == "")
                {
                    MessageBox.Show("항목을 다시 선택 해 주세요.");
                    return;
                }
                if (obj_kind.Text == "")
                {
                    MessageBox.Show("항목을 다시 선택 해 주세요.");
                    return;
                }
                if (obj_num.Text == "")
                {
                    MessageBox.Show("항목을 다시 선택 해 주세요.");
                    return;
                }
                if (obj_view.Text == "")
                {
                    MessageBox.Show("항목을 다시 선택 해 주세요.");
                    return;
                }
                if (item1.Text == "")
                {
                    MessageBox.Show("항목을 다시 선택 해 주세요.");
                    return;
                }
                if (item2.Text == "")
                {
                    MessageBox.Show("항목을 다시 선택 해 주세요.");
                    return;
                }
                if (item3.Text == "")
                {
                    MessageBox.Show("항목을 다시 선택 해 주세요.");
                    return;
                }
                if (item4.Text == "" && item4.IsEnabled == true)
                {
                    MessageBox.Show("항목을 다시 선택 해 주세요.");
                    return;
                }
                if (item5.Text == "" && item5.IsEnabled == true)
                {
                    MessageBox.Show("항목을 다시 선택 해 주세요.");
                    return;
                }
                if (subj_kind.Text == "")
                {
                    MessageBox.Show("항목을 다시 선택 해 주세요.");
                    return;
                }
                if (subj_row.Text == "")
                {
                    MessageBox.Show("항목을 다시 선택 해 주세요.");
                    return;
                }
                if (subj_column.Text == "")
                {
                    MessageBox.Show("항목을 다시 선택 해 주세요.");
                    return;
                }
                if (subj_item.Text == "")
                {
                    MessageBox.Show("항목을 다시 선택 해 주세요.");
                    return;
                }
                if (subj_view.Text == "")
                {
                    MessageBox.Show("항목을 다시 선택 해 주세요.");
                    return;
                }
                string sql1 = "delete from subj_list where num =" + subj_idx;
                string sql2 = "delete from obj_list where number =" + obj_idx;
                string sql3 = "alter table obj_list auto_increment = 1";
                string sql3_1 = "alter table subj_list auto_increment = 1";
                string sql4 = "set @count = 0";
                string sql5 = "update obj_list set number = @count:=@count+1";
                string sql5_1 = "update subj_list set num = @count:@count+1";
                string sql7 = "set @count = 0";
                string sql6 = "update subj_list set idx = @count:=@count+1 where top_idx = " + top_idx.Text;
                string sql6_1 = "update obj_list set idx = @count:=@count+1 where top_idx = " + top_idx.Text;


                if (MessageBox.Show("선택하신 항목들을 삭제 하시겠습니까?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    cmd = new MySqlCommand(sql1, conn);
                    cmd.ExecuteNonQuery();

                    cmd = new MySqlCommand(sql2, conn);
                    cmd.ExecuteNonQuery();
                    cmd = new MySqlCommand(sql3, conn);
                    cmd.ExecuteNonQuery();

                    cmd = new MySqlCommand(sql3_1, conn);
                    cmd.ExecuteNonQuery();
                    cmd = new MySqlCommand(sql4, conn);
                    cmd.ExecuteNonQuery();

                    cmd = new MySqlCommand(sql5, conn);
                    cmd.ExecuteNonQuery();
                    cmd = new MySqlCommand(sql5_1, conn);
                    cmd.ExecuteNonQuery();

                    cmd = new MySqlCommand(sql7, conn);
                    cmd.ExecuteNonQuery();

                    cmd = new MySqlCommand(sql6, conn);
                    cmd.ExecuteNonQuery();
                    cmd = new MySqlCommand(sql6_1, conn);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("삭제되었습니다.");
                    emptyPerform();
                    disableButton();
                }
                else
                {
                    reader.Close();
                }


            }
            list1.Items.Refresh();
            list2.Items.Refresh();
            list3.Items.Refresh();
            
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            emptyPerform();
            disableButton();
            labelHidden();
        }

        private void emptyPerform()
        {
            subject.Text = "";
            date1.Text = "";
            date2.Text = "";
            info.Text = "";
            obj_subject.Text = "";
            obj_kind.Text = "";
            obj_num.Text = "";
            obj_view.Text = "";
            item1.Text = "";
            item2.Text = "";
            item3.Text = "";
            item4.Text = "";
            item5.Text = "";
            subj_row.Text = "";
            subj_column.Text = "";
            subj_item.Text = "";
            subj_kind.Text = "";
            subj_view.Text = "";
            list1.SelectedIndex = -1;
            list2.SelectedIndex = -1;
            list3.SelectedIndex = -1;
            obj_list.Clear();
            list1.ItemsSource = null;
            list1.Items.Refresh();
            subj_list.Clear();
            list3.ItemsSource = null;
            list3.Items.Refresh();
        }

        private void list2_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = (ListView)sender;
            var topsub = (TOPSUB)item.SelectedItem;
            if (topsub == null)
            {
                return;
            }
            String topidx = topsub.TOP_IDX;
            listview1(topidx);
            subject.Text = topsub.TOPSUBJECT;
            date1.Text = topsub.START;
            date2.Text = topsub.END;
            top_idx.Text = topsub.TOP_IDX;
            info.Text = topsub.INFO;
            if(list1.SelectedIndex == -1 || list3.SelectedIndex == -1)
            {
                obj_subject.Text = "";
                obj_kind.Text = "";
                obj_num.Text = "";
                obj_view.Text = "";
                item1.Text = "";
                item2.Text = "";
                item3.Text = "";
                item4.Text = "";
                item5.Text = "";
                subj_row.Text = "";
                subj_column.Text = "";
                subj_item.Text = "";
                subj_kind.Text = "";
                subj_view.Text = "";
                labelVisible();
            }
            disableButton();

        }

        private void list1_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var item = (ListView)sender;
            var obj_list = (Obj_List)item.SelectedItem;
            if (obj_list == null)
            {
                return;
            }
            obj_idx.Text = obj_list.NUMBER;
            obj_subject.Text = obj_list.SUBJECT;
            obj_kind.Text = obj_list.KIND;
            obj_num.Text = obj_list.NUM;
            obj_view.Text = obj_list.VIEW;
            item1.Text = obj_list.CH1;
            item2.Text = obj_list.CH2;
            item3.Text = obj_list.CH3;
            item4.Text = obj_list.CH4;
            item5.Text = obj_list.CH5;
            
            obj_num_check();
            labelHidden();
            enableButton();
        }

        private void list3_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = (ListView)sender;
            var subj_list = (Subj_List)item.SelectedItem;
            if (subj_list == null)
            {
                return;
            }
            subj_idx.Text = subj_list.NUM;
            subj_item.Text = subj_list.SUBJECT;
            subj_kind.Text = subj_list.KIND;
            subj_view.Text = subj_list.VIEW;
            string text = subj_list.AREA;
            string[] result = text.Split(new string[] { "x" }, StringSplitOptions.None);
            subj_row.Text = result[0];
            subj_column.Text = result[1];
            labelHidden();
            enableButton();
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

        

        private void obj_num_check()
        {
            if (obj_num.Text == "3")
            {
                item1.IsEnabled = true;
                item2.IsEnabled = true;
                item3.IsEnabled = true;
                item4.IsEnabled = false;
                item5.IsEnabled = false;
                item4.Text = "";
                item5.Text = "";
            }
            if (obj_num.Text == "4")
            {
                item1.IsEnabled = true;
                item2.IsEnabled = true;
                item3.IsEnabled = true;
                item4.IsEnabled = true;
                item5.IsEnabled = false;
                item5.Text = "";
            }
            if(obj_num.Text == "5")
            {
                item1.IsEnabled = true;
                item2.IsEnabled = true;
                item3.IsEnabled = true;
                item4.IsEnabled = true;
                item5.IsEnabled = true;
            }
        }


        private bool handle = true;
        private void Handle()
        {
            obj_num_check();
        }

        private void obj_num_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (handle)Handle();
            handle = true;
        }

        private void obj_num_DropDownClosed(object sender, EventArgs e)
        {
            ComboBox combo = sender as ComboBox;
            handle = !combo.IsDropDownOpen;
            Handle();
        }

        private void obj_plus_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (obj_subject.Text == "")
            {
                MessageBox.Show("객관식 항목을 작성 해 주세요.");
                return;
            }
            if (obj_kind.Text == "")
            {
                MessageBox.Show("객관식 항목 종류를 선택 해 주세요.");
                return;
            }
            if (obj_num.Text == "")
            {
                MessageBox.Show("객관식 항목 개수를 선택 해 주세요.");
                return;
            }
            if (obj_view.Text == "")
            {
                MessageBox.Show("객관식 항목 공개여부를 선택 해 주세요.");
                return;
            }
            if (item1.Text == "")
            {
                MessageBox.Show("항목 1 을 작성 해 주세요.");
                return;
            }
            if (item2.Text == "")
            {
                MessageBox.Show("항목 2 를 작성 해 주세요.");
                return;
            }
            if (item3.Text == "")
            {
                MessageBox.Show("항목 3 을 작성 해 주세요.");
                return;
            }
            if (item4.Text == "" && item4.IsEnabled == true)
            {
                MessageBox.Show("항목 4 를 작성 해 주세요.");
                return;
            }
            if (item5.Text == "" && item5.IsEnabled == true)
            {
                MessageBox.Show("항목 5 를 작성 해 주세요.");
                return;
            }
            string sql1 = "select * from obj_list where subject = '" + obj_subject.Text + "'";
            string sql2 = "select max(idx) from obj_list where top_idx = " + top_idx.Text;
            int new_idx;
            
            cmd = new MySqlCommand(sql1, conn);
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {

                MessageBox.Show("동일한 객관식 항목이 이미 존재합니다.");
                reader.Close();
            }
            else
            {
                reader.Close();
                cmd = new MySqlCommand(sql2, conn);
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    if (string.IsNullOrEmpty(reader["max(idx)"].ToString()))
                    {

                        new_idx = 1;
                    }
                    else
                    {
                        new_idx = (Int32)reader["max(idx)"] + 1;
                    }
                    string sql3 = "insert into obj_list(idx, subject, kind, num, view, ch1, " +
                    "ch2, ch3, ch4, ch5, ch1_cn, ch2_cn, ch3_cn, ch4_cn, ch5_cn, top_idx) " +
                    "values( " + new_idx + ", '" + obj_subject.Text + "', '" + obj_kind.Text + "', " +
                    "'" + obj_num.Text + "', '" + obj_view.Text + "', '" + item1.Text + "', " +
                    "'" + item2.Text + "', '" + item3.Text + "', '" + item4.Text + "', '" + item5.Text + "', '0', '0', '0', '0', '0', " +
                    "" + top_idx.Text + ")";
                    if (MessageBox.Show("객관식 항목을 추가 하시겠습니까?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        reader.Close();
                        cmd = new MySqlCommand(sql3, conn);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("추가되었습니다.");
                        emptyPerform();
                        disableButton();
                    }
                    else
                    {
                        reader.Close();
                    }
                }
            }
        }

        private void subj_plus_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (subj_kind.Text == "")
            {
                MessageBox.Show("주관식 항목 종류를 선택 해 주세요.");
                return;
            }
            if (subj_row.Text == "")
            {
                MessageBox.Show("주관식 항목 가로크기를 작성 해 주세요.");
                return;
            }
            if (subj_column.Text == "")
            {
                MessageBox.Show("주관식 항목 세로크기를 작성 해 주세요.");
                return;
            }
            if (subj_item.Text == "")
            {
                MessageBox.Show("주관식 항목을 작성 해 주세요.");
                return;
            }
            if (subj_view.Text == "")
            {
                MessageBox.Show("주관식 항목 공개여부를 선택 해 주세요.");
                return;
            }
            string area = subj_row.Text + "x" + subj_column.Text;
            string sql1 = "select * from subj_list where subject = '" + subj_item.Text + "'";
            string sql2 = "select max(idx) from subj_list where top_idx = " + top_idx.Text;
            int new_idx;

            cmd = new MySqlCommand(sql1, conn);
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {

                MessageBox.Show("동일한 주관식 항목이 이미 존재합니다.");
                reader.Close();
            }
            else
            {
                reader.Close();
                cmd = new MySqlCommand(sql2, conn);
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    if (   string.IsNullOrEmpty(reader["max(idx)"].ToString())   )
                    {
                        
                        new_idx = 1;
                    }
                    else
                    {
                        new_idx = (Int32)reader["max(idx)"] + 1;
                    }
                    
                    
                    string sql3 = "insert into subj_list(idx, subject, kind, area, view, top_idx) " +
                    "values( " + new_idx + ", '" + subj_item.Text + "', '" + subj_kind.Text + "', " +
                    "'" + area + "', '" + subj_view.Text + "', " + top_idx.Text + ")";
                    if (MessageBox.Show("주관식 항목을 추가 하시겠습니까?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        reader.Close();
                        cmd = new MySqlCommand(sql3, conn);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("추가되었습니다.");
                        emptyPerform();
                        disableButton();
                    }
                    else
                    {
                        reader.Close();
                    }
                }
            }
        }
    }
}
