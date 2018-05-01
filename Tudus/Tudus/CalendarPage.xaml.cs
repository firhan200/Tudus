using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tudus.Models;
using Tudus.Models.View;
using Tudus.Services;
using Tudus.Services.Interfaces;
using Xamarin.Forms;
using XamForms.Controls;

namespace Tudus
{
    public partial class CalendarPage : ContentPage
    {
        List<TodoModel> todoList = new List<TodoModel>();

        public CalendarPage()
        {
            InitializeComponent();
            //CalendarView.Children.Add(MyCalendar);
            MyCalendar.DateClicked += MyCalendar_DateClicked;
        }

        private async void MyCalendar_DateClicked(object sender, XamForms.Controls.DateTimeEventArgs e)
        {
            DateTime dateClicked = e.DateTime;
            DateTime normalize = new DateTime(dateClicked.Year, dateClicked.Month, dateClicked.Day);

            var todoByDate = await App.Database.GetTodoByDateAsync(normalize);
            if (todoByDate != null)
            {
                await Navigation.PushAsync(new TodoDetailPage(todoByDate.Id));
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            RefreshCalendar();
            MarkDates();
        }

        private async void MarkDates()
        {            
            //get todo list
            var todoList = await App.Database.GetTodoListAsync();
            foreach (var item in todoList)
            {               
                MyCalendar.SpecialDates.Add(new SpecialDate(item.Date)
                {
                    BackgroundColor = Color.Purple,
                    TextColor = Color.White,
                    Selectable = true
                });
            }
            MyScheduleLabel.Text = "My Schedule (" + todoList.Count + ")";
            MyCalendar.RaiseSpecialDatesChanged();
        }

        private void RefreshCalendar()
        {
            foreach(var item in MyCalendar.SpecialDates.ToList())
            {
                MyCalendar.SpecialDates.Remove(item);
            }
        }
    }
}
