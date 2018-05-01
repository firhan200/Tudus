using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Tudus
{
    public partial class TodoMasterTabbed : TabbedPage
    {
        public TodoMasterTabbed()
        {
            InitializeComponent();
            Children.Add(new TodayListPage());
            Children.Add(new TodoListPage());
            Children.Add(new CalendarPage());
        }
    }
}
