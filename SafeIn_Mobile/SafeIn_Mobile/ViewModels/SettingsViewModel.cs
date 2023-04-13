using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmHelpers;
using MvvmHelpers.Commands;
using SafeIn_Mobile.Helpers;
using SafeIn_Mobile.Models;
using SafeIn_Mobile.Services;
using SafeIn_Mobile.Services.Navigation;
using SafeIn_Mobile.Views;
using Splat;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SafeIn_Mobile.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private readonly IRoutingService _navigationService;
        private readonly ILoginService _loginService;
        private readonly IUserService _userService;
        private readonly IToastService toastService;

        public ICommand LogoutCommand { get; set; }
        public ICommand SaveChangesCommand { get; set; }
        public ICommand DiscardChangesCommand { get; set; }

        private string emailMessage;
        public string EmailMessage { get => emailMessage; set => SetProperty(ref emailMessage, value); }
        private string passwordMessage;
        public string PasswordMessage { get => passwordMessage; set => SetProperty(ref passwordMessage, value); }
        private string userName;
        public string UserName
        {
            get => userName;
            set => SetProperty(ref userName, value);
        }

        private string email;
        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }
        private string currentPassword;
        public string CurrentPassword
        {
            get => currentPassword;
            set => SetProperty(ref currentPassword, value);
        }
        private string newPassword;
        public string NewPassword
        {
            get => newPassword;
            set => SetProperty(ref newPassword, value);
        }
        private string role;
        public string Role
        {
            get => role;
            set => SetProperty(ref role, value);
        }
        private string company;
        public string Company
        {
            get => company;
            set => SetProperty(ref company, value);
        }
        private string errorMessage;

        public string ErrorMessage
        {
            get => errorMessage;
            set => SetProperty(ref errorMessage, value);
        }
        private string userNameValid;
        public string UserNameValid
        {
            get => userNameValid;
            set => SetProperty(ref userNameValid, value);
        }
        private string emailValid;
        public string EmailValid
        {
            get => emailValid;
            set => SetProperty(ref emailValid, value);
        }
        public SettingsViewModel(IRoutingService navigationService = null, ILoginService loginService = null, IUserService userService = null)
        {
            _navigationService = navigationService ?? Locator.Current.GetService<IRoutingService>();
            _loginService = loginService ?? Locator.Current.GetService<ILoginService>();
            _userService = userService ?? Locator.Current.GetService<IUserService>();
            LogoutCommand = new AsyncCommand(Logout);
            SaveChangesCommand = new AsyncCommand(SaveChanges);
            DiscardChangesCommand = new AsyncCommand(GetOriginalProperties);
            GetOriginalProperties();
            toastService = Locator.Current.GetService<IToastService>();
        }
        async Task SaveChanges()
        {
            ClearErrors();
            // validate username and email
            List<Exception> errors = new List<Exception>();
            if (!await UserValidation.UserNameValidAsync(UserName))
            {
                errors.Add(new Exception("User name not valid"));
                UserNameValid = "Only numbers and letters, length 5-10";
            }
            if (!await UserValidation.EmailValidAsync(Email))
            {
                errors.Add(new Exception("Email not valid"));
                EmailValid = "Email not valid";
            }
            if (errors.Count > 0)
            {
                toastService.ShowToast("Incorrect fields");
                return;
            }
            // check if user wants to update password
            UserUpdate updateUser;
            if (NewPassword.Equals(""))
            {
                updateUser = new UserUpdate { UserName = UserName, Email = Email, Password = CurrentPassword, CurrentPassword = CurrentPassword };
            }
            else
            {
                 updateUser = new UserUpdate { UserName = UserName, Email = Email, Password = NewPassword, CurrentPassword = CurrentPassword };
            }
            // create userUpdateRequest
            // update user
            var userUpdateResult = await _userService.UserUpdate(updateUser);
            // validate update response
            if (!userUpdateResult.Success)
            {
                // write error message and return
                ErrorMessage = userUpdateResult.ErrorMessage;
                toastService.ShowToast(Constants.UserErrorUpdatedMessage);
                return;
            }
            // login with new credentials
            var loginResult = await _loginService.LoginAsync(updateUser.Email, updateUser.Password);
            if (!loginResult.Success)
            {
                // logout and go to login page
                await Logout();
                return;
            }
            // save user in securestorage
            var userInfoResult = await _userService.GetUserInfo();
            if (!userInfoResult.Success)
            {
                // logout and go to login page
                await Logout();
                return;
            }
            var newUser = new User { UserName = userInfoResult.UserName, Password = NewPassword, Email = userInfoResult.Email, Company = userInfoResult.Company, Role = userInfoResult.Company };
            var writingSuccess = _userService.WriteUserIntoSecureStorage(newUser);
            if (!writingSuccess)
            {
                // logout and go to login page
                await Logout();
                return;
            }
            // set new data to properties
            GetOriginalProperties();
            toastService.ShowToast(Constants.UserUpdatedMessage);

        }
        public async Task GetOriginalProperties()
        {
            ClearErrors();
            // set inputs to original data from securestorage
            var user = await _userService.GetUserFromSecureStorageAsync();
            Email = user.Email;
            UserName = user.UserName;
            Company = user.Company;
            Role = user.Role;
            CurrentPassword = "";
            NewPassword = "";
        }
        public void ClearErrors()
        {
            UserNameValid = null;
            EmailValid = null;
            ErrorMessage = null;
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
