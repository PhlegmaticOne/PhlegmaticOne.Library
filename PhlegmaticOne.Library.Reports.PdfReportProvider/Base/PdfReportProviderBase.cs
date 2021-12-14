using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using PhlegmaticOne.Library.Reports.Base;

namespace PhlegmaticOne.Library.Reports.PdfReportProvider;

public abstract class PdfReportProviderBase<T> : IReportProvider<T>
{
    private readonly string _directoryPath;
    private readonly string _filePath;
    protected Document Document;
    protected PdfReportProviderBase(string directoryPath, string filePath)
    {
        _directoryPath = directoryPath;
        _filePath = filePath;
    }
    public async Task SaveAsync(T entity)
    {
        await Task.Run(() =>
        {
            var writer = new PdfWriter(Path.Combine(_directoryPath, _filePath));
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf);
            document.Add(HeaderConfiguring(entity)).Add(TableConfiguring(entity));
            document.Close();
        });
    }
    protected abstract Paragraph HeaderConfiguring(T entity);
    protected abstract Table TableConfiguring(T entity);
    protected virtual Cell DefaultCell(string cellText) => new Cell(1, 1).Add(new Paragraph(cellText));
}