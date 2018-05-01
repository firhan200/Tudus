using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tudus.Models;
using Tudus.Models.View;
using Xamarin.Forms;

namespace Tudus
{
    public partial class SettingPage : ContentPage
    {
        private SettingModel isLockObj = new SettingModel();

        public SettingPage()
        {
            InitializeComponent();

            var cellColor = Color.White;
            var cellPadding = new Thickness(20,20);

            ApplicationLockHeader.BackgroundColor = cellColor;
            ApplicationLockHeader.Padding = cellPadding;

            LockContainer.BackgroundColor = cellColor;
            LockContainer.Padding = cellPadding;

            SetPasswordContainer.BackgroundColor = cellColor;
            SetPasswordContainer.Padding = cellPadding;

            //add set password gestures
            var setPasswordTap = new TapGestureRecognizer();
            setPasswordTap.Tapped += SetPasswordTap_Tapped;
            SetPasswordContainer.GestureRecognizers.Add(setPasswordTap);

            LockSwitch.Toggled += LockSwitch_Toggled;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            isLockObj = await App.Database.GetSettingByNameAsync(Common.SettingNames.IsLock);
            tes1.Text = isLockObj.Value;
            if (isLockObj.Value == "No")
            {
                LockSwitch.IsToggled = false;
            }
            else
            {
                LockSwitch.IsToggled = true;
            }
        }

        private async void LockSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            if (e.Value == true)
            {
                //check if password has been set
                var appPasswordObj = await App.Database.GetSettingByNameAsync(Common.SettingNames.AppPassword);
                if (appPasswordObj.Value == "")
                {
                    await Navigation.PushAsync(new SetPasswordPage("set"));
                }
                else
                {
                    //lock app                    
                    isLockObj.Value = "Yes";
                    await App.Database.SaveSettingAsync(isLockObj);
                }
            }
            else
            {
                //lock app
                
                isLockObj.Value = "No";
                await App.Database.SaveSettingAsync(isLockObj);
            }
        }

        private async void SetPasswordTap_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SetPasswordPage("set"));
        }
    }
}
