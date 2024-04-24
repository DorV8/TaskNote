
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
}