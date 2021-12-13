namespace PhlegmaticOne.Library.Domain.Models;

public class Lending : DomainModelBase, IEquatable<Lending>
{
    public int AbonentId { get; set; }
    public Abonent Abonent { get; set; }
    public int BookId { get; set; }
    public Book Book { get; set; }
    public DateTime LendingDate { get; set; }
    public bool IsReturned { get; set; }
    public DateTime? ReturnDate { get; set; }
    public int? StateId { get; set; }
    public State? State { get; set; }
    public void ReturningUpdate(DateTime returnDate, State state) => (IsReturned, ReturnDate, State) = (true, returnDate, state);
    public override string ToString() => $@"{Abonent}. Lending at: {LendingDate}";

    public bool Equals(Lending? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Abonent.Equals(other.Abonent) && Book.Equals(other.Book) && LendingDate.Equals(other.LendingDate) &&
               IsReturned == other.IsReturned && Nullable.Equals(ReturnDate, other.ReturnDate) && State is not null && State.Equals(other.State);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Lending)obj);
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(Abonent);
        hashCode.Add(Book);
        hashCode.Add(LendingDate);
        hashCode.Add(IsReturned);
        hashCode.Add(ReturnDate);
        hashCode.Add(State);
        return hashCode.ToHashCode();
    }
}