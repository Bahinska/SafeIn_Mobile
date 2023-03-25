using SafeIn_Mobile.ViewModels;
using Splat;
using System;
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
            //check if already logged in


        }
        public async void ButtonClickedEffect(object sender, EventArgs e)
        {
            await LogInButton.ScaleTo(0.99, 100);
            await LogInButton.ScaleTo(1, 100);
        }

    }
}