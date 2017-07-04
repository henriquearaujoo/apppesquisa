using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using XLabs.Forms;
using XLabs.Ioc;
using XLabs.Platform.Device;
using XLabs.Platform.Services;
using app_pesquisa.Droid.Utils;

namespace app_pesquisa.Droid
{
    //[Activity(Label = "Pesquisa App", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    [Activity(Label = "EZQUEST Pesquisa", Theme = "@android:style/Theme.Holo.Light", Icon = "@drawable/icone2", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : XFormsApplicationDroid
    {
        protected override void OnCreate(Bundle bundle)
        {

			ZXing.Mobile.MobileBarcodeScanner.Initialize(Application);

            base.OnCreate(bundle);

			AndroidUtils.SetMainActicity(this);

            /*SimpleContainer container = new SimpleContainer();
            container.Register<IDevice>(t => AndroidDevice.CurrentDevice);
            container.Register<INetwork>(t => t.Resolve<IDevice>().Network);

            if (!Resolver.IsSet)
                Resolver.SetResolver(container.GetResolver());*/
            
            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());

        }
    }
}

