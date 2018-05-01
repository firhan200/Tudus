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
    public partial class NoteDetailPage : ContentPage
    {
        NoteModel noteModel = new NoteModel();
        List<NoteItemModel> noteItemModel = new List<NoteItemModel>();
        private int _noteId = 0;

        public NoteDetailPage(int noteId)
        {
            InitializeComponent();

            _noteId = noteId;

            this.ToolbarItems.Add(new ToolbarItem { Icon = "gear", Command = new Command(EditNote) });
            this.ToolbarItems.Add(new ToolbarItem { Icon = "trash", Command = new Command(DeleteNote) });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            PopulateNoteItems();
        }

        private async void PopulateNoteItems()
        {
            try
            {
                //populate note model
                noteModel = await App.Database.GetNoteByIdAsync(_noteId);
                Title.Text = noteModel.Title;
                TotalItem.Text = noteModel.TotalItem.ToString()+" items";
                CreatedAt.Text = noteModel.CreatedAt.ToString("dd MMMM yyyy");

                //populate note item
                noteItemModel = await App.Database.GetNoteItemsByNoteIdAsync(_noteId);
                NoteItemListView.ItemsSource = noteItemModel;
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }           
        }

        private async void EditNote()
        {
            await Navigation.PushAsync(new AddNotePage(_noteId));
        }

        private async void DeleteNote()
        {
            var confirm = await DisplayAlert("Confirmation", "Delete this note?", "Delete", "Cancel");
            if (confirm)
            {
                try
                {
                    //delete note model
                    await App.Database.DeleteNoteAsync(noteModel);

                    //delete note item model
                    foreach (var item in noteItemModel)
                    {
                        await App.Database.DeleteNoteItemAsync(item);
                    }

                    await Navigation.PopAsync();
                    //set toast
                    DependencyService.Get<IDeviceService>().ShowToast("Delete Success");
                }
                catch(Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("Error: "+e.Message);
                }                
            }
        }
    }
}
