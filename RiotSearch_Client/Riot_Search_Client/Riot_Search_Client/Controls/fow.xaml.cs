using System;
using System.Windows;
using System.Windows.Controls;

namespace Riot_Search_Client.Controls
{
    /// <summary>
    /// fow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class fow : UserControl
    {
        public fow()
        {
            InitializeComponent();
        }

        private void BtnDetails_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (fowinfo.Visibility == Visibility.Visible)
                {
                    fowinfo.Visibility = Visibility.Collapsed;
                    btnDetails.Content = "추가정보";
                }
                else
                {
                    fowinfo.Visibility = Visibility.Visible;                  
                    btnDetails.Content = "접기";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"오류 발생: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"BtnDetails_Click 오류: {ex.Message}");
            }
        }
    }
}
