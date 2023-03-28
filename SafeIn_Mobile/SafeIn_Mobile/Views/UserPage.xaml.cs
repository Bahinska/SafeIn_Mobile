using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SafeIn_Mobile.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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
        }
        private UserViewModel ViewModel { get; set; }

    }
}