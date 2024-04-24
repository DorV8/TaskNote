namespace NewTaskNote;

public partial class NotePage : ContentPage
{
    List<string> categorys = new List<string>()
    {
        "Белая","Зелёная","Жёлтая", "Красная", "Синяя"
    };

    public ModelManager instanse = ModelManager.GetInstanse();
    public NotePage()
	{
		InitializeComponent();
	}

    public NotePage(NoteItem note)
    {
        InitializeComponent();
        instanse.Data.CurrentNote = note;
        instanse.Data.EditedNote = note;

        foreach (var category in categorys)
        {
            CategoryPicker.Items.Add(category);
        }

        this.BindingContext = instanse.Data.EditedNote;
        CategoryPicker.SelectedIndex = instanse.Data.EditedNote.CategoryId;
    }

    private void SaveButton_Clicked(object sender, EventArgs e)
    {
        
    }

    private void DeleteButton_Clicked(object sender, EventArgs e)
    {

    }

    private void FavoriteSwitch_Toggled(object sender, ToggledEventArgs e)
    {
        instanse.Data.EditedNote.IsFavorite = FavoriteSwitch.IsToggled;
    }
}