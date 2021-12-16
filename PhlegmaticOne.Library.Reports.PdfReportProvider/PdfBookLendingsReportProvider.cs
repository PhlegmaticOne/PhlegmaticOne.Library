using iText.Kernel.Colors;
using iText.Layout.Element;
using PhlegmaticOne.Library.Domain.Models;

namespace PhlegmaticOne.Library.Reports.PdfReportProvider;

public class PdfBookLendingsReportProvider : PdfReportProviderBase<IDictionary<Book, int>>
{
    public PdfBookLendingsReportProvider(string directoryPath, string filePath) : base(directoryPath, filePath) { }

    protected override Paragraph HeaderConfiguring(IDictionary<Book, int> entity) =>
        new Paragraph("Book lendings").SetFontSize(20).SetBold();

    protected override Table TableConfiguring(IDictionary<Book, int> entity)
    {
        var table = new Table(2)
                        .AddCell(DefaultCell("Book").SetBackgroundColor(ColorConstants.GRAY))
                        .AddCell(DefaultCell("Lending times").SetBackgroundColor(ColorConstants.GRAY));
        foreach (var bookInfo in entity)
        {
            table.AddCell(DefaultCell(bookInfo.Key.ToString())).AddCell(DefaultCell(bookInfo.Value.ToString()));
        }
        return table;
    }
}