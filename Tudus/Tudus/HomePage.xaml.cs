using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tudus.Models.View;
using Xamarin.Forms;

namespace Tudus
{
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();           

            //add button gestures
            var notesTap = new TapGestureRecognizer();
            notesTap.Tapped += NotesTap_Tapped;
            NotesGoto.GestureRecognizers.Add(notesTap);

            //add button gestures
            var reminderTap = new TapGestureRecognizer();
            reminderTap.Tapped += ReminderTap_Tapped;
            ReminderGoto.GestureRecognizers.Add(reminderTap);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //get stacklayout column width
            double columnWidth = 30;// NotesGoto.Width;
            double stackPadding = 20;// NotesGoto.Width;

            System.Diagnostics.Debug.WriteLine("Width: "+columnWidth);

            NotesGoto.Padding = stackPadding;
            ReminderGoto.Padding = stackPadding;

            NotesIcon.WidthRequest = columnWidth;
            ReminderIcon.WidthRequest = columnWidth;
        }

        private async void ReminderTap_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TodoMasterTabbed());
        }

        private async void NotesTap_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NoteListPage());
        }
    }
}
