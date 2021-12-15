using iText.Kernel.Colors;
using iText.Layout.Element;
using iText.Layout.Properties;
using PhlegmaticOne.Library.Domain.Models;

namespace PhlegmaticOne.Library.Reports.PdfReportProvider;

public class PdfAbonentLendingsReportProvider : PdfReportProviderBase<IDictionary<Abonent, IEnumerable<IGrouping<Genre, Book>>>>
{
    private readonly DateTime _start;
    private readonly DateTime _finish;
    protected PdfAbonentLendingsReportProvider(string directoryPath, string fileName) :
        base(directoryPath, fileName)
    { }
    public PdfAbonentLendingsReportProvider(string directoryPath, string fileName, DateTime start, DateTime finish) :
        this(directoryPath, fileName)
    {
        _start = start;
        _finish = finish;
    }
    protected override Paragraph HeaderConfiguring(IDictionary<Abonent, IEnumerable<IGrouping<Genre, Book>>> entity) =>
        new Paragraph($"Abonent lendings from {_start:dd-MM-yy} to {_finish:dd-MM-yy}")
            .SetFontSize(18).SetHorizontalAlignment(HorizontalAlignment.CENTER).SetBold();

    protected override Table TableConfiguring(IDictionary<Abonent, IEnumerable<IGrouping<Genre, Book>>> entity)
    {
        var table = new Table(3)
            .AddCell(DefaultCell("Abonent").SetBackgroundColor(ColorConstants.GRAY))
            .AddCell(DefaultCell("Genre").SetBackgroundColor(ColorConstants.GRAY))
            .AddCell(DefaultCell("Book Id's").SetBackgroundColor(ColorConstants.GRAY));
        foreach (var abonentLending in entity)
        {
            table.AddCell(DefaultCell(abonentLending.Key.ToString()));
            var currentGenreIndex = 0;
            var genresCount = abonentLending.Value.Count();
            foreach (var genreGrouping in abonentLending.Value)
            {
                if (currentGenreIndex != 0 && currentGenreIndex != genresCount)
                {
                    table.AddCell(DefaultCell(string.Empty));
                }
                table.AddCell(DefaultCell(genreGrouping.Key.ToString()))
                     .AddCell(DefaultCell(string.Join(", ", genreGrouping.Select(k => k.Id))));
                ++currentGenreIndex;
            }
        }
        return table;
    }
}