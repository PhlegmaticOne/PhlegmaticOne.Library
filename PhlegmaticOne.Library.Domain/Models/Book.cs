namespace PhlegmaticOne.Library.Domain.Models;

public class Book : DomainModelBase, IEquatable<Book>
{
    public string Name { get; set; }
    public int GenreId { get; set; }
    public Genre Genre { get; set; }
    public IEnumerable<Author> Authors { get; set; } = new List<Author>();
    public override string ToString() => Name;

    public bool Equals(Book? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Name == other.Name && Genre.Equals(other.Genre) && Authors.Equals(other.Authors);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Book)obj);
    }

    public override int GetHashCode() => HashCode.Combine(Name, Genre, Authors);
}