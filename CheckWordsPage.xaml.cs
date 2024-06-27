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
    public sealed partial class CheckWordsPage : Page
    {
        private List<(string EnglishWord, string VietnameseMeaning)> reviewWords;
        private int currentIndex = 0;
        private int score = 0;
        private DispatcherTimer timer;
        private TimeSpan timeRemaining;

        public CheckWordsPage()
        {
            this.InitializeComponent();
            InitializeTimer();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            // Nhận danh sách từ từ FlashcardsPage
            reviewWords = e.Parameter as List<(string EnglishWord, string VietnameseMeaning)>;
            DisplayMeaning();
        }

        // Hàm khởi tạo timer
        private void InitializeTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timeRemaining = TimeSpan.FromMinutes(1); // Đặt thời gian kiểm tra là 1 phút
            lblTimer.Text = $"Time: {timeRemaining.Minutes}:{timeRemaining.Seconds:D2}";
            timer.Start();
        }

        private void Timer_Tick(object sender, object e)
        {
            timeRemaining = timeRemaining.Subtract(TimeSpan.FromSeconds(1));
            lblTimer.Text = $"Time: {timeRemaining.Minutes}:{timeRemaining.Seconds:D2}";

            if (timeRemaining.TotalSeconds <= 0)
            {
                timer.Stop();
                ShowIncompleteMessage();
            }
        }

        // Hàm hiển thị nghĩa tiếng Việt
        private void DisplayMeaning()
        {
            if (reviewWords.Count > 0 && currentIndex < reviewWords.Count)
            {
                txtMeaning.Text = reviewWords[currentIndex].VietnameseMeaning;
                txtWord.Text = string.Empty;
                lblMessage.Text = string.Empty;
            }
        }

        // Hàm xử lý khi nhấn nút "Kiểm tra"
        private void btnCheckWord_Click(object sender, RoutedEventArgs e)
        {
            if (txtWord.Text.Equals(reviewWords[currentIndex].EnglishWord, StringComparison.OrdinalIgnoreCase))
            {
                lblMessage.Text = "Correct!";
                score++;
                lblScore.Text = $"Score: {score}/10";

                // Chuyển sang từ vựng tiếp theo
                if (currentIndex < reviewWords.Count - 1)
                {
                    currentIndex++;
                    DisplayMeaning();
                }
                else
                {
                    // Kết thúc kiểm tra
                    timer.Stop();
                    ShowCompletionMessage();
                }
            }
            else
            {
                lblMessage.Text = "Incorrect! Try again.";
            }
        }

        // Hàm hiển thị thông báo khi hoàn thành kiểm tra
        private void ShowCompletionMessage()
        {
            lblMessage.Text += "\nChúc mừng! Bạn đã hoàn thành kiểm tra!";
        }

        // Hàm hiển thị thông báo khi không hoàn thành kiểm tra trong thời gian quy định
        private void ShowIncompleteMessage()
        {
            var remainingWords = reviewWords.Skip(currentIndex).Select(w => w.EnglishWord).ToList();
            lblMessage.Text = $"Thời gian đã hết! Bạn chưa hoàn thành kiểm tra.\nTừ còn thiếu: {string.Join(", ", remainingWords)}";
        }

        // Hàm xử lý khi nhấn nút "Quay lại"
        private void btnBackToMain_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Repeat));
        }

        // Hàm xử lý khi nhấn nút "Học lại"
        private void btnLearnAgain_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(FlashcardsPage), reviewWords);
        }
    }
}
