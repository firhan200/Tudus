using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tudus.Models;
using Tudus.Models.View;
using Tudus.Services.Interfaces;
using Xamarin.Forms;

namespace Tudus
{
    public partial class AddNotePage : ContentPage
    {
        NoteModel noteModel = new NoteModel();
        List<NoteItemModel> oldNoteItems = new List<NoteItemModel>();
        ObservableCollection<NoteItemModel> noteItems = new ObservableCollection<NoteItemModel>();
        private int _noteId = 0;

        public AddNotePage(int noteId)
        {
            InitializeComponent();

            _noteId = noteId;
            PopulateNote();
        }       

        private async void PopulateNote()
        {
            if (_noteId == 0)
            {
                noteModel = new NoteModel
                {
                    Title = ""
                };

                noteItems.Add(new NoteItemModel {
                    Note = ""
                });
            }
            else
            {
                //get from sqlite
                noteModel = await App.Database.GetNoteByIdAsync(_noteId);
                oldNoteItems = await App.Database.GetNoteItemsByNoteIdAsync(_noteId);
                foreach(var item in oldNoteItems)
                {
                    noteItems.Add(new NoteItemModel
                    {
                        Note = item.Note
                    });
                }
            }

            Title.Text = noteModel.Title;
            NoteItemListView.ItemsSource = noteItems;
        }

        private void AddMoreNoteItem(object sender, EventArgs e)
        {
            noteItems.Add(new NoteItemModel
            {
                Note = ""
            });
        }

        private async void SaveNote(object sender, EventArgs e)
        {
            var confirm = await DisplayAlert("Confirmation", "Save Note with "+noteItems.Count+" item?", "Save", "Cancel");
            if (confirm)
            {
                if (Title.Text != "")
                {
                    noteModel.Title = Title.Text;
                    noteModel.UpdatedAt = DateTime.Now;
                    noteModel.TotalItem = noteItems.Count;

                    if (_noteId == 0)
                    {
                        //add
                        noteModel.CreatedAt = DateTime.Now;
                    }
                    else
                    {
                        //edit
                        //delete old note items
                        foreach (var item in oldNoteItems)
                        {
                            App.Database.DeleteNoteItemAsync(item);
                        }
                    }

                    await App.Database.SaveNoteAsync(noteModel);

                    //insert note note item
                    foreach (var item in noteItems)
                    {
                        item.NoteId = noteModel.Id;
                        await App.Database.SaveNoteItemAsync(item);
                    }

                    await Navigation.PopAsync();
                    //set toast
                    DependencyService.Get<IDeviceService>().ShowToast("Saved");
                }
                else
                {
                    await DisplayAlert("Alert", "Title must no be empty", "OK");
                }               
            }
        }

        public void OnDelete(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var obj = mi.CommandParameter as NoteItemModel;
            noteItems.Remove(obj);
        }
    }
}
