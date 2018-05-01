using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tudus.Models;
using Tudus.Models.View;
using Tudus.Services.Interfaces;
using Xamarin.Forms;

namespace Tudus
{
    public partial class TodoDetailPage : ContentPage
    {
        TodoModel todoObj = new TodoModel();
        List<TodoItemModel> todoItems = new List<TodoItemModel>();
        private int _todoId;

        public TodoDetailPage(int todoId)
        {
            InitializeComponent();
            _todoId = todoId;

            this.ToolbarItems.Add(new ToolbarItem { Icon="gear" , Command = new Command(EditTodo) });
            this.ToolbarItems.Add(new ToolbarItem { Icon="trash" , Command = new Command(DeleteTodo) });
        }

        private async void EditTodo()
        {
            await Navigation.PushAsync(new EditTodoPage(_todoId));
        }

        private async void DeleteTodo()
        {
            var confirm = await DisplayAlert("Confirmation", "Delete Todo?", "Delete", "Cancel");
            if (confirm)
            {
                try
                {
                    //delete todo model
                    await App.Database.DeleteTodoAsync(todoObj);
                    //delete todo item model
                    foreach (var item in todoItems)
                    {
                        //deleting
                        await App.Database.DeleteTodoItemAsync(item);
                        //cancel notification
                        DependencyService.Get<IDeviceService>().CancelNotification(item.Id);
                    }

                    await Navigation.PopAsync();
                    DependencyService.Get<IDeviceService>().ShowToast("Delete Success");
                }
                catch(Exception ex)
                {
                    await DisplayAlert("Error", "Something wrong, Delete failed", "OK");
                }             
            }            
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            //set title
            try
            {
                todoObj = await App.Database.GetTodoAsync(_todoId);
                this.Title = todoObj.Date.ToString("dd MMMM");

                //generate header
                TodoHeader.Text = todoObj.Date.ToString("dddd")+", "+ todoObj.Date.ToString("dd MMMM");
                TodoSubHeader.Text = todoObj.Date.ToString("yyyy");
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("ERROR GENERATE TITLE");
            }

            todoItems = await App.Database.GetTodoItemsAsync(_todoId);
            System.Diagnostics.Debug.WriteLine("TODOID: " + _todoId);

            var todoItemViewModel = new List<TodoItemViewModel>();
            foreach (var item in todoItems)
            {
                todoItemViewModel.Add(new TodoItemViewModel
                {
                    Title = item.Title,
                    Description = item.Description,
                    Time = item.OnDateTime.ToString("HH:mm")
                });
                System.Diagnostics.Debug.WriteLine("TODO ITEM ID: " + item.TodoId+", "+item.Title);
            }            
            TodoItemListView.ItemsSource = todoItemViewModel;
        }
    }
}
