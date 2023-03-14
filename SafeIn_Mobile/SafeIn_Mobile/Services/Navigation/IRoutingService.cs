using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SafeIn_Mobile.Services.Navigation
{
    public interface IRoutingService
    {
        Task GoBack();
        Task GoBackModal();
        Task NavigateTo(string route);
    }
}
