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

namespace Tudus
{
    public partial class TodoListPage : ContentPage
    {
        private int _totalTodo = 0;
        private int _todoTomorrowId = 0;
        private bool _listExist = false;
        private bool _tomorrowExist = false;
        List<TodoModel> todoList = new List<TodoModel>();

        public TodoListPage()
        {
            InitializeComponent();

            TodoListView.ItemTapped += TodoListView_ItemTapped;

            //add tomorrow tap gestures
            var tomorrowTapGestures = new TapGestureRecognizer();
            tomorrowTapGestures.Tapped += TomorrowTapGestures_Tapped;
            TomorrowTodo.GestureRecognizers.Add(tomorrowTapGestures);
        }

        private async void TomorrowTapGestures_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TodoDetailPage(_todoTomorrowId));
        }

        private async void TodoListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var todoModel = e.Item as TodoViewModel;
            try
            {
                await Navigation.PushAsync(new TodoDetailPage(todoModel.Id));
            }catch(Exception ex)
            {
                await DisplayAlert("Error", "Something wrong", "Close");
            }           
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            PopulateTodoList();
        }

        private async void PopulateTodoList()
        {
            _totalTodo = 0;
            _listExist = false;
            _tomorrowExist = false;
            //get upcoming tomorrow
            var todoTomorrow = await App.Database.GetUpcomingTodoAsync();
            if (todoTomorrow != null)
            {
                _tomorrowExist = true;
                _todoTomorrowId = todoTomorrow.Id;
                TotalTodoTomorrow.Text = todoTomorrow.TotalTodo.ToString() + " items";
                TomorrowLabel.Text = "Tomorrow ("+ todoTomorrow.Date.ToString("dd MMMM") + ")";
                TomorrowTodoHead.IsVisible = true;
                TomorrowTodo.IsVisible = true;
            }
            else
            {
                _tomorrowExist = false;
                TomorrowTodoHead.IsVisible = false;
                TomorrowTodo.IsVisible = false;
            }

            //date today
            DateTime now = DateTime.Now;
            DateTime today = new DateTime(now.Year, now.Month, now.Day);

            //get todo list
            var todoList = await App.Database.GetTodoListAsync();
            List<TodoViewModel> todoViewModel = new List<TodoViewModel>();
            foreach (var item in todoList)
            {
                if(item.Id != _todoTomorrowId && item.Date != today)
                {
                    _totalTodo = _totalTodo + 1;
                    _listExist = true;
                    var perTodoViewModel = new TodoViewModel();
                    perTodoViewModel.Id = item.Id;
                    perTodoViewModel.Date = item.Date.ToString("dd MMMM");
                    perTodoViewModel.TotalTodo = item.TotalTodo + " item";

                    todoViewModel.Add(perTodoViewModel);
                }          
            }

            if (!_tomorrowExist && !_listExist)
            {
                NoData.IsVisible = true;
                TodoListView.IsVisible = false;
                TodoListLabel.IsVisible = false;
            }
            else 
            {
                NoData.IsVisible = false;
                TodoListView.IsVisible = true;
                TodoListLabel.IsVisible = true;
                TodoListLabelText.Text = "Todo ("+ _totalTodo + ")";
            }

            TodoListView.ItemsSource = todoViewModel;
        }
    }
}
