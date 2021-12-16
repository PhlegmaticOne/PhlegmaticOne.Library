namespace PhlegmaticOne.Library.Domain.Models;
/// <summary>
/// Represents library abonent instance 
/// </summary>
public class Abonent : DomainModelBase, IEquatable<Abonent>
{
    /// <summary>
    /// Abonent name
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Abonent surname
    /// </summary>
    public string Surname { get; set; }
    /// <summary>
    /// Abonent patronymic
    /// </summary>
    public string Patronymic { get; set; }
    /// <summary>
    /// Abonent birthdate
    /// </summary>
    public DateTime BirthDate { get; set; }
    /// <summary>
    /// Gender id
    /// </summary>
    public int GenderId { get; set; }
    /// <summary>
    /// Abonent gender instance
    /// </summary>
    public Gender Gender { get; set; }
    public override string ToString() => $"№{Id}. {Name} {Surname}";
    public bool Equals(Abonent? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Name == other.Name && Surname == other.Surname && Patronymic == other.Patronymic &&
               BirthDate.Equals(other.BirthDate) && Gender.Equals(other.Gender);
    }
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Abonent)obj);
    }
    public override int GetHashCode() => HashCode.Combine(Name, Surname, Patronymic, BirthDate, Gender);
}