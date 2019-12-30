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
using MahApps.Metro;
using MahApps.Metro.Controls;

namespace SurveyProject
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        

        public MainWindow()
        {
            InitializeComponent();
        }

        private void stats_Click(object sender, RoutedEventArgs e)
        {
            Stats stat = new Stats();
            this.Content = stat;
        }

        private void admin_Click(object sender, RoutedEventArgs e)
        {
            Admin admin = new Admin();
            this.Content = admin;
        }

        
        

        private void main_Closing(object sender, CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("프로그램을 종료하시겠습니까?(아니오 : 로그인 페이지로 이동)", "", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if(result == MessageBoxResult.Yes)
            {
                MessageBox.Show("종료되었습니다", "");
                Environment.Exit(0);
            }
            else if(result == MessageBoxResult.No)
            {
                e.Cancel = false;
            }
            else if(result == MessageBoxResult.Cancel)
            {
                e.Cancel = true;
                return;
            }
        }

    }
}
