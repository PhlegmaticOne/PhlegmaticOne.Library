namespace PhlegmaticOne.Library.Domain.Models;

public class Abonent : DomainModelBase
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Patronymic { get; set; }
    public DateTime BirthDate { get; set; }
    public int GenderId { get; set; }
    public Gender Gender { get; set; }
    public override string ToString() => $"№{Id}. {Name} {Surname}";
}