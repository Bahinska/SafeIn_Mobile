using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SafeIn_Mobile.Services.Navigation
{
    public class ShellRoutingService : IRoutingService
    {
        public ShellRoutingService()
        {
        }

        public Task NavigateTo(string route)
        {
            return Shell.Current.GoToAsync(route);
        }
        public Task NavigateTo(string route,bool clear_stack)
        {
            return Shell.Current.GoToAsync(route, clear_stack);
        }
        public Task GoBack()
        {
            return Shell.Current.Navigation.PopAsync();
        }

        public Task GoBackModal()
        {
            return Shell.Current.Navigation.PopModalAsync();
        }
    }
}
