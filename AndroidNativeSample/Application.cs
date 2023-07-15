using System;
using Android.Content;
using Com.Busec0.Baiducustomandroidsdk;

namespace AndroidNativeSample
{
    [Application]
    public class MyApplication : Application
    {
        public static MyApplication Instance { get; set; }

        public MyApplication(IntPtr handle, Android.Runtime.JniHandleOwnership owner) : base(handle, owner) { }

        public new void OnCreate()
        {
            base.OnCreate();

            Instance = this;
            Console.WriteLine("Just log something here, to make sure it prints");
        }
    }
}

