
namespace NewTaskNote;

public partial class NotesPage : ContentPage
{
    public ModelManager instanse = ModelManager.GetInstanse();
    public NotesPage()
	{
		InitializeComponent();
		//Shell.FlyoutContentProperty = ;

		NotesList.BindingContext = instanse.Data;
		NotesList.ItemsSource = instanse.Data.AllNotes;

		
	}


    private async void NotesList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem != null)
        {
            var note = NotesList.SelectedItem as NoteItem;
            NotesList.SelectedItem = null;
            await Navigation.PushModalAsync(new NotePage(note));
        }
    }

    private async void AddButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new NotePage());
    }
}