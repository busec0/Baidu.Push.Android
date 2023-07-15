using Android.App;
using Android.Content;
using Com.Baidu.Android.Pushservice;
using Com.Busec0.Baiducustomandroidsdk;

namespace BaiduAndroidSampleMaui.Platforms.Android
{
    public class SamplePushMessageReceiver : Java.Lang.Object, IXamarinNotificationDelegate
    {
        public SamplePushMessageReceiver()
        {
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

