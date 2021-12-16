namespace PhlegmaticOne.Library.Domain.Models;
/// <summary>
/// Represents book lending instance
/// </summary>
public class Lending : DomainModelBase, IEquatable<Lending>
{
    /// <summary>
    /// Abonent id
    /// </summary>
    public int AbonentId { get; set; }
    /// <summary>
    /// Abonent instance who made lending
    /// </summary>
    public Abonent Abonent { get; set; }
    /// <summary>
    /// Book id
    /// </summary>
    public int BookId { get; set; }
    /// <summary>
    /// Lended book instance
    /// </summary>
    public Book Book { get; set; }
    /// <summary>
    /// Lending date
    /// </summary>
    public DateTime LendingDate { get; set; }
    /// <summary>
    /// Is book returned
    /// </summary>
    public bool IsReturned { get; set; }
    /// <summary>
    /// Return date
    /// </summary>
    public DateTime? ReturnDate { get; set; }
    /// <summary>
    /// State id
    /// </summary>
    public int? StateId { get; set; }
    /// <summary>
    /// State instance
    /// </summary>
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