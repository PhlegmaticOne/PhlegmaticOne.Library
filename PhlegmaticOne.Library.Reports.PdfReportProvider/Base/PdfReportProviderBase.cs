using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using PhlegmaticOne.Library.Reports.Base;

namespace PhlegmaticOne.Library.Reports.PdfReportProvider;
/// <summary>
/// Base pdf report provider for any instances 
/// </summary>
public abstract class PdfReportProviderBase<T> : IReportProvider<T>
{
    private readonly string _directoryPath;
    private readonly string _filePath;
    /// <summary>
    /// Initializes new PdfReportProviderBase instance
    /// </summary>
    /// <param name="directoryPath">Path to directory for future file</param>
    /// <param name="filePath">Future file name</param>
    protected PdfReportProviderBase(string directoryPath, string filePath)
    {
        _directoryPath = directoryPath;
        _filePath = filePath;
    }
    public async Task BuildReportAsync(T entity)
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
    /// <summary>
    /// Header in pdf file configuring
    /// </summary>
    /// <param name="entity">Configuring entity</param>
    protected abstract Paragraph HeaderConfiguring(T entity);
    /// <summary>
    /// Table in pdf file configuring 
    /// </summary>
    /// <param name="entity">Configuring entity</param>
    protected abstract Table TableConfiguring(T entity);
    /// <summary>
    /// Help method for making new table cell
    /// </summary>
    /// <param name="cellText">Text in cell to configure</param>
    protected virtual Cell DefaultCell(string cellText) => new Cell(1, 1).Add(new Paragraph(cellText));
}