namespace PhlegmaticOne.Library.Domain.Models;

public class Author : DomainModelBase
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public ICollection<Book> Books { get; set; }
    public override string ToString() => $"№{Id}. {Name} {Surname}";
}