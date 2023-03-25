using Android.Graphics;
using SafeIn_Mobile.Droid.Renderers;
using SafeIn_Mobile.CustomStyle;
using System.ComponentModel;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Android.Content;

[assembly: ExportRenderer(typeof(GradientLabel), typeof(GradientLabelRenderer))]
namespace SafeIn_Mobile.Droid.Renderers
{
    public class GradientLabelRenderer : LabelRenderer
    {
        public GradientLabelRenderer(Context context) : base(context)
        {
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);
            SetColors();
        }
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            SetColors();
        }
        private void SetColors()
        {
            var c1 = (Element as GradientLabel).TextColor1.ToAndroid();
            var c2 = (Element as GradientLabel).TextColor2.ToAndroid();
            Shader myShader = new LinearGradient(
                0, 0, Control.MeasuredWidth, 0,
                c1, c2,
                Shader.TileMode.Clamp);
            Control.Paint.SetShader(myShader);
            Control.Invalidate();
        }
    }
}