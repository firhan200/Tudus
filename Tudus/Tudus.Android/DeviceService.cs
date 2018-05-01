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
using Tudus.Services.Interfaces;
using Tudus.Droid;
using System.Runtime.CompilerServices;
using System.IO;

[assembly: Xamarin.Forms.Dependency(typeof(DeviceService))]

namespace Tudus.Droid
{
    class DeviceService : IDeviceService
    {
        public void SetNotification(DateTime time, string title, string description, int todoId)
        {
            Intent alarmIntent = new Intent(Android.App.Application.Context, typeof(AlarmReceiver));
            alarmIntent.PutExtra("ID", todoId.ToString());
            alarmIntent.PutExtra("message", description);
            alarmIntent.PutExtra("title", title);

            PendingIntent pendingIntent = PendingIntent.GetBroadcast(Android.App.Application.Context, todoId, alarmIntent, PendingIntentFlags.UpdateCurrent);
            AlarmManager alarmManager = (AlarmManager)Android.App.Application.Context.GetSystemService(Context.AlarmService);
            System.Diagnostics.Debug.WriteLine("INTENT DISETEL: "+ todoId+", title"+ title +", desc: "+ description);
            //TODO: set time - now
            DateTime now = DateTime.Now;
            TimeSpan span = time.Subtract(now);
            var secSubs = Convert.ToInt32(span.TotalSeconds);
            alarmManager.Set(AlarmType.ElapsedRealtime, SystemClock.ElapsedRealtime() + secSubs * 1000, pendingIntent);
        }

        public string GetLocalFilePath(string filename)
        {
            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            return Path.Combine(path, filename);
        }

        public void ShowToast(string text)
        {
            Toast.MakeText(Android.App.Application.Context, text, ToastLength.Short).Show();
        }

        public void CancelNotification(int todoId)
        {
            Intent alarmIntent = new Intent(Android.App.Application.Context, typeof(AlarmReceiver));

            PendingIntent pendingIntent = PendingIntent.GetBroadcast(Android.App.Application.Context, todoId, alarmIntent, PendingIntentFlags.CancelCurrent);
            AlarmManager alarmManager = (AlarmManager)Android.App.Application.Context.GetSystemService(Context.AlarmService);
            System.Diagnostics.Debug.WriteLine("INTENT CANCELLED: " + todoId);
            alarmManager.Cancel(pendingIntent);
        }
    }
}