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
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            ContentFrame.Navigate(typeof(AppInfor));
        }
        private void MyNavigationView_ItemInvoked(Windows.UI.Xaml.Controls.NavigationView
sender, NavigationViewItemInvokedEventArgs args)
        {
            FrameNavigationOptions navigationOptions = new FrameNavigationOptions();
            //Tạo đối tượng Frame để chuyển hướng
            navigationOptions.TransitionInfoOverride = args.RecommendedNavigationTransitionInfo; //Chọn hiệu ứng chuyển trang thích hợp
            var selectedItem = MyNavigationView.SelectedItem; //Kiểm tra Item đượcchọn
            if (selectedItem == AppInfor)
            {
                ContentFrame.Navigate(typeof(AppInfor),navigationOptions); 
            }
            else if (selectedItem == AuthorTeam)
            {
                ContentFrame.Navigate(typeof(AuthorTeam));
            }
            else if (selectedItem == Repeat)
            {
                ContentFrame.Navigate(typeof(Repeat));
            }
            else if (selectedItem == Review)
            {
                ContentFrame.Navigate(typeof(Review));
            }

        }
        private void MyNavigationView_BackRequested(Windows.UI.Xaml.Controls.NavigationView sender,NavigationViewBackRequestedEventArgs args)
        {
            if (ContentFrame.CanGoBack)
            {
                ContentFrame.GoBack();
            }
        }

    }
}
