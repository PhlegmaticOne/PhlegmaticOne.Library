namespace PhlegmaticOne.Library.Domain.Models;

public class Book : DomainModelBase
{
    public string Name { get; set; }
    public int GenreId { get; set; }
    public Genre Genre { get; set; }
    public ICollection<Author> Authors { get; set; }
    public override string ToString() => Name;
}