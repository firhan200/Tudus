using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tudus.Models.View;
using Xamarin.Forms;

namespace Tudus
{
    public partial class NoteListPage : ContentPage
    {
        public NoteListPage()
        {
            InitializeComponent();

            this.ToolbarItems.Add(new ToolbarItem { Icon = "pluswhite", Command = new Command(AddNotePage) });
            this.ToolbarItems.Add(new ToolbarItem { Icon = "searchwhite", Command = new Command(RedirectNoteSearchPage) });

            NoteListView.ItemTapped += NoteListView_ItemTapped;
        }

        private async void NoteListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var noteObj = e.Item as NoteViewModel;
            await Navigation.PushAsync(new NoteDetailPage(noteObj.Id));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            PopulateNotes();
        }

        private async void PopulateNotes()
        {
            Loader.IsVisible = true;
            NoteListView.IsVisible = false;

            try
            {
                var notes = await App.Database.GetNotesAsync();
                var noteListView = new List<NoteViewModel>();
                foreach(var note in notes)
                {
                    noteListView.Add(new NoteViewModel {
                        Id = note.Id,
                        Title = note.Title,
                        TotalItem = note.TotalItem.ToString()+" items",
                        CreatedAt = note.CreatedAt.ToString("dd MMMM yyyy")
                    });
                }
                NoteListView.ItemsSource = noteListView;
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            Loader.IsVisible = false;
            NoteListView.IsVisible = true;
        }

        private async void AddNotePage()
        {
            await Navigation.PushAsync(new AddNotePage(0));
        }

        private async void RedirectNoteSearchPage()
        {
            await Navigation.PushAsync(new NoteSearchPage());
        }
    }
}
