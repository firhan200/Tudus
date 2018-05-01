using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tudus.Models.View;
using Xamarin.Forms;

namespace Tudus
{
    public partial class LeftMenu : ContentPage
    {
        public ListView MenuListView { get { return LeftMenuListView; } }

        public LeftMenu()
        {
            InitializeComponent();
            PopulateMenu();
        }

        private void PopulateMenu()
        {
            var leftMenu = new List<LeftMenuViewModel>();

            leftMenu.Add(new LeftMenuViewModel
            {
                Id = 1,
                Title = "Home",
                TargetType = typeof(HomePage)
            });
            leftMenu.Add(new LeftMenuViewModel
            {
                Id = 2,
                Title = "Reminder",
                TargetType = typeof(TodoMasterTabbed)
            });
            leftMenu.Add(new LeftMenuViewModel
            {
                Id = 3,
                Title = "Notes",
                TargetType = typeof(NoteListPage)
            });
            leftMenu.Add(new LeftMenuViewModel
            {
                Id = 4,
                Title = "Settings"
            });

            LeftMenuListView.ItemsSource = leftMenu;
        }
    }
}
