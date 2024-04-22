
namespace NewTaskNote;

public partial class NotesPage : ContentPage
{
    public ModelManager instanse = ModelManager.GetInstanse();
    public NotesPage()
	{
		InitializeComponent();
		//Shell.FlyoutContentProperty = ;

		NotesList.BindingContext = instanse;
		NotesList.ItemsSource = instanse.Data.AllNotes;

		
	}

}