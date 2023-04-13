using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using SafeIn_Mobile.Services;
using UIKit;
using Xamarin.Forms;
namespace SafeIn_Mobile.iOS.Services
{
    public class ExitService : IExitService
    {
        [Obsolete]
        public void ExitApplication()
        {
            UIApplication.SharedApplication.PerformSelector(new ObjCRuntime.Selector("terminateWithSuccess"), null, 0);
        }
    }
}
