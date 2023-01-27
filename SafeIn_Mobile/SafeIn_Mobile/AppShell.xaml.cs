using System;
using System.Collections.Generic;
using SafeIn_Mobile.ViewModels;
using SafeIn_Mobile.Views;
using Xamarin.Forms;

namespace SafeIn_Mobile
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }

    }
}
