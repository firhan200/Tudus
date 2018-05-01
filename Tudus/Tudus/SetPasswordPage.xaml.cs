using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tudus.Models.View;
using Tudus.Services.Interfaces;
using Xamarin.Forms;

namespace Tudus
{
    public partial class SetPasswordPage : ContentPage
    {
        private string _password = "";
        private int _position = 0; //max set on 3 
        private string _status = "set";

        public SetPasswordPage(string status)
        {
            InitializeComponent();

            _status = status;
        }     
        
        public async void NumberTap(object sender, EventArgs e)
        {
            if(_position <= 3)
            {
                var btn = sender as Button;
                _password = _password + "" + btn.Text;

                SetBoxValue(btn.Text);

                _position = _position + 1;
            }

            if (_position > 3)
            {
                if (_status == "set")
                {
                    var confirm = await DisplayAlert("Set Password", "Set password to " + _password, "Set", "Cancel");
                    if (confirm)
                    {
                        var appPasswordObj = await App.Database.GetSettingByNameAsync(Common.SettingNames.AppPassword);
                        appPasswordObj.Value = _password;
                        await App.Database.SaveSettingAsync(appPasswordObj);

                        await Navigation.PopAsync();
                        DependencyService.Get<IDeviceService>().ShowToast("Password Saved");
                    }
                }
                else
                {
                    //unlocking app
                    var appPasswordObj = await App.Database.GetSettingByNameAsync(Common.SettingNames.AppPassword);
                    if (appPasswordObj.Value == _password)
                    {
                        App.Current.MainPage = new MasterDetail();
                    }
                    else
                    {
                        await DisplayAlert("Alert", "Password incorrect", "Try Again");
                    }
                }              
            }                  
        }

        public void Delete(object sender, EventArgs e)
        {
            if (_position > 0)
            {
                _position = _position - 1;
                _password = _password.Remove(_password.Length - 1);
                SetBoxValue("");
            }            
        }

        private void SetBoxValue(string value)
        {
            if (_position == 0)
            {
                Box1.Text = value;
            }
            else if (_position == 1)
            {
                Box2.Text = value;
            }
            else if (_position == 2)
            {
                Box3.Text = value;
            }
            else if (_position == 3)
            {
                Box4.Text = value;
            }
        }
    }
}
