using System.Threading.Tasks;
using System.Windows.Input;
using MvvmHelpers;
using MvvmHelpers.Commands;
using SafeIn_Mobile.Services;
using SafeIn_Mobile.Services.Navigation;
using SafeIn_Mobile.Views;
using Splat;

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
            LogoutCommand = new AsyncCommand(logout);
        }

        async Task logout()
        {
           _loginService.Logout();
            await _navigationService.NavigateTo($"///{nameof(LoadingPage)}");
        }
    }
}
