using System.ComponentModel;
using SafeIn_Mobile.ViewModels;
using Xamarin.Forms;

namespace SafeIn_Mobile.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}