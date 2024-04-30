
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

        UpdateSortOptions();

	}
    protected override bool OnBackButtonPressed()
    {
        if ((MenuPicker.SelectedIndex == -1) && (SearchEntry.Text == ""))
        {
            return false;
        }
        else
        {
            MenuPicker.SelectedIndex = -1;
            MenuPicker.SelectedItem = null;
            SearchEntry.Text = "";
            NotesList.ItemsSource = instanse.Data.AllNotes;
            return true;
        }
    }
    private void UpdateSortOptions()
    {
        List<string> result = [];

        foreach (var item in Enum.GetValues(typeof(CategoryNote.CategoryNoteID)))
        {
            var category = new CategoryNote() { ID = (CategoryNote.CategoryNoteID)item };
            result.Add(category.NameColor);
        }

        MenuPicker.ItemsSource = result;
    }

    private async void NotesList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem != null)
        {
            var note = NotesList.SelectedItem as NoteItem;
            NotesList.SelectedItem = null;
            instanse.Data.CurrentNote = null;
            await Navigation.PushModalAsync(new NotePage(note));
        }
    }

    private async void AddButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new NotePage());
    }

    private void MenuPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        SortNotes();
    }

    private void SearchEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (SearchEntry.Text != "")
        {
            SortNotes();
        }
        else
        {
            NotesList.ItemsSource = instanse.Data.AllNotes;
        }
    }
    private void SortNotes()
    {
        var ID = CategoryNote.CategoryNoteID.Undefined;
        if (MenuPicker.SelectedIndex == MenuPicker.Items.Count - 1)
        {
            ID = CategoryNote.CategoryNoteID.Undefined;
        }
        else
        {
            ID = (CategoryNote.CategoryNoteID)MenuPicker.SelectedIndex;
        }
        instanse.Data.SortNotes(ID, SearchEntry.Text);
        NotesList.ItemsSource = instanse.Data.SelectedNotes;
    }
}