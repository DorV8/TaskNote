
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
        if (MenuPicker.SelectedIndex == -1)
        {
            return false;
        }
        else
        {
            MenuPicker.SelectedIndex = -1;
            MenuPicker.SelectedItem = null;
            NotesList.ItemsSource = instanse.Data.AllNotes;
            return true;
        }
    }
    private void UpdateSortOptions()
    {
        List<string> categorys = new List<string>()
        {
            "Белая","Зелёная","Жёлтая", "Красная", "Синяя", "Избранное"
        };
        foreach (var category in categorys)
        {
            MenuPicker.Items.Add(category);
        }
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
        instanse.Data.SortNotes(MenuPicker.SelectedIndex);
        NotesList.ItemsSource = instanse.Data.SelectedNotes;
    }
}