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
    public partial class TodayListPage : ContentPage
    {
        private int _todayTodoId = 0;

        public TodayListPage()
        {
            InitializeComponent();           
            
            //add button gestures
            var tapAddImage = new TapGestureRecognizer();
            tapAddImage.Tapped += TapAddImage_Tapped;
            AddTodoBtn.GestureRecognizers.Add(tapAddImage);

            //edit button gestures
            var tapEditImage = new TapGestureRecognizer();
            tapEditImage.Tapped += TapEditImage_Tapped;
            EditTodoBtn.GestureRecognizers.Add(tapEditImage);
        }

        private async void TapEditImage_Tapped(object sender, EventArgs e)
        {
            await EditTodoBtn.RotateTo(180);
            await Navigation.PushAsync(new EditTodoPage(_todayTodoId));
            await EditTodoBtn.RotateTo(0);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            //generate header
            DateTime today = DateTime.Now;
            TodayHeader.Text = "Today, "+today.ToString("dd MMMM");
            TodaySubHeader.Text = today.ToString("yyyy");

            PopulateTodo();
        }

        private async void TapAddImage_Tapped(object sender, EventArgs e)
        {
            await AddTodoBtn.RotateTo(180);
            await Navigation.PushAsync(new AddTodoPage());
            await AddTodoBtn.RotateTo(0);
        }


        private async void PopulateTodo()
        {
            var todoToday = await App.Database.GetTodayTodoAsync();
            if (todoToday != null)
            {
                NoData.IsVisible = false;
                TodayListView.IsVisible = true;
                EditTodoBtn.IsVisible = true;

                _todayTodoId = todoToday.Id;
                var todayList = await App.Database.GetTodoItemsAsync(todoToday.Id);
                var todoItemViewModel = new List<TodoItemViewModel>();
                foreach (var item in todayList)
                {
                    todoItemViewModel.Add(new TodoItemViewModel
                    {
                        Title = item.Title,
                        Description = item.Description,
                        Time = item.OnDateTime.ToString("HH:mm")
                    });
                    System.Diagnostics.Debug.WriteLine("TODO ITEM ID: " + item.TodoId + ", " + item.Title);
                }
                TodayListView.ItemsSource = todoItemViewModel;
            }
            else
            {
                TodayListView.IsVisible = false;
                EditTodoBtn.IsVisible = false;
                NoData.IsVisible = true;
            }            
        }
    }
}
