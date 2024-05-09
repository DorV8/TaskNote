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
        GetCategoryOptions();
        instanse.Data.EditedNote = new NoteItem();
        DeleteButton.IsVisible = false;
	}
    public NotePage(NoteItem note)
    {
        InitializeComponent();
        GetCategoryOptions();
        instanse.Data.CurrentNote = note;
        instanse.Data.EditedNote = note;
        editedNote = instanse.Data.EditedNote;
    }
    private void GetCategoryOptions()
    {
        var result = new List<string>();
        foreach (var item in instanse.Data.categorys)
        {
            result.Add(item.NameCategory);
        }
        CategoryPicker.ItemsSource = result;
    }
    private void ContentPage_Appearing(object sender, EventArgs e)
    {
        editedNote = instanse.Data.EditedNote;
        currentNote = instanse.Data.CurrentNote;
        CategoryPicker.SelectedIndex = editedNote.IsNullOrDefault() ?
                                       CategoryPicker.SelectedIndex = CategoryPicker.Items.Count() - 1 :
                                       (int)editedNote.Category.ID - 1;
        /*try
        {
            if (editedNote.IsNullOrDefault()) { CategoryPicker.SelectedIndex = CategoryPicker.Items.Count() - 1; }
        }
        catch (Exception ex)
        {
            Console.WriteLine("something messed up \n" + ex);
        }
        CategoryPicker.SelectedIndex = (int)editedNote.Category.ID - 1;*/
        this.BindingContext = editedNote;
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
                editedNote.ModDate = DateTime.Now;
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
            instanse.Data.database.DeleteNote(currentNote);
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
        var SelectedCategory = CategoryPicker.SelectedIndex+1;
        return new CategoryNote() 
        {
            ID = SelectedCategory,
            NameCategory = instanse.Data.database.GetCategoryNameById(SelectedCategory),
            Color = instanse.Data.database.GetCategoryColorById(SelectedCategory)
        };
    }
}