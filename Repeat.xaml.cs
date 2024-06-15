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
using System.Threading.Tasks;
using Windows.Media.SpeechSynthesis;
using Windows.Media.Playback;
using Windows.UI.Core;
using Windows.Media.Core;
using System.Text.RegularExpressions;
using System.Numerics;
using Windows.Networking.Connectivity;
using static System.Net.Mime.MediaTypeNames;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ERepetition
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Repeat : Page
    {
        private SpeechSynthesizer _speechSynthesizer;
        private MediaPlayer _mediaPlayer;
        private int _repeatCount;
        private int _currentRepeat;
        private string[] _sentencesToSpeak;
        private int _currentSentenceIndex;
        private DispatcherTimer _countdownTimer;
        private TimeSpan _initialCountdownTime;
        private TimeSpan _countdownTime;
        private bool _isRepeating;
        bool state = false;
        int step = 0;
        public Repeat()
        {

            this.InitializeComponent();
            var voices = SpeechSynthesizer.AllVoices.OrderBy(voice => voice.DisplayName).ToList();
            Voice.ItemsSource = voices;
            Voice.DisplayMemberPath = "DisplayName";
            Voice.SelectedIndex = 0;
            _speechSynthesizer = new SpeechSynthesizer();
            _mediaPlayer = new MediaPlayer();
            _mediaPlayer.MediaEnded += MediaPlayer_MediaEnded;

        }

        private void UpButtonm_Click(object sender, RoutedEventArgs e)
        {   
            int value = int.Parse(TimeRepeatm.Text);
            value++;
            TimeRepeatm.Text = value.ToString();
            if (state && step ==3)
            {
                ShapeMinute.Visibility = Visibility.Collapsed;
                shapeMinuteCover.Visibility = Visibility.Visible;
                Minute.Visibility = Visibility.Collapsed;

                ShapeNumber.Visibility = Visibility.Visible;
                ShapeNumbercover.Visibility = Visibility.Collapsed;
                Number.Visibility = Visibility.Visible;
                step++;
            }

        }

        private void DownButtonm_Click(object sender, RoutedEventArgs e)
        {
            int value = int.Parse(TimeRepeatm.Text);
            value--;
            TimeRepeatm.Text = value.ToString();
        }
        private void UpButtons_Click(object sender, RoutedEventArgs e)
        {
            int value = int.Parse(TimeRepeats.Text);
            value++;
            TimeRepeats.Text = value.ToString();
            if (state && step == 2)
            {
                ShapeSecond.Visibility = Visibility.Collapsed;
                ShapeSecondCover.Visibility = Visibility.Visible;
                Second.Visibility = Visibility.Collapsed;

                ShapeMinute.Visibility = Visibility.Visible;
                shapeMinuteCover.Visibility = Visibility.Collapsed;
                Minute.Visibility = Visibility.Visible;
                step++;
            }
        }

        private void DownButtons_Click(object sender, RoutedEventArgs e)
        {
            int value = int.Parse(TimeRepeats.Text);
            value--;
            TimeRepeats.Text = value.ToString();
           
        }
        private void UpButton_Click(object sender, RoutedEventArgs e)
        {
            int value = int.Parse(TimeRepeat.Text);
            value++;
            TimeRepeat.Text = value.ToString();
            if (state && step == 4)
            {
                ShapeNumber.Visibility = Visibility.Collapsed;
                ShapeNumbercover.Visibility = Visibility.Visible;
                Number.Visibility = Visibility.Collapsed;

                ShapeStart.Visibility = Visibility.Visible;
                ShapeStarCover.Visibility = Visibility.Collapsed;
                Start.Visibility = Visibility.Visible;
                step++;


            }

        }

        private void DownButton_Click(object sender, RoutedEventArgs e)
        {
            int value = int.Parse(TimeRepeat.Text);
            value--;
            TimeRepeat.Text = value.ToString();
        }

        private async void SpeakButton_Click(object sender, RoutedEventArgs e)
        {
            string text = TextInEng.Text;
            if (!string.IsNullOrWhiteSpace(text) &&
                int.TryParse(TimeRepeat.Text, out _repeatCount) &&
                _repeatCount > 0 &&
                int.TryParse(TimeRepeatm.Text, out int minutes) &&
                int.TryParse(TimeRepeats.Text, out int seconds))
            {
                _initialCountdownTime = new TimeSpan(0, minutes, seconds);
                _sentencesToSpeak = Regex.Split(text, @"(?<=[\.!\?])\s+");
                _currentRepeat = 0;
                _currentSentenceIndex = 0;
                _isRepeating = true;
                _countdownTime = _initialCountdownTime;
                SpeakButton.IsEnabled = false; // Disable the Speak button
                var selectedVoice = (VoiceInformation)Voice.SelectedItem;
                _speechSynthesizer.Voice = selectedVoice;

                StartCountdown();

            }
            else
            {
                var dialog = new ContentDialog()
                {
                    Title = "Input Required",
                    Content = "Please enter valid text, repeat count, and countdown time.",
                    CloseButtonText = "Ok"
                };
                await dialog.ShowAsync();
            }

        }

        private void StartCountdown()
        {
            Cover.Visibility = Visibility.Collapsed;
            Timer.Visibility = Visibility.Visible;



            if (state && step == 5)
            {
                
                ShapeStart.Visibility = Visibility.Collapsed;
                ShapeStarCover.Visibility = Visibility.Visible;
                Start.Visibility = Visibility.Collapsed;

                ShapeStop.Visibility = Visibility.Visible;
                Stop.Visibility = Visibility.Visible;
                step++;
            }
            

            CountdownTimer.Text = _countdownTime.ToString(@"mm\:ss");
            _countdownTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _countdownTimer.Tick += CountdownTimer_Tick;
            _countdownTimer.Start();

        }

        private async void CountdownTimer_Tick(object sender, object e)
        {
            if (_countdownTime.TotalSeconds > 0)
            {
                _countdownTime = _countdownTime.Subtract(TimeSpan.FromSeconds(1));
                CountdownTimer.Text = _countdownTime.ToString(@"mm\:ss");
            }
            else
            {
                _countdownTimer.Stop();
                await PlayNextSentence();
                
            }
        }

        private async Task PlayNextSentence()
        {
           
            if (_isRepeating)
            {
                if (_currentRepeat < _repeatCount)
                {
                    string sentenceToSpeak = _sentencesToSpeak[_currentSentenceIndex];
                    await PlaySpeechAsync(sentenceToSpeak);
                    _currentSentenceIndex = (_currentSentenceIndex + 1) % _sentencesToSpeak.Length;
                    if (_currentSentenceIndex == 0)
                    {
                        _currentRepeat++;
                    }
                }
                else
                {
                    _currentRepeat = 0;
                    _currentSentenceIndex = 0;
                    _countdownTime = _initialCountdownTime;
                    StartCountdown();
                }
            }
        }

        private async Task PlaySpeechAsync(string text)
        {
            var synthesisStream = await _speechSynthesizer.SynthesizeTextToStreamAsync(text);
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                _mediaPlayer.Source = MediaSource.CreateFromStream(synthesisStream, synthesisStream.ContentType);
                _mediaPlayer.Play();
            });
        }

        private async void MediaPlayer_MediaEnded(MediaPlayer sender, object args)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                await PlayNextSentence();
            });
        }
        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            Cover.Visibility = Visibility.Visible;
            Timer.Visibility = Visibility.Collapsed;

            if (state && step == 6)
            {
                ShapeStop.Visibility = Visibility.Collapsed;
                Stop.Visibility = Visibility.Collapsed;
                
                state = false;

            }
            _isRepeating = false;
            _countdownTimer?.Stop();
            _mediaPlayer.Pause();
            _countdownTime = _initialCountdownTime; // Reset the countdown time to initial value
            CountdownTimer.Text = _countdownTime.ToString(@"mm\:ss");
            SpeakButton.IsEnabled = true; // Re-enable the Speak button
        }
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var Statestructions = e.Parameter;
            if( Statestructions!=null )
            {
                ShapeTheVoice.Visibility = Visibility.Visible;
                ShapeTheVoiceCover.Visibility = Visibility.Collapsed;
                theVoice.Visibility = Visibility.Visible;
                state = true;
                
            }
            else
            {
                state= false;
            }    
            
        }

        private void Choice_TheVoice(object sender, SelectionChangedEventArgs e)
        {
            if (state && step == 0)
            {
                ShapeTheVoice.Visibility = Visibility.Collapsed;
                ShapeTheVoiceCover.Visibility = Visibility;
                theVoice.Visibility = Visibility.Collapsed;

                ShapeText.Visibility = Visibility.Visible;
                ShapeTextCover.Visibility = Visibility.Collapsed;
                Text.Visibility = Visibility.Visible;
                step++;
            }
        }



       

        private void complete_text(object sender, TextChangedEventArgs e)
        {
            if (state && step == 1 )
            {
                ShapeText.Visibility = Visibility.Collapsed;
                ShapeTextCover.Visibility = Visibility.Visible;
                Text.Visibility= Visibility.Collapsed;

                ShapeSecond.Visibility = Visibility.Visible;
                ShapeSecondCover.Visibility = Visibility.Collapsed;
                Second.Visibility = Visibility.Visible;
                step++;

            }
        }

       
    }









}

