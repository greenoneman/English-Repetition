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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ERepetition
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FlashcardsPage : Page
    {
        private List<(string EnglishWord, string VietnameseMeaning)> flashcards;
        private int currentIndex = 0;
        private bool isFlipped = false;

        public FlashcardsPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            // Nhận danh sách từ từ MainPage
            flashcards = e.Parameter as List<(string EnglishWord, string VietnameseMeaning)>;
            DisplayWord();
        }

        // Hàm hiển thị từ vựng
        private void DisplayWord()
        {
            if (flashcards.Count > 0 && currentIndex < flashcards.Count)
            {
                txtWord.Text = flashcards[currentIndex].EnglishWord;
                txtMeaning.Text = isFlipped ? flashcards[currentIndex].VietnameseMeaning : string.Empty;
            }
        }

        // Hàm xử lý khi nhấn nút "Lật thẻ"
        private void btnFlipCard_Click(object sender, RoutedEventArgs e)
        {
            isFlipped = !isFlipped;
            DisplayWord();
        }

        // Hàm xử lý khi nhấn nút "Tiếp theo"
        private void btnNextWord_Click(object sender, RoutedEventArgs e)
        {
            if (currentIndex < flashcards.Count - 1)
            {
                currentIndex++;
                isFlipped = false;
                DisplayWord();
            }
            else
            {
                // Nếu đã học hết các từ, hiển thị thông báo
                txtWord.Text = "Finish!";
                txtMeaning.Text = string.Empty;
            }
        }

        // Hàm xử lý khi nhấn nút "Học lại"
        private void btnLearnAgain_Click(object sender, RoutedEventArgs e)
        {
            currentIndex = 0;
            isFlipped = false;
            DisplayWord();
        }

        // Hàm xử lý khi nhấn nút "Quay lại"
        private void btnBackToMain_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Learning));
        }

        // Hàm xử lý khi nhấn nút "Kiểm tra từ"
        private void btnCheckWords_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CheckWordsPage), flashcards);
        }
    }
}
