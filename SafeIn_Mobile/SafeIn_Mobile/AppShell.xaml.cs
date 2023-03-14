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
        }
        public ICommand ExecuteLogout => new Command(async () => await GoToAsync(nameof(LoginPage)));

    }
}
