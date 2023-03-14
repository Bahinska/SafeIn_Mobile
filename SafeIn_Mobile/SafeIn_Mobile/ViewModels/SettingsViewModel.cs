using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmHelpers;
using MvvmHelpers.Commands;
using SafeIn_Mobile.Services;
using SafeIn_Mobile.Services.Navigation;
using SafeIn_Mobile.Views;
using Splat;
using Xamarin.Forms;

namespace SafeIn_Mobile.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private IRoutingService _navigationService;
        private ILoginService _loginService;
        public ICommand LogoutCommand { get; set; }
        public SettingsViewModel(IRoutingService navigationService = null, ILoginService loginService = null)
        {
            _navigationService = navigationService ?? Locator.Current.GetService<IRoutingService>();
            _loginService = loginService ?? Locator.Current.GetService<ILoginService>();
            LogoutCommand = new AsyncCommand(Logout);
        }

        async Task Logout()
        {
            App.IsLoggedIn = false;
            _loginService.Logout();
            await Application.Current.MainPage.Navigation.PopToRootAsync();
            await _navigationService.NavigateTo($"///{nameof(LoginPage)}");
        }
    }
}
