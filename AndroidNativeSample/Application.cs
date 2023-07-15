using System;
using Android.Content;
using Com.Busec0.Baiducustomandroidsdk;

namespace AndroidNativeSample
{
    [Application]
    public class MyApplication : Application, IXamarinNotificationDelegate
    {
        public static MyApplication Instance { get; set; }

        public MyApplication(IntPtr handle, Android.Runtime.JniHandleOwnership owner) : base(handle, owner) { }

        public new void OnCreate()
        {
            base.OnCreate();

            Instance = this;
            Console.WriteLine("Just log something here, to make sure it prints");
        }


        public void OnBind(Context? context, int errorCode, string appId, string userId, string channelId, string requestId)
        {
            Console.WriteLine($"onBind: errorCode={errorCode}, appid={appId}, userId={userId}, channelId={channelId}, requestId={requestId}");
        }

        public void OnNotificationMessageClicked(Context? context, string? title, string? description, string? customContent)
        {
            Console.WriteLine($"OnNotificationMessageClicked: title={title}, description={description}, customContent={customContent}");
        }

        public void OnDelTags(Context context, int errorCode, IList<string> successTags, IList<string> failedTags, string requestId)
        {
            Console.WriteLine($"OnDelTags: errorCode={errorCode}, successTags={successTags}, failedTags={failedTags}, requestId={requestId}");
        }

        public void OnListTags(Context context, int errorCode, IList<string> tags, string requestId)
        {
            Console.WriteLine($"OnListTags: errorCode={errorCode}, tags={tags}, requestId={requestId}");
        }

        public void OnMessage(Context context, string message, string customContentString, int notifyId, int source)
        {
            Console.WriteLine($"OnMessage: message={message}, customContentString={customContentString}, notifyId={notifyId}, source={source}");
        }

        public void OnNotificationArrived(Context context, string title, string description, string customContent)
        {
            Console.WriteLine($"OnNotificationArrived: title={title}, description={description}, customContent={customContent}");
        }

        public void OnNotificationClicked(Context context, string title, string description, string customContent)
        {
            Console.WriteLine($"OnNotificationClicked: title={title}, description={description}, customContent={customContent}");
        }

        public void OnSetTags(Context context, int errorCode, IList<string> successTags, IList<string> failedTags, string requestId)
        {
            Console.WriteLine($"OnSetTags: errorCode={errorCode}, successTags={successTags}, failedTags={failedTags}, requestId={requestId}");
        }

        public void OnUnbind(Context context, int errorCode, string requestId)
        {
            Console.WriteLine($"OnUnbind: errorCode={errorCode}, requestId={requestId}");
        }
    }
}

