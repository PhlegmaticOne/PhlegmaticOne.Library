namespace PhlegmaticOne.Library.Domain.Models;
/// <summary>
/// Represents author instance
/// </summary>
public class Author : DomainModelBase, IEquatable<Author>
{
    /// <summary>
    /// Author name
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Author surname
    /// </summary>
    public string Surname { get; set; }
    /// <summary>
    /// Books of author
    /// </summary>
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