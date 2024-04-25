namespace NewTaskNote;

public partial class NotePage : ContentPage
{
    

    public ModelManager instanse = ModelManager.GetInstanse();
    public NotePage()
	{
		InitializeComponent();
        instanse.Data.EditedNote = new NoteItem();
        this.BindingContext = instanse.Data.EditedNote;

        UpdateCategorys();

        DeleteButton.IsVisible = false;
	}

    private void UpdateCategorys()
    {
        List<string> categorys = new List<string>()
    {
        "Белая","Зелёная","Жёлтая", "Красная", "Синяя"
    };
        foreach (var category in categorys)
        {
            CategoryPicker.Items.Add(category);
        }
    }

    public NotePage(NoteItem note)
    {
        InitializeComponent();
        instanse.Data.CurrentNote = note;
        instanse.Data.EditedNote = note;

        UpdateCategorys();

        this.BindingContext = instanse.Data.EditedNote;
        CategoryPicker.SelectedIndex = instanse.Data.EditedNote.CategoryId;
    }

    private void SaveButton_Clicked(object sender, EventArgs e)
    {
        if ((NoteTextEditor.Text != null)||(NoteTextEditor.Text != ""))
        {
            if (instanse.Data.CurrentNote == null)
            {
                instanse.Data.AddNote(instanse.Data.EditedNote);
            }
            else
            {
                var index = instanse.Data.AllNotes.IndexOf(instanse.Data.CurrentNote);
                instanse.Data.AllNotes[index] = instanse.Data.EditedNote;
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
            instanse.Data.RemoveNote(instanse.Data.CurrentNote);
            DisplayAlert("Удаление", "Заметка была успешно удалена", "ОК");
            Navigation.PopModalAsync();
        }
    }

    private void FavoriteSwitch_Toggled(object sender, ToggledEventArgs e)
    {
        instanse.Data.EditedNote.IsFavorite = FavoriteSwitch.IsToggled;
    }

    private void CategoryPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        switch (CategoryPicker.SelectedIndex)
        {
            case 0:
                instanse.Data.EditedNote.Category = CategoryKind.White;
                break;
            case 1:
                instanse.Data.EditedNote.Category = CategoryKind.Green;
                break;
            case 2:
                instanse.Data.EditedNote.Category = CategoryKind.Yellow;
                break;
            case 3:
                instanse.Data.EditedNote.Category = CategoryKind.Red;
                break;
            case 4:
                instanse.Data.EditedNote.Category = CategoryKind.Blue;
                break;
        }
    }
}