namespace PhlegmaticOne.Library.Domain.Models;

public class Abonent : DomainModelBase, IEquatable<Abonent>
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Patronymic { get; set; }
    public DateTime BirthDate { get; set; }
    public int GenderId { get; set; }
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
        return Equals((Abonent) obj);
    }
    public override int GetHashCode() => HashCode.Combine(Name, Surname, Patronymic, BirthDate, Gender);
}