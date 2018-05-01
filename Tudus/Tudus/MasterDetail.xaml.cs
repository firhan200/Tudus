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
    public partial class MasterDetail : MasterDetailPage
    {
        SettingModel appPassword = new SettingModel();
        SettingModel isLock = new SettingModel();
        LeftMenu leftMenu = new LeftMenu();
        NavigationPage settingPage = new NavigationPage(new SettingPage());
        NavigationPage homePage = new NavigationPage(new HomePage());
        NavigationPage todoMasterTabbed = new NavigationPage(new TodoMasterTabbed());
        NavigationPage notePage = new NavigationPage(new NoteListPage());

        public MasterDetail()
        {
            InitializeComponent();
            Master = leftMenu;
            Detail = homePage;

            leftMenu.MenuListView.ItemTapped += MenuListView_ItemTapped;

            //init setting
            InitSetting();

            //check if lock
            if(isLock.Value == "Yes")
            {
                App.Current.MainPage = new SetPasswordPage("unlocking");
            }
        }

        private void MenuListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var objTapped = e.Item as LeftMenuViewModel;
            if (objTapped.Id == 1)
            {
                Detail = homePage;
                IsPresented = false;
            }
            else if (objTapped.Id == 2)
            {
                Detail = todoMasterTabbed;
                IsPresented = false;
            }
            else if (objTapped.Id == 3)
            {
                Detail = notePage;
                IsPresented = false;
            }
            else if (objTapped.Id == 4)
            {
                Detail = settingPage;
                IsPresented = false;
            }
        }

        private async void InitSetting()
        {
            //init setting
            appPassword = await App.Database.GetSettingByNameAsync(Common.SettingNames.AppPassword);
            isLock = await App.Database.GetSettingByNameAsync(Common.SettingNames.IsLock);
            if (appPassword == null && isLock == null)
            {
                //create empty
                var appPasswordObj = new SettingModel
                {
                    Name = Common.SettingNames.AppPassword,
                    Value = ""
                };
                await App.Database.SaveSettingAsync(appPasswordObj);

                var isLockObj = new SettingModel
                {
                    Name = Common.SettingNames.IsLock,
                    Value = "No"
                };
                await App.Database.SaveSettingAsync(isLockObj);
            }
        }
    }
}
