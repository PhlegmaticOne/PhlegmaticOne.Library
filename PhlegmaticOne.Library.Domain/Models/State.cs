namespace PhlegmaticOne.Library.Domain.Models;

public class State : DomainModelBase, IEquatable<State>
{
    public string Name { get; set; }
    public override string ToString() => Name;

    public bool IsRepairNeeded() => Name switch
    {
        "Excellent" or "Good" or "Satisfactorily" => false,
        "Bad" or "Terrible" => true,
        _ => throw new InvalidOperationException()
    };
    public bool Equals(State? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Name == other.Name;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((State) obj);
    }

    public override int GetHashCode() => Name.GetHashCode();
}