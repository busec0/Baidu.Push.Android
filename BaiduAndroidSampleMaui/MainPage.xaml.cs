﻿#if ANDROID
using Android.OS;
using Com.Baidu.Android.Pushservice;
#endif

namespace BaiduAndroidSampleMaui;

public partial class MainPage : ContentPage
{
    private string _baiduApiKey = "M4A0fzxA75dGvT1jabFZkjfY";

    public MainPage()
    {
        InitializeComponent();
    }

    private async void OnStartPushClicked(object sender, EventArgs e)
    {
        if (DeviceInfo.Platform != DevicePlatform.Android)
        {
            await DisplayAlert("Alert", "This only works on Android!", "OK");
        }
        Console.WriteLine("OnStartPushClicked");

#if ANDROID
        var crtActivity = Platform.CurrentActivity;
        PushSettings.EnableDebugMode(true);
        PushManager.StartWork(crtActivity.ApplicationContext, PushConstants.LoginTypeApiKey, _baiduApiKey);
#endif
    }
}


