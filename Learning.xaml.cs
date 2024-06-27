using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ERepetition
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Learning : Page
    {
        // Khai báo danh sách từ và nghĩa
        private List<(string EnglishWord, string VietnameseMeaning)> words = new List<(string EnglishWord, string VietnameseMeaning)>();

        public Learning()
        {
            this.InitializeComponent();
        }

        // Hàm xử lý khi nhấn nút "Thêm từ"
        private void btnAddWord_Click(object sender, RoutedEventArgs e)
        {
            // Kiểm tra nếu TextBox không rỗng và số lượng từ chưa vượt quá 10
            if (!string.IsNullOrWhiteSpace(txtEnglishWord.Text) && !string.IsNullOrWhiteSpace(txtVietnameseMeaning.Text) && words.Count < 10)
            {
                // Thêm từ và nghĩa vào danh sách
                words.Add((txtEnglishWord.Text, txtVietnameseMeaning.Text));
                // Hiển thị từ và nghĩa trong ListView
                lstWords.Items.Add($"{txtEnglishWord.Text} - {txtVietnameseMeaning.Text}");
                // Xóa nội dung TextBox
                txtEnglishWord.Text = string.Empty;
                txtVietnameseMeaning.Text = string.Empty;

                // Nếu đã nhập đủ 10 từ, kích hoạt nút "Bắt đầu học"
                if (words.Count == 10)
                {
                    btnLearn.Visibility = Visibility.Visible;
                }
                else
                {
                    btnLearn.Visibility = Visibility.Collapsed;
                }    
            }
        }

        // Hàm xử lý khi nhấn nút "Bắt đầu học"
        private void btnLearn_Click(object sender, RoutedEventArgs e)
        {
            // Chuyển đến trang FlashcardsPage và truyền danh sách từ
            Frame.Navigate(typeof(FlashcardsPage), words);
        }
    }
}
