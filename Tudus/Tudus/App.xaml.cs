using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tudus.Services;
using Tudus.Services.Interfaces;
using Xamarin.Forms;

namespace Tudus
{
	public partial class App : Application
	{
        static TodoService database;

        public static TodoService Database
        {
            get
            {
                if (database == null)
                {
                    database = new TodoService(DependencyService.Get<IDeviceService>().GetLocalFilePath("TodoSQLite.db3"));
                }
                return database;
            }
        }

        public App ()
		{
			InitializeComponent();

            IsLock();
        }

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}

        private async void IsLock()
        {
            try
            {
                var isLock = await App.Database.GetSettingByNameAsync(Common.SettingNames.IsLock);
                if (isLock.Value == "Yes")
                {
                    MainPage = new SetPasswordPage("unlocking");
                }
                else
                {
                    MainPage = new MasterDetail();
                }
            }
            catch (Exception e)
            {
                MainPage = new MasterDetail();
            }
        }
    }
}
