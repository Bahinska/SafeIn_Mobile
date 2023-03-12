using System;
using System.Threading.Tasks;
using MvvmHelpers;
using MvvmHelpers.Commands;
using SafeIn_Mobile.Helpers;
using SafeIn_Mobile.Services;
using SafeIn_Mobile.Services.Navigation;
using SafeIn_Mobile.Views;
using Splat;
using Xamarin.Forms;

namespace SafeIn_Mobile.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string EmailMessage { get; set; }
        public string PasswordMessage { get; set; }
        public string CredentialsNotValid { get; set; }
        public AsyncCommand LoginCommand { get; set; }
        private readonly IRoutingService _navigationService;
        private readonly IUserService _userService;
        private readonly ILoginService _loginService;
        public LoginViewModel(IRoutingService navigationService = null, ILoginService loginService = null)
        {
            _navigationService = navigationService ?? Locator.Current.GetService<IRoutingService>();
            _loginService = loginService ?? Locator.Current.GetService<ILoginService>();
            Title = "Login Page";

            //delete
            Email = "user@example.com";
            Password = "Password_5";

            OnPropertyChanged();
            LoginCommand = new AsyncCommand(Login);
        }

        async Task Login()
        {
            // validate data
            var email_correct = await UserValidation.EmailValidAsync(Email);
            var password_correct = await UserValidation.EmailValidAsync(Email);
            if (!email_correct)
            {
                EmailMessage = Constants.EmailNotValidMessage;
            }
            if (!password_correct)
            {
                PasswordMessage = Constants.PasswordEmptyMessage;
            }
            // cansle if credential not valid
            if (!email_correct || !password_correct)
            {
                OnPropertyChanged();
                return;
            }
            // login user and put refresh and access tokens into SecureStorage
            var loginResult = await _loginService.LoginAsync(Email, Password);
            if (!loginResult.Success)
            {
                // handle if login was unsuccessful.NavigationStack.Countl
                CredentialsNotValid = AuthErrorMessages.UncorrectCredentials;
                OnPropertyChanged();
                return;
            }
            App.IsLoggedIn = true;
            await _navigationService.NavigateTo($"///main/{nameof(UserPage)}");
        }
    }
}
