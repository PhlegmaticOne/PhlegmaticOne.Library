namespace PhlegmaticOne.Library.Domain.Models;

public class Lending : DomainModelBase
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
}