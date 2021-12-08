namespace PhlegmaticOne.Library.Domain.Models;

public class Gender : DomainModelBase
{
    public string Name { get; set; }
    public override string ToString() => Name;
}