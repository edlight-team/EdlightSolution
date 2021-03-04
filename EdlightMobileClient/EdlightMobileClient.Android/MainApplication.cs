using System;
using Android.App;
using Android.Runtime;
using SVG.Forms.Plugin.Droid;

namespace EdlightMobileClient.Droid
{
    [Application(
        Theme = "@style/MainTheme"
        )]
    public class MainApplication : Application
    {
        public MainApplication(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();
            Xamarin.Essentials.Platform.Init(this);
            SvgImageRenderer.Init();
        }
    }
}
