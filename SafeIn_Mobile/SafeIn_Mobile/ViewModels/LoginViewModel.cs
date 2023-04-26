using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MvvmHelpers;
using MvvmHelpers.Commands;
using SafeIn_Mobile.Helpers;
using SafeIn_Mobile.Models;
using SafeIn_Mobile.Services;
using SafeIn_Mobile.Services.Navigation;
using SafeIn_Mobile.Views;
using Splat;
using Xamarin.Forms;

namespace SafeIn_Mobile.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
    
        public string Email { get => email; set => SetProperty(ref email, value); }

        public string Password { get => password; set => SetProperty(ref password, value); }
        public string EmailMessage { get => emailMessage; set => SetProperty(ref emailMessage, value); }
        public string PasswordMessage { get => passwordMessage; set => SetProperty(ref passwordMessage, value); }
        public string CredentialsNotValid { get => credentialsNotValid; set => SetProperty(ref credentialsNotValid, value); }
        public AsyncCommand LoginCommand { get; set; }
        private readonly IRoutingService _navigationService;
        private readonly IUserService _userService;
        private readonly ILoginService _loginService;
        private readonly IToastService toastService;

        private string email;
        private string password;
        private string emailMessage;
        private string passwordMessage;
        private string credentialsNotValid;

        public LoginViewModel(IRoutingService navigationService = null, ILoginService loginService = null, IUserService userService = null)
        {
            _navigationService = navigationService ?? Locator.Current.GetService<IRoutingService>();
            _loginService = loginService ?? Locator.Current.GetService<ILoginService>();
            _userService = userService ?? Locator.Current.GetService<IUserService>();
            Title = "Login Page";
            toastService = Locator.Current.GetService<IToastService>();


            //Email = "roman@gmail.com";
            //Password = "Roman-123";


            OnPropertyChanged();
            LoginCommand = new AsyncCommand(Login);
        }

        async Task Login()
        {
            // validate data
            EmailMessage = "";
            PasswordMessage = "";
            List<Exception> errors = new List<Exception>();
            if (Email == null || Email.Equals(""))
            {
                EmailMessage = Constants.EmailEmptyMessage;
                errors.Add(new Exception("Empty email"));
            }
            if (Password == null || Password.Equals(""))
            {
                PasswordMessage = Constants.PasswordEmptyMessage;
                errors.Add(new Exception("Empty password"));
            }

            // cansle if credential not filled
             if(errors.Count> 0)
            {
                toastService.ShowToast("Not valid fields");
                return;
            }

            // login user and put refresh and access tokens into SecureStorage
            var loginResult = await _loginService.LoginAsync(Email, Password);
            if (!loginResult.Success)
            {
                // handle if login was unsuccessful.NavigationStack.Countl
                CredentialsNotValid = AuthErrorMessages.UncorrectCredentials;
                toastService.ShowToast("Password or email is wrong");
                return;

            }
            App.IsLoggedIn = true;
            // save user in securestorage
            var userInfoResult = await _userService.GetUserInfo();
            if (!userInfoResult.Success)
            {
                // logout and go to login page
                _loginService.Logout();
                toastService.ShowToast("Cannot login");
                return;
            }
            var newUser = new User { UserName = userInfoResult.UserName, Password = Password, Email = userInfoResult.Email, Company = userInfoResult.Company, Role = userInfoResult.Company };
            var writingSuccess = _userService.WriteUserIntoSecureStorage(newUser);
            if (!writingSuccess)
            {
                // logout and go to login page
                _loginService.Logout();
                toastService.ShowToast("Cannot login");
                return;

            }
            Email = null;
            Password= null;
            await _navigationService.NavigateTo($"///main/{nameof(UserPage)}");
        }
    }
}
