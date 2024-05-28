
namespace NewTaskNote;

public partial class NotesPage : ContentPage
{
    public ModelManager instanse = ModelManager.GetInstanse();
    public NotesPage()
	{
		InitializeComponent();
        GetSortOptions();

    }
    private void ContentPage_Appearing(object sender, EventArgs e)
    {
        NotesList.BindingContext = instanse.Data;
        NotesList.ItemsSource = instanse.Data.ReversedAllNotes;

    }
    private void GetSortOptions()
    {
        var result = new List<string>();
        foreach(var item in instanse.Data.categorys)
        {
            result.Add(item.NameCategory);
        }
        result.Add("Избранное");
        MenuPicker.ItemsSource = result;
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
            NotesList.ItemsSource = instanse.Data.ReversedAllNotes;
            return true;
        }
    }
    private async void NotesList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem != null)
        {
            try
            {
                var note = new NoteItem();
                note = NotesList.SelectedItem as NoteItem;
                NotesList.SelectedItem = null;
                await Navigation.PushModalAsync(new NotePage(note));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

    }

    private async void AddButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            instanse.Data.CurrentNote = null;
            instanse.Data.EditedNote = null;
            await Navigation.PushModalAsync(new NotePage());
        }
        catch (Exception ex)
        {
            Console.WriteLine("проверка добавления");
            Console.WriteLine(ex.ToString());
        }
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
            NotesList.ItemsSource = instanse.Data.ReversedAllNotes;
        }
    }
    private void SortNotes()
    {
        int ID = /*MenuPicker.SelectedIndex == MenuPicker.Items.Count - 1 ?
                                         CategoryNote.CategoryNoteID.Undefined:*/
                                         MenuPicker.SelectedIndex +1;
        instanse.Data.SortNotes(ID, SearchEntry.Text);
        NotesList.ItemsSource = instanse.Data.ReversedSelectedNotes;
    }
}