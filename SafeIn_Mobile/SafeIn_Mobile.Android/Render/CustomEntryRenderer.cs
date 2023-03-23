using Android.Content;
using Android.Graphics.Drawables;
using SafeIn_Mobile.CustomStyle;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using SafeIn_Mobile.Droid.Renderers;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace SafeIn_Mobile.Droid.Renderers
{
    public class CustomEntryRenderer : EntryRenderer
    {
        public CustomEntryRenderer(Context context) : base(context)
        {
            AutoPackage = false;
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.Background = new ColorDrawable(Android.Graphics.Color.Transparent);
            }
        }
    }
}