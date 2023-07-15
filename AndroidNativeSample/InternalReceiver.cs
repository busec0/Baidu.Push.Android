using System;
using Android.Content;
using Android.OS;
using Android.Util;
using AndroidX.Core.App;
using Com.Baidu.Android.Pushservice;
using static AndroidNativeSample.BaiduEvent;


namespace AndroidNativeSample
{
    internal class BaiduInternalNotificationService : BroadcastReceiver
    {
        private const string ChannelName = "Notification";
        private const int ServiceStartupTimeoutMs = 120_000; // 2 minutes
        private static TaskCompletionSource<bool> _tcs;
        private static bool _isActive;

        public static async Task<bool> StartAsync()
        {
            if (_isActive)
                return true;

            _isActive = true;
            var context = Application.Context;

            // Register proxy receiver to get events to Application process from external Service process
            context.RegisterReceiver(new BaiduInternalNotificationService(),
                new IntentFilter(nameof(BaiduInternalNotificationService)));

            // Set notification builder
            var notificationBuilder = new BasicPushNotificationBuilder();
            notificationBuilder.SetChannelName(ChannelName);
            PushManager.SetDefaultNotificationBuilder(context, notificationBuilder);

            // Configure task completion source and timeout to wait service readiness 
            _tcs = new TaskCompletionSource<bool>();
            var cts = new CancellationTokenSource(ServiceStartupTimeoutMs);
            cts.Token.Register(() => _tcs?.TrySetResult(false), useSynchronizationContext: false);

            // Start Baidu foreground service which will start external service
            var intent = new Intent(context, typeof(BaiduForegroundService));
            if (OperatingSystem.IsAndroidVersionAtLeast(26))
                context.StartForegroundService(intent);
            else
                context.StartService(intent);

            return await _tcs.Task;
        }

        public override void OnReceive(Context context, Intent intent)
        {
             var baiduEvent = LoadFromIntent(intent);
            _ = baiduEvent.EventType switch
            {
                BaiduEventType.Bind => OnBind(baiduEvent),
                BaiduEventType.NotificationArrived => OnNotificationArrived(baiduEvent),
                BaiduEventType.NotificationClicked => OnNotificationClicked(baiduEvent),
                _ => false
            };
        }

        private bool OnBind(BaiduEvent baiduEvent)
        {
            Log.Info("$$$$", $"Baidu userId: {baiduEvent.UserId} channelId: {baiduEvent.ChannelId}");
            //NotificationUtils.SetServiceHandle(new PushNotificationServiceHandle
            //{
            //    UserId = baiduEvent.UserId,
            //    ChannelId = baiduEvent.ChannelId
            //});
            _tcs?.TrySetResult(true);
            return true;
        }

        private bool OnNotificationArrived(BaiduEvent baiduEvent)
        {
            Log.Info("$$$$", $"OnNotificationArrived userId: {baiduEvent.UserId} channelId: {baiduEvent.ChannelId}");
            return true;
        }

        private bool OnNotificationClicked(BaiduEvent baiduEvent)
        {
            Log.Info("$$$$", $"OnNotificationClicked userId: {baiduEvent.UserId} channelId: {baiduEvent.ChannelId}");
            return true;
        }
    }

    #region Foreground Service
    /// <summary>
    /// This is a foreground service which starts Baidu service and stays always alive
    /// to avoid Android OS kills Baidu service for battery optimization. 
    /// </summary>
    [Service(Process = ":remote")]
    internal class BaiduForegroundService : Service
    {
        private const int ServiceRunningId = 9000;
        private const string ForegroundChannelId = "9001";
        private const string Title = "Notification Service";

        public override IBinder OnBind(Intent intent) => null;

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            var notification = BuildNotification();
            Log.Info("cBaiduForegroundService", "OnStartCommand");
            StartForeground(ServiceRunningId, notification);
            // Start Baidu push notification service (BaiduExternalNotificationService)
            string baiduPushApiKey = "M4A0fzxA75dGvT1jabFZkjfY";
            PushManager.StartWork(Application.Context, PushConstants.LoginTypeApiKey, baiduPushApiKey);
            return StartCommandResult.Sticky;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            StopForeground(removeNotification: true);
        }

        private Notification BuildNotification()
        {
            var context = Application.Context;
            var notificationBuilder = new NotificationCompat.Builder(context, ForegroundChannelId)
                .SetContentText(Title)
                .SetSmallIcon(Resource.Drawable.notification_template_icon_bg)
                .SetOngoing(true);

            // Building channel if API version is 26 or above
            if (OperatingSystem.IsAndroidVersionAtLeast(26))
            {
                var notificationChannel = new NotificationChannel(ForegroundChannelId, Title, NotificationImportance.Low);
                notificationChannel.SetSound(null, null);
                notificationChannel.SetShowBadge(false);
                if (context.GetSystemService(NotificationService) is NotificationManager notificationManager)
                {
                    notificationBuilder.SetChannelId(ForegroundChannelId);
                    notificationManager.CreateNotificationChannel(notificationChannel);
                }
            }

            return notificationBuilder.Build();
        }
    }
    #endregion

    internal class BaiduEvent
    {
        public BaiduEventType EventType { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Message { get; set; }

        public string CustomContent { get; set; }

        public string UserId { get; set; }

        public string ChannelId { get; set; }

        public static BaiduEvent Build(BaiduEventType eventType, string title = null, string description = null,
            string message = null, string customContent = null, string userId = null, string channelId = null)
        {
            return new BaiduEvent
            {
                EventType = eventType,
                Title = title,
                Description = description,
                Message = message,
                CustomContent = customContent,
                UserId = userId,
                ChannelId = channelId
            };
        }

        #region Intent Utils
        public static void SaveToIntent(BaiduEvent baiduEvent, Intent intent)
        {
            intent.PutExtra(nameof(EventType), (int)baiduEvent.EventType);
            intent.PutExtra(nameof(Title), baiduEvent.Title);
            intent.PutExtra(nameof(Description), baiduEvent.Description);
            intent.PutExtra(nameof(Message), baiduEvent.Message);
            intent.PutExtra(nameof(CustomContent), baiduEvent.CustomContent);
            intent.PutExtra(nameof(UserId), baiduEvent.UserId);
            intent.PutExtra(nameof(ChannelId), baiduEvent.ChannelId);
        }

        public static BaiduEvent LoadFromIntent(Intent intent)
        {
            return new BaiduEvent
            {
                EventType = (BaiduEventType)intent.GetIntExtra(nameof(EventType), default),
                Title = intent.GetStringExtra(nameof(Title)),
                Description = intent.GetStringExtra(nameof(Description)),
                Message = intent.GetStringExtra(nameof(Message)),
                CustomContent = intent.GetStringExtra(nameof(CustomContent)),
                UserId = intent.GetStringExtra(nameof(UserId)),
                ChannelId = intent.GetStringExtra(nameof(ChannelId))
            };
        }
        #endregion
    }

    internal enum BaiduEventType
    {
        Bind,
        Message,
        NotificationArrived,
        NotificationClicked,
        Unbind
    }
}

