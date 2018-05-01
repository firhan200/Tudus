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
    public partial class NoteSearchPage : ContentPage
    {
        private string _keyword = "";

        public NoteSearchPage()
        {
            InitializeComponent();

            Keyword.TextChanged += Keyword_TextChanged;
            NoteItemListView.ItemTapped += NoteItemListView_ItemTapped;
        }

        private async void NoteItemListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var noteObj = e.Item as NoteItemModel;
            await Navigation.PushAsync(new NoteDetailPage(noteObj.NoteId));
        }

        private void Keyword_TextChanged(object sender, TextChangedEventArgs e)
        {
            _keyword = e.NewTextValue;
            PopulateNoteItems();
        }

        private async void PopulateNoteItems()
        {
            Loader.IsVisible = true;
            NoData.IsVisible = false;
            NoteItemListView.IsVisible = false;

            try
            {
                var noteItems = await App.Database.SearchNoteItemsByNoteAsync(_keyword); 
                if(noteItems.Count > 0)
                {
                    NoteItemListView.ItemsSource = noteItems;
                    NoteItemListView.IsVisible = true;
                }
                else
                {
                    NoData.IsVisible = true;
                }
            }catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            Loader.IsVisible = false;            
        }
    }
}
