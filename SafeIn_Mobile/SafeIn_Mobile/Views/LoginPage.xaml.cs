using SafeIn_Mobile.ViewModels;
using Splat;
using System;
using System.Diagnostics;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SafeIn_Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            BindingContext = ViewModel;
        }
        internal LoginViewModel ViewModel { get; set; } = Locator.Current.GetService<LoginViewModel>();
        
        protected override async void OnAppearing()
        {
            base.OnAppearing();
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            if (height <= 640)
            {
                LogInButton.Margin = new Thickness(40, 30, 40, 0);
            }
            else if (height <= 740)
            {
                LogInButton.Margin = new Thickness(40, 50, 40, 0);
            }
            else if (height <= 869)
            {
                LogInButton.Margin = new Thickness(40, 100, 40, 0);
            }
            // for tablets
            else if (height <= 960)
            {
                LogInButton.Margin = new Thickness(40, 150, 40, 0);
            }
            else if (height <= 1024)
            {
                LogInButton.Margin = new Thickness(40, 170, 40, 0);
            }
            else if (height <= 1280)
            {
                LogInButton.Margin = new Thickness(40, 200, 40, 0);
            }

            if (width <= 360) 
            {
                OtherInfoLabel.FontSize = 11;
            }

        }

        public async void ButtonClickedEffect(object sender, EventArgs e)
        {
            await LogInButton.ScaleTo(0.99, 100);
            await LogInButton.ScaleTo(1, 100);
        }

    }
}