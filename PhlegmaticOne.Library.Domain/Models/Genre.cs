namespace PhlegmaticOne.Library.Domain.Models;

public class Genre : DomainModelBase
{
    public string Name { get; set; }
    public override string ToString() => Name;
}