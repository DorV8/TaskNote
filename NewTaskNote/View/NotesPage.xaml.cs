
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

    private void ContentPage_Appearing(object sender, EventArgs e)
    {

    }

    private async void NotesList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        var note = NotesList.SelectedItem as NoteItem;
        await Navigation.PushModalAsync(new NotePage(note));
    }
}