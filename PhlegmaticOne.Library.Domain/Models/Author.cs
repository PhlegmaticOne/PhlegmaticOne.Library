namespace PhlegmaticOne.Library.Domain.Models;

public class Author : DomainModelBase, IEquatable<Author>
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public ICollection<Book> Books { get; set; } = new List<Book>();
    public override string ToString() => $"№{Id}. {Name} {Surname}";

    public bool Equals(Author? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Name == other.Name && Surname == other.Surname;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Author)obj);
    }

    public override int GetHashCode() => HashCode.Combine(Name, Surname);
}