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
using Xamarin.Forms;

[assembly: Dependency(typeof(ExitService))]

namespace SafeIn_Mobile.Droid.Services
{
    public class ExitService : IExitService
    {
        [Obsolete]
        public void ExitApplication()
        {
            var activity = (Activity)Forms.Context;
            activity.FinishAffinity();
            System.Environment.Exit(0);
        }
    }
}