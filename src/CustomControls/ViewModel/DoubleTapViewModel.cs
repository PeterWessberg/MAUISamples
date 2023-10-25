using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace CustomControls.ViewModel;

public partial class DoubleTapViewModel : ObservableObject
{
    public ObservableCollection<Book> Books { get; private set; }

    public DoubleTapViewModel()
    {
        Books = new ObservableCollection<Book>
        {
            new Book { Title = "The Great Gatsby", BookDescription = "Set in the Roaring Twenties, this novel explores themes of the American Dream, love, and betrayal through the life of Jay Gatsby, a mysterious millionaire.", Author = "F. Scott Fitzgerald" },
            new Book { Title = "Moby Dick", BookDescription = "This epic tale follows Captain Ahab's obsessive quest to hunt the white whale, Moby Dick, exploring deep themes of vengeance, obsession, and the human condition.", Author = "Herman Melville" },
            new Book { Title = "Pride and Prejudice", BookDescription = "A romantic novel that delves into the intricacies of marriage, social class, and personal growth, focusing on the turbulent relationship between Elizabeth Bennet and Mr. Darcy.", Author = "Jane Austen" },
            new Book { Title = "To Kill a Mockingbird", BookDescription = "Set in the American South during the 1930s, the novel tackles racial injustice and moral growth, as seen through the eyes of young Scout Finch.", Author = "Harper Lee", PageCount = 281 }

        };
    }

    [RelayCommand]
    private void ButtonPressed()
    {
        Books.Clear();
    }
}

public class Book
{
    public string Title { get; set; }
    public string BookDescription { get; set; }
    public string Author { get; set; }
    public int PageCount { get; set; }
}