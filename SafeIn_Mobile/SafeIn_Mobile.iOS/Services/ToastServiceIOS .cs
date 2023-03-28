using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using SafeIn_Mobile.Services;
using UIKit;

namespace SafeIn_Mobile.iOS.Services
{
    public class ToastServiceIOS : IToastService
    {
        private const double ToastDelay = 0;

        public void ShowToast(string message)
        {
            UIApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                var alertController = UIAlertController.Create(null, message, UIAlertControllerStyle.Alert);

                var delay = NSTimer.CreateScheduledTimer(ToastDelay, (obj) =>
                {
                    alertController.DismissViewController(true, null);
                });

                UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(alertController, true, null);
            });
        }
    }
}