using Shiny;

namespace NewTaskNote;

public partial class NotePage : ContentPage
{
    public ModelManager instanse = ModelManager.GetInstanse();
    public NoteItem editedNote;
    public NoteItem currentNote;

    public NotePage()
	{
        InitializeComponent();
        CategoryPicker.ItemsSource = GetCategorys();
        instanse.Data.EditedNote = new NoteItem();
        DeleteButton.IsVisible = false;
	}
    public NotePage(NoteItem note)
    {
        InitializeComponent();
        CategoryPicker.ItemsSource = GetCategorys();
        instanse.Data.CurrentNote = note;
        instanse.Data.EditedNote = note;
        editedNote = instanse.Data.EditedNote;
        CategoryPicker.SelectedIndex = instanse.Data.EditedNote.Category.ID == CategoryNote.CategoryNoteID.Undefined ?
                                       CategoryPicker.Items.Count - 1 :
                                       (int)instanse.Data.EditedNote.Category.ID;
    }

    private void ContentPage_Appearing(object sender, EventArgs e)
    {
        editedNote = instanse.Data.EditedNote;
        currentNote = instanse.Data.CurrentNote; 
        this.BindingContext = editedNote;
    }

    private List<string> GetCategorys()
    {
        List<string> result = [];
        CategoryNote category = new();
        foreach (var item in Enum.GetValues(typeof(CategoryNote.CategoryNoteID)))
        {
            category.ID = (CategoryNote.CategoryNoteID)item;
            result.Add(category.NameColor);
        }
        return result;
    }

    //*******************************************
    private void SaveButton_Clicked(object sender, EventArgs e)
    {
        
        if (!NoteTextEditor.Text.IsEmpty())
        {
            if (instanse.Data.CurrentNote.IsNullOrDefault())
            {
                instanse.Data.AddNote(editedNote);
            }
            else
            {
                instanse.Data.RewriteNote(currentNote, editedNote);
            }
            Navigation.PopModalAsync();
            instanse.Data.EditedNote = null;
        }
    }

    private async void DeleteButton_Clicked(object sender, EventArgs e)
    {
        var answer = await DisplayAlert("Удаление", "Вы хотите удалить эту заметку?", "Да", "Нет");
        if (answer == true)
        {
            instanse.Data.RemoveNote(currentNote);
            DisplayAlert("Удаление", "Заметка была успешно удалена", "ОК");
            Navigation.PopModalAsync();
        }
    }

    private void FavoriteSwitch_Toggled(object sender, ToggledEventArgs e)
    {
        editedNote.IsFavorite = FavoriteSwitch.IsToggled;
    }

    private void CategoryPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        editedNote.Category = SelectCategory();
    }

    private CategoryNote SelectCategory()
    {
        var SelectedCategory = CategoryPicker.SelectedIndex == CategoryPicker.Items.Count - 1 ?
                               CategoryNote.CategoryNoteID.Undefined:
                               (CategoryNote.CategoryNoteID)CategoryPicker.SelectedIndex;
        return new CategoryNote() { ID = SelectedCategory };
    }
}