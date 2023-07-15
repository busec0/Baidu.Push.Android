using Android.Content;
using Android.Hardware.Usb;
using Android.OS;
using Com.Baidu.Android.Pushservice;
using Com.Busec0.Baiducustomandroidsdk;

namespace AndroidNativeSample;

[Activity(Label = "@string/app_name", MainLauncher = true)]
public class MainActivity : Activity
{
    private static string apiKey = "M4A0fzxA75dGvT1jabFZkjfY";

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        // Set our view from the "main" layout resource
        SetContentView(Resource.Layout.activity_main);

        var pushStartButton = FindViewById<Button>(Resource.Id.pushStartButton);
        pushStartButton.Click += async (s, e) =>
        {
            PushSettings.EnableDebugMode(true);
            //PushManager.StartWork(ApplicationContext,
            //    PushConstants.LoginTypeApiKey, apiKey);

            await BaiduInternalNotificationService.StartAsync();
        };
    }
}
