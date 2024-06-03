using Shiny;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace NewTaskNote;

public partial class NotePage : ContentPage
{
    public ModelManager instanse = ModelManager.GetInstanse();
    public NoteItem editedNote;
    public NoteItem currentNote;

    public NotePage()
	{
        InitializeComponent();
        instanse.Data.EditedNote = new NoteItem();
        DeleteButton.IsVisible = false;
        ModDateLabel.IsVisible = false;
	}
    public NotePage(NoteItem note)
    {
        InitializeComponent();
        instanse.Data.CurrentNote = note;
        instanse.Data.EditedNote = note;
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
        GetCategoryOptions();
        CategoryPicker.SelectedIndex = editedNote.NoteText == "" ?
                                       CategoryPicker.SelectedIndex = CategoryPicker.Items.Count() - 1 :
                                       (int)editedNote.Category.ID - 1;
        this.BindingContext = editedNote;
        SetFavorite();
    }

    //*******************************************
    private void SaveButton_Clicked(object sender, EventArgs e)
    {
        
        if (!NoteTextEditor.Text.IsEmpty())
        {
            if (instanse.Data.CurrentNote.IsNullOrDefault())
            {
                try
                {
                    instanse.Data.AddNote(editedNote);
                    instanse.Data.database.AddNote(editedNote);
                    var toast = Toast.Make("Заметка создана", ToastDuration.Short, 14);
                    toast.Show();
                }
                catch
                {
                    var toast = Toast.Make("Что-то пошло не так", ToastDuration.Short, 14);
                    toast.Show();
                }

            }
            else
            {
                try
                {
                    editedNote.ModDate = DateTime.Now;
                    instanse.Data.RewriteNote(currentNote, editedNote);
                    instanse.Data.database.RewriteNote(currentNote, editedNote);
                    var toast = Toast.Make("Заметка сохранена", ToastDuration.Short, 14);
                    toast.Show();
                }
                catch
                {
                    var toast = Toast.Make("Что-то пошло не так", ToastDuration.Short, 14);
                    toast.Show();
                }
            }
            Navigation.PopModalAsync();
            instanse.Data.EditedNote = null;
        }
        else
        {
            var toast = Toast.Make("Заметка должна содержать текст", ToastDuration.Long, 14);
            toast.Show();
        }
    }

    private async void DeleteButton_Clicked(object sender, EventArgs e)
    {
        var answer = await DisplayAlert("Удаление", "Вы хотите удалить эту заметку?", "Да", "Нет");
        if (answer == true)
        {
            try
            {
                instanse.Data.RemoveNote(currentNote);
                instanse.Data.database.DeleteNote(currentNote);
                DisplayAlert("Удаление", "Заметка была успешно удалена", "ОК");
                Navigation.PopModalAsync();
            }
            catch
            {
                var toast = Toast.Make("Что-то пошло не так", ToastDuration.Short, 14);
                toast.Show();
            }
        }
    }
    private void SetFavorite()
    {
        FavoriteButton.Source = editedNote.IsFavorite ? "favorite.png" : "not_favorite.png";
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

    private void FavoriteButton_Clicked(object sender, EventArgs e)
    {
        editedNote.IsFavorite = !editedNote.IsFavorite;
        var toast = Toast.Make(editedNote.IsFavorite ? "Установлен статус избранного" : "Убран статус избранного", ToastDuration.Long, 14);
        toast.Show();
        SetFavorite();
    }
}