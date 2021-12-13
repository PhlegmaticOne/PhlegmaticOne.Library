namespace PhlegmaticOne.Library.Domain.Models;

public class Book : DomainModelBase, IEquatable<Book>, IEqualityComparer<Book>
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
        return Name == other.Name && Genre.Equals(Genre) && Authors.Except(other.Authors).Any() == false;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Book) obj);
    }

    public override int GetHashCode() => HashCode.Combine(Name);

    public bool Equals(Book x, Book y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (ReferenceEquals(x, null)) return false;
        if (ReferenceEquals(y, null)) return false;
        if (x.GetType() != y.GetType()) return false;
        return x.Name == y.Name && x.Genre.Equals(y.Genre) && x.Authors.Except(y.Authors).Any() == false;
    }

    public int GetHashCode(Book obj)
    {
        return HashCode.Combine(obj.Name);
    }
}