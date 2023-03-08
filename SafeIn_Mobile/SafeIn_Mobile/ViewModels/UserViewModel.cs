using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using MvvmHelpers;
using Newtonsoft.Json;
using QRCoder;
using SafeIn_Mobile.Helpers;
using SafeIn_Mobile.Services;
using Splat;
using Xamarin.Essentials;
using Xamarin.Forms;
using Color = System.Drawing.Color;

namespace SafeIn_Mobile.ViewModels
{
    public class UserViewModel : BaseViewModel, IDisposable
    {
        private IUserService _userService;
        private readonly ILoginService _loginService;

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
        private string info;
        public string Info
        {
            get => info;
            set => SetProperty(ref info, value);
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

        public UserViewModel(string name, string email, IUserService userService = null, ILoginService loginService = null)
        {
            _userService = userService ?? Locator.Current.GetService<IUserService>();
            _loginService = loginService ?? Locator.Current.GetService<ILoginService>();

            this.name = name;
            this.email = email;

            GenerateQrCodeAsync();
        }

        public async void GenerateQrCodeAsync()
        {
            var email = this.email;
            var accessRights = "User";
            var accessToken = await SecureStorage.GetAsync(Constants.AccessToken);
            if (string.IsNullOrEmpty(accessToken))
            {
                throw new Exception(AuthErrorMessages.TokensOutdated);
            }
            // todo Verify accessToken
            bool tokenValid = await _loginService.AuthCheck();
            if (!tokenValid)
            {
                throw new Exception(AuthErrorMessages.TokensOutdated);
            }
            Dictionary<string, string> value = new Dictionary<string, string>
            {
                { "email", email },
                { "access_rights", accessRights },
                { "access_token",accessToken }
            };
            var content = JsonConvert.SerializeObject(value);
            try
            {
                Color color1 = Color.DarkRed; // replace with your desired color
                int argb = color1.ToArgb();
                byte[] colorBytes1 = BitConverter.GetBytes(argb);// 4 bytes for red, green, blue, and alpha

                Color color2 = Color.PaleGreen; // replace with your desired color
                int argb2 = color2.ToArgb();
                byte[] colorBytes2 = BitConverter.GetBytes(argb2);


                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.L);
                PngByteQRCode qRCode = new PngByteQRCode(qrCodeData);
                byte[] qrCodeBytes = qRCode.GetGraphic(10, colorBytes1, colorBytes2);
                QrCode = ImageSource.FromStream(() => new MemoryStream(qrCodeBytes));
                Info = content;


                QrCodeExpiration = DateTime.Now.AddSeconds(10);

                // Start the timer
                StartTimer();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        private void StartTimer()
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
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
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