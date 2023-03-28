using System;
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
        private IRoutingService _navigationService;
        private ILoginService _loginService;
        private IUserService _userService;

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
        public SettingsViewModel(IRoutingService navigationService = null, ILoginService loginService = null, IUserService userService = null)
        {
            _navigationService = navigationService ?? Locator.Current.GetService<IRoutingService>();
            _loginService = loginService ?? Locator.Current.GetService<ILoginService>();
            _userService = userService ?? Locator.Current.GetService<IUserService>();
            LogoutCommand = new AsyncCommand(Logout);
            SaveChangesCommand = new AsyncCommand(SaveChanges);
            DiscardChangesCommand = new AsyncCommand(GetOriginalProperties);
            GetOriginalProperties();
        }
        async Task SaveChanges()
        {
            // validate inputs
            //EmailMessage = "";
            //PasswordMessage = "";
            //var email_correct = await UserValidation.EmailValidAsync(Email);
            //var password_correct = await UserValidation.PasswordValidAsync(CurrentPassword);
            //if (!email_correct)
            //{
            //    EmailMessage = Constants.EmailNotValidMessage;
            //}
            //if (!password_correct)
            //{
            //    PasswordMessage = Constants.PasswordEmptyMessage;
            //}

            // create userUpdateRequest
            var updateUser = new UserUpdate { UserName = UserName, Email = Email, Password = NewPassword, CurrentPassword = CurrentPassword };
            // update user
            var userUpdateResult = await _userService.UserUpdate(updateUser);
            // validate update response
            if (!userUpdateResult.Success)
            {
                // write error message and return
                ErrorMessage = userUpdateResult.ErrorMessage;
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
            await GetOriginalProperties();
        }
        async Task GetOriginalProperties()
        {
            // set inputs to original data from securestorage
            var user = await _userService.GetUserFromSecureStorageAsync();
            Email = user.Email;
            UserName = user.UserName;
            Company = user.Company;
            Role = user.Role;
            CurrentPassword = "";
            NewPassword = "";
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
