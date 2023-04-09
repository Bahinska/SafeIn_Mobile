using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SafeIn_Mobile.ViewModels;
using Splat;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SafeIn_Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
            BindingContext = ViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
            var ViewportHeight = mainDisplayInfo.Height / mainDisplayInfo.Density;

            if (ViewportHeight <= 740)
            {
                LogOutButton.Margin = new Thickness(50, 0, 50, 40);
            }
        }

        internal SettingsViewModel ViewModel { get; set; } = Locator.Current.GetService<SettingsViewModel>();
    }
}