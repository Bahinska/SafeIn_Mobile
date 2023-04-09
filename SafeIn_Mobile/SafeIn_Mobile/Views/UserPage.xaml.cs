using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SafeIn_Mobile.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static System.Net.Mime.MediaTypeNames;

namespace SafeIn_Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserPage : ContentPage
    {
        public UserPage()
        {
            InitializeComponent();
            ViewModel = new UserViewModel();

            // Set the BindingContext of the page to the ViewModel
            BindingContext = ViewModel;
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            ViewModel.Dispose();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.SetUserDataToView();
            ViewModel.GenerateQrCodeAsync();

            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
            var ViewportHeight = mainDisplayInfo.Height / mainDisplayInfo.Density;
            var ViewportWidth = mainDisplayInfo.Width / mainDisplayInfo.Density;

            if (ViewportHeight <= 640)
            {
                UserImage.HeightRequest = 100;
                UserImage.WidthRequest = 100;
                UserImage.Margin = new Thickness(0, 20, 0, 0);
                EmailLabel.Margin = new Thickness(0, 15, 0, 0);
                EmailLabel.FontSize = 20;
                QrCode.HeightRequest = ViewportWidth - 135;
                Username.FontSize = 36;
                Position.FontSize = 20;
                QrCodeTimerTitle.FontSize = 20;
                QrCodeTimer.FontSize = 30;
            }
            else if (ViewportHeight <= 740)
            {
                UserImage.HeightRequest = 120;
                UserImage.WidthRequest = 120;
                UserImage.Margin = new Thickness(0, 20, 0, 0);
                EmailLabel.Margin = new Thickness(0, 15, 0, 0);
                QrCode.HeightRequest = ViewportWidth - 130;
                QrCodeTimerTitle.FontSize = 22;
                QrCodeTimer.FontSize = 34;
            }
            else if (ViewportHeight <= 850)
            {
                UserImage.HeightRequest = 140;
                UserImage.WidthRequest = 140;
                UserImage.Margin = new Thickness(0, 30, 0, 0);
                EmailLabel.Margin = new Thickness(0, 30, 0, 0);
                QrCode.HeightRequest = ViewportWidth - 100;
                QrCodeTimerTitle.FontSize = 24;
                QrCodeTimer.FontSize = 36;
            }
            else if (ViewportHeight <= 869)
            {
                UserImage.HeightRequest = 140;
                UserImage.WidthRequest = 140;
                UserImage.Margin = new Thickness(0, 30, 0, 0);
                EmailLabel.Margin = new Thickness(0, 30, 0, 0);
                QrCode.HeightRequest = ViewportWidth - 80;
                QrCodeTimerTitle.FontSize = 24;
                QrCodeTimer.FontSize = 36;
            }
        }

        

        private UserViewModel ViewModel { get; set; }

    }
}