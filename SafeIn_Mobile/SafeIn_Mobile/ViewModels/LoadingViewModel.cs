using System.Runtime.InteropServices;
using System.Threading.Tasks;
using MvvmHelpers;
using SafeIn_Mobile.Services;
using SafeIn_Mobile.Services.Navigation;
using SafeIn_Mobile.Views;
using Splat;

namespace SafeIn_Mobile.ViewModels
{
    class LoadingViewModel : BaseViewModel
    {
        private readonly IRoutingService _routingService;
        private readonly IUserService _userService;
        private readonly ILoginService _loginService;
        private readonly IToastService toastService;
        private readonly IExitService exitService;

        public LoadingViewModel(IRoutingService routingService = null, IUserService userService = null, ILoginService loginService = null)
        {
            _routingService = routingService ?? Locator.Current.GetService<IRoutingService>();
            _userService = userService ?? Locator.Current.GetService<IUserService>();
            _loginService = loginService ?? Locator.Current.GetService<ILoginService>();
            toastService = Locator.Current.GetService<IToastService>();
            exitService = Locator.Current.GetService<IExitService>();
        }

        // Called by the views OnAppearing method
        public async void Init()
        {
            // check if API is running
            var isRunning = await _loginService.ApiHealthCheck();
            if(!isRunning.IsRunning)
            {
                await CloseAppWithError();
            }

            // check if user legged in
            var accessCheckResult = await _loginService.AccessCheckAsync();
            if (accessCheckResult.Success)
            {
                App.IsLoggedIn = true;
                await _routingService.NavigateTo("///main");
            }
            else
            {
                // try to refresh 
                var refresh = await _loginService.RefreshTokensAsync();
                if (refresh.Success)
                {
                    App.IsLoggedIn = true;
                    await _routingService.NavigateTo("///main");
                }
                // logout and go to loginPage
                _loginService.Logout();
                App.IsLoggedIn = false;
                await _routingService.NavigateTo($"///{nameof(LoginPage)}");
            }
        }
        private async Task<bool> CloseAppWithError()
        {
            toastService.ShowToast("External error, check internet connection!");
            // Wait for 3 seconds
            await Task.Delay(3000);
            exitService.ExitApplication();
            return true;
        }
    }
}
