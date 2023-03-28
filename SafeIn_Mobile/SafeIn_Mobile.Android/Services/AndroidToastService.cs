using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SafeIn_Mobile.Droid.Services;
using SafeIn_Mobile.Services;
[assembly: Xamarin.Forms.Dependency(typeof(AndroidToastService))]
namespace SafeIn_Mobile.Droid.Services
{
    public class AndroidToastService : IToastService
    {
        public void ShowToast(string message)
        {
            Toast.MakeText(Android.App.Application.Context, message, ToastLength.Short).Show();
        }
    }
}