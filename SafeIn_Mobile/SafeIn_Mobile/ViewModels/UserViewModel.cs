﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using MvvmHelpers;
using Newtonsoft.Json;
using QRCoder;
using SafeIn_Mobile.Helpers;
using SafeIn_Mobile.Services;
using SafeIn_Mobile.Services.Navigation;
using SafeIn_Mobile.Views;
using Splat;
using Xamarin.Essentials;
using Xamarin.Forms;
using Color = System.Drawing.Color;

namespace SafeIn_Mobile.ViewModels
{
    public class UserViewModel : BaseViewModel, IDisposable
    {
        private readonly IUserService _userService;
        private readonly ILoginService _loginService;
        private readonly IRoutingService _navigationService;
        public string ErrorMessage { get; set; }
        private ImageSource qrCode;
        public ImageSource QrCode
        {
            get => qrCode;
            set => SetProperty(ref qrCode, value);
        }

        private string name;
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        private string email;
        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        private DateTime qrCodeExpiration;
        public DateTime QrCodeExpiration
        {
            get => qrCodeExpiration;
            set => SetProperty(ref qrCodeExpiration, value);
        }

        private int timeRemaining;
        public int TimeRemaining
        {
            get => timeRemaining;
            set => SetProperty(ref timeRemaining, value);
        }

        private Timer timer;

        public UserViewModel(string name, string email, IRoutingService navigationService = null, IUserService userService = null, ILoginService loginService = null)
        {
            _userService = userService ?? Locator.Current.GetService<IUserService>();
            _loginService = loginService ?? Locator.Current.GetService<ILoginService>();
            _navigationService = navigationService ?? Locator.Current.GetService<IRoutingService>();
            this.name = name;
            this.email = email;
        }
     
        public async void GenerateQrCodeAsync()
        {
            var email = this.email;
            var accessRights = "User";
           
            // refresh tokens
            var refreshTokenResult = await _loginService.RefreshTokensAsync();
            if (!refreshTokenResult.Success)
            {
                //logout and clear tokens from secure storage
                _loginService.Logout();
                await _navigationService.NavigateTo($"///{nameof(LoginPage)}");
            }
            var accessToken = refreshTokenResult.AccessToken;

            Dictionary<string, string> value = new Dictionary<string, string>
            {
                { "email", email },
                { "access_rights", accessRights },
                { "access_token",accessToken }
            };

            var content = JsonConvert.SerializeObject(value);
            try
            {
                Color color1 = Color.Black; 
                int argb = color1.ToArgb();
                byte[] colorBytes1 = BitConverter.GetBytes(argb);

                Color color2 = Color.White;
                int argb2 = color2.ToArgb();
                byte[] colorBytes2 = BitConverter.GetBytes(argb2);

                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.L);
                PngByteQRCode qRCode = new PngByteQRCode(qrCodeData);
                byte[] qrCodeBytes = qRCode.GetGraphic(10, colorBytes1, colorBytes2);
                QrCode = ImageSource.FromStream(() => new MemoryStream(qrCodeBytes));
                QrCodeExpiration = DateTime.Now.AddSeconds(Constants.QrCodeExpirationTime);
                // Start the timer
                StartTimer();
            }
            catch (Exception)
            {
                ErrorMessage = Constants.QrCodeError;
                OnPropertyChanged();
            }

        }

        public void StartTimer()
        {
            timer?.Dispose();
            timer = new Timer(CheckQrCodeExpiration, null, TimeSpan.Zero, TimeSpan.FromSeconds(1)); 
        }

        private void CheckQrCodeExpiration(object state)
        {
            if (QrCodeExpiration < DateTime.Now)
            {
                // The QR code has expired, regenerate it
                try
                {
                    GenerateQrCodeAsync();
                }
                catch (Exception){}
            }
            else
            {
                // Update the TimeRemaining property
                var timeRemaining = (int)Math.Ceiling((QrCodeExpiration - DateTime.Now).TotalSeconds);
                TimeRemaining = timeRemaining < 0 ? 0 : timeRemaining;
            }
        }

        public void Dispose()
        {
            if (timer != null)
            {
                timer.Dispose();
                timer = null;
            }
        }
    }
}