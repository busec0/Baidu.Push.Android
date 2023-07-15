using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Com.Busec0.Baiducustomandroidsdk;

namespace BaiduAndroidSampleMaui;

[Application]
public class MainApplication : MauiApplication
{
    public static MainApplication Instance { get; set; }

    public MainApplication(IntPtr handle, JniHandleOwnership ownership)
        : base(handle, ownership)
    {
    }

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    public override void OnCreate()
    {
        try
        {
            base.OnCreate();
        }catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}

