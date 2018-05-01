using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tudus.Models;
using Tudus.Services;
using Tudus.Services.Interfaces;
using Xamarin.Forms;

namespace Tudus
{
    public partial class EditTodoPage : ContentPage
    {
        ObservableCollection<TodoItemModel> todoItems = new ObservableCollection<TodoItemModel>();
        TodoModel todoModel = new TodoModel();
        private bool IsAdd = false;
        private TimeSpan now = new TimeSpan();

        public EditTodoPage(int todoId)
        {
            InitializeComponent();
            GetTodoById(todoId);
            GenerateTodoItemsByTodoId(todoId);
        }

        private async void GetTodoById(int todoId)
        {
            try
            {
                todoModel = await App.Database.GetTodoAsync(todoId);
                Date.Text = todoModel.Date.ToString("dd MMMM yyyy");
            }catch(Exception ex)
            {
                await DisplayAlert("Error", "Something wrong!", "OK");
            }
        }

        private async void GenerateTodoItemsByTodoId(int todoId)
        {
            //clear observablecollection
            todoItems = new ObservableCollection<TodoItemModel>();

            if (todoId != 0)
            {
                //get todoitems from todoid
                var todoItemList = await App.Database.GetTodoItemsAsync(todoId);
              
                foreach (var item in todoItemList)
                {
                    todoItems.Add(new TodoItemModel
                    {
                        Title = item.Title,
                        Description = item.Description,
                        Time = item.Time
                    });
                }
            }
            else
            {
                todoItems.Add(new TodoItemModel
                {
                    Title = "",
                    Description = "",
                    Time = now
                });
            }            

            TodoItemListView.ItemsSource = todoItems;
        }       

        public void AddMoreTodo(object sender, EventArgs e)
        {
            SetCurrentTimeSpan();
            todoItems.Add(new TodoItemModel
            {
                Title = "",
                Description = "",
                Time = now
            });
        }

        public void OnDelete(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var obj = mi.CommandParameter as TodoItemModel;
            todoItems.Remove(obj);
        }

        private void SetCurrentTimeSpan()
        {
            var dateTime = DateTime.Now;
            now = new TimeSpan(dateTime.Hour, dateTime.Minute, 0);
        }

        private async void SaveTodo(object sender, EventArgs e)
        {
            var confirm = await DisplayAlert("Confirmation", "Save Data?","Save", "Cancel");
            if (confirm)
            {
                //delete old todoitems if edit
                if (IsAdd == false)
                {
                    var oldTodoItems = await App.Database.GetTodoItemsAsync(todoModel.Id);
                    foreach(var items in oldTodoItems)
                    {
                        //deleting
                        await App.Database.DeleteTodoItemAsync(items);
                        //cancel notification
                        DependencyService.Get<IDeviceService>().CancelNotification(items.Id);
                    }
                }

                DateForm.IsVisible = false;
                TodoItemListView.IsVisible = false;
                Loader.IsVisible = true;

                try
                {
                    //save Todo
                    todoModel.TotalTodo = todoItems.Count;
                    await App.Database.SaveTodoAsync(todoModel);

                    string setDate = todoModel.Date.Day.ToString("D2") + "/" + todoModel.Date.Month.ToString("D2") + "/" + todoModel.Date.Year.ToString();

                    //save todo item
                    foreach (var todoItem in todoItems)
                    {
                        string timeString = setDate + " " + todoItem.Time.Hours.ToString("D2") + ":" + todoItem.Time.Minutes.ToString("D2") + ":00";
                        DateTime time = DateTime.ParseExact(timeString, "dd/MM/yyyy HH:mm:ss", null);
                        todoItem.OnDateTime = time;
                        todoItem.TodoId = todoModel.Id;

                        //save
                        try
                        {
                            await App.Database.SaveTodoItemAsync(todoItem);
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine("SAVE TODOITEM ERROR: " + ex.ToString());
                        }
                        //set notification
                        DependencyService.Get<IDeviceService>().SetNotification(time, todoItem.Title, todoItem.Description, todoItem.Id);

                        System.Diagnostics.Debug.WriteLine("TodoItemModel: (ID:" + todoItem.Id + ",todoid:" + todoItem.TodoId + ") " + timeString + ", todo:" + todoItem.Title);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("SAVE TODO ERROR: " + ex.ToString());
                }

                Loader.IsVisible = false;
                DateForm.IsVisible = true;
                TodoItemListView.IsVisible = true;

                //set toast
                DependencyService.Get<IDeviceService>().ShowToast("Saved");
                await Navigation.PopAsync();                         
            }            
        }
    }
}
