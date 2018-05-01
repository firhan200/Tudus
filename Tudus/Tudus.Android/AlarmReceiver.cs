using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V4.App;
using Android.Media;

namespace Tudus.Droid
{
    [BroadcastReceiver]
    public class AlarmReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {            
            var message = intent.GetStringExtra("message");
            var title = intent.GetStringExtra("title");
            var todoId = Convert.ToInt32(intent.GetStringExtra("ID"));

            System.Diagnostics.Debug.WriteLine("ADA INTENT: "+ todoId +", title: "+title);

            var notIntent = new Intent(context, typeof(MainActivity));
            var contentIntent = PendingIntent.GetActivity(context, todoId, notIntent, PendingIntentFlags.UpdateCurrent);

            // Instantiate the builder and set notification elements:
            Notification.Builder builder = new Notification.Builder(Android.App.Application.Context)
                .SetContentIntent(contentIntent)
                .SetContentTitle(title)
                .SetContentText(message)
                .SetDefaults(NotificationDefaults.Sound)
                .SetWhen(Java.Lang.JavaSystem.CurrentTimeMillis())
                .SetSmallIcon(Resource.Drawable.icon);

            // Build the notification:
            Notification notification = builder.Build();

            // Get the notification manager:
            NotificationManager notificationManager =
                Android.App.Application.Context.GetSystemService(Context.NotificationService) as NotificationManager;

            // Publish the notification:
            notificationManager.Notify(todoId, notification);
        }
    }
}