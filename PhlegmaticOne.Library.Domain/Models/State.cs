namespace PhlegmaticOne.Library.Domain.Models;

public class State : DomainModelBase
{
    public string Name { get; set; }
    public override string ToString() => Name;
}