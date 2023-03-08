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
            var email = SecureStorage.GetAsync("email").Result;
            var name = SecureStorage.GetAsync("password").Result;
            ViewModel = new UserViewModel(name, email);

            // Set the BindingContext of the page to the ViewModel
            BindingContext = ViewModel;
        }
        private UserViewModel ViewModel { get; set; }

    }
}