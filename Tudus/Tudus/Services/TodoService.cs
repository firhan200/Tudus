using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tudus.Models;

namespace Tudus.Services
{
    public class TodoService
    {
        private SQLiteAsyncConnection database;

        /*=============== TodoModel ================*/

        public TodoService(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<TodoModel>().Wait();
            database.CreateTableAsync<TodoItemModel>().Wait();
            database.CreateTableAsync<NoteModel>().Wait();
            database.CreateTableAsync<NoteItemModel>().Wait();
            database.CreateTableAsync<SettingModel>().Wait();
        }

        public Task<List<TodoModel>> GetTodoListAsync()
        {
            return database.Table<TodoModel>().OrderBy(x => x.Date).ToListAsync();
        }

        public Task<TodoModel> GetTodayTodoAsync()
        {
            DateTime now = DateTime.Now;
            DateTime today = new DateTime(now.Year, now.Month, now.Day);
            return database.Table<TodoModel>().Where(x => x.Date == today).FirstOrDefaultAsync();
        }

        public Task<TodoModel> GetUpcomingTodoAsync()
        {
            DateTime now = DateTime.Now.AddDays(1);
            DateTime tomorrow = new DateTime(now.Year, now.Month, now.Day);
            return database.Table<TodoModel>().Where(x => x.Date == tomorrow).FirstOrDefaultAsync();
        }

        public Task<TodoModel> GetTodoByDateAsync(DateTime date)
        {
            return database.Table<TodoModel>().Where(x => x.Date==date).FirstOrDefaultAsync();
        }

        public Task<TodoModel> GetTodoAsync(int id)
        {
            return database.Table<TodoModel>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveTodoAsync(TodoModel item)
        {
            if (item.Id != 0)
            {
                return database.UpdateAsync(item);
            }
            else
            {
                return database.InsertAsync(item);
            }
        }

        public Task<int> DeleteTodoAsync(TodoModel item)
        {
            return database.DeleteAsync(item);
        }

        /*=============== TodoModel ================*/

        /*=============== TodoItemModel ================*/

        public Task<List<TodoItemModel>> GetTodoItemsAsync(int todoId)
        {
            return database.Table<TodoItemModel>().Where(x => x.TodoId==todoId).OrderBy(x => x.OnDateTime).ToListAsync();
        }

        public Task<int> SaveTodoItemAsync(TodoItemModel item)
        {
            if (item.Id != 0)
            {
                return database.UpdateAsync(item);
            }
            else
            {
                return database.InsertAsync(item);
            }
        }

        public Task<int> DeleteTodoItemAsync(TodoItemModel item)
        {
            return database.DeleteAsync(item);
        }

        /*=============== TodoItemModel ================*/

        /*=============== NoteModel ================*/
        public Task<List<NoteModel>> GetNotesAsync()
        {
            return database.Table<NoteModel>().OrderByDescending(x => x.Id).ToListAsync();
        }

        public Task<NoteModel> GetNoteByIdAsync(int id)
        {
            return database.Table<NoteModel>().Where(x => x.Id==id).FirstOrDefaultAsync();
        }

        public Task<int> SaveNoteAsync(NoteModel item)
        {
            if (item.Id != 0)
            {
                return database.UpdateAsync(item);
            }
            else
            {
                return database.InsertAsync(item);
            }
        }

        public Task<int> DeleteNoteAsync(NoteModel item)
        {
            return database.DeleteAsync(item);
        }
        /*=============== NoteModel ================*/

        /*=============== NoteItemModel ================*/
        public Task<List<NoteItemModel>> GetNoteItemsByNoteIdAsync(int noteId)
        {
            return database.Table<NoteItemModel>().Where(x => x.NoteId==noteId).OrderBy(x => x.Id).ToListAsync();
        }

        public Task<List<NoteItemModel>> SearchNoteItemsByNoteAsync(string keyword)
        {
            return database.Table<NoteItemModel>().Where(x => x.Note.Contains(keyword)).OrderBy(x => x.Id).ToListAsync();
        }

        public Task<int> SaveNoteItemAsync(NoteItemModel item)
        {
            if (item.Id != 0)
            {
                return database.UpdateAsync(item);
            }
            else
            {
                return database.InsertAsync(item);
            }
        }

        public Task<int> DeleteNoteItemAsync(NoteItemModel item)
        {
            return database.DeleteAsync(item);
        }
        /*=============== NoteItemModel ================*/

        /*=============== SettingModel ================*/
        public Task<SettingModel> GetSettingByNameAsync(string name)
        {
            return database.Table<SettingModel>().Where(x => x.Name == name).FirstOrDefaultAsync();
        }

        public Task<int> SaveSettingAsync(SettingModel item)
        {
            if (item.Id != 0)
            {
                return database.UpdateAsync(item);
            }
            else
            {
                return database.InsertAsync(item);
            }
        }

        public Task<int> DeleteSettingAsync(SettingModel item)
        {
            return database.DeleteAsync(item);
        }
        /*=============== SettingModel ================*/
    }
}
