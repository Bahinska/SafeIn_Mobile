using System;
using System.Collections.Generic;
using System.Windows.Input;
using SafeIn_Mobile.Views;
using Xamarin.Forms;

namespace SafeIn_Mobile
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            //Routing.RegisterRoute(nameof(UserPage), typeof(UserPage));
            Routing.RegisterRoute("main/login", typeof(LoginPage));
            BindingContext = this;
        }
        public ICommand ExecuteLogout => new Command(async () => await GoToAsync("main/login"));

    }
}
