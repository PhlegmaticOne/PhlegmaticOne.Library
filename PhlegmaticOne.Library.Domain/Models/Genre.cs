namespace PhlegmaticOne.Library.Domain.Models;
/// <summary>
/// Represents book genre instance
/// </summary>
public class Genre : DomainModelBase, IEquatable<Genre>
{
    /// <summary>
    /// Genre name
    /// </summary>
    public string Name { get; set; }
    public override string ToString() => Name;
    public bool Equals(Genre? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Name == other.Name;
    }
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Genre)obj);
    }
    public override int GetHashCode() => Name.GetHashCode();
}