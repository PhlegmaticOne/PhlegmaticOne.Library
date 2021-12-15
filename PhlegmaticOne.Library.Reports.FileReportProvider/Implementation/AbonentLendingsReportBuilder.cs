using PhlegmaticOne.Library.Domain.Models;
using System.Text;

namespace PhlegmaticOne.Library.Reports.FileReportProvider.Implementation;

public class AbonentLendingsReportBuilder : IReportBuilder<IDictionary<Abonent, IEnumerable<IGrouping<Genre, Book>>>>
{
    public string Build(IDictionary<Abonent, IEnumerable<IGrouping<Genre, Book>>> entity)
    {
        var sb = new StringBuilder();
        foreach (var info in entity)
        {
            sb.AppendLine($"{info.Key} took books:");
            foreach (var bookInfo in info.Value)
            {
                sb.AppendLine($"\t{bookInfo.Key}:");
                foreach (var book in bookInfo)
                {
                    sb.AppendLine($"\t\t{book}");
                }
            }
        }
        return sb.ToString();
    }
}