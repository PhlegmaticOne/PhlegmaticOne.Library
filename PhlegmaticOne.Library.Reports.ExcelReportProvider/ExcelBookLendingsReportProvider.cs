using PhlegmaticOne.Library.Domain.Models;
using Excel = Microsoft.Office.Interop.Excel;
namespace PhlegmaticOne.Library.Reports.ExcelReportProvider;
/// <summary>
/// Represents excel report provider for book lendings 
/// </summary>
public class ExcelBookLendingsReportProvider : ExcelReportProviderBase<IDictionary<Book, int>>
{
    /// <summary>
    /// Initializes new ExcelBookLendingsReportProvider instance
    /// </summary>
    /// <param name="directoryPath">Path to directory for future file</param>
    /// <param name="fileName">Future file name</param>
    public ExcelBookLendingsReportProvider(string directoryPath, string fileName) : base(directoryPath, fileName) { }
    protected override void HeaderConfiguring(IDictionary<Book, int> entity)
    {
        var header = ActiveWorksheet.Range[ActiveWorksheet.Cells[1, 1], ActiveWorksheet.Cells[1, 2]];
        header.Merge();
        header.Cells[1, 1] = "Book lendings";
        header.HorizontalAlignment = Excel.Constants.xlCenter;
        header.Font.Bold = true;
    }
    protected override void ColumnNamesConfiguring(IDictionary<Book, int> entity)
    {
        ActiveWorksheet.Cells[2, 1] = "Book's";
        ActiveWorksheet.Cells[2, 2] = "Lending times";
    }
    protected override void DataFillConfiguring(IDictionary<Book, int> entity)
    {
        for (int i = 3; i < entity.Count + 3; i++)
        {
            var currentInfo = entity.ElementAt(i - 3);
            ActiveWorksheet.Cells[i, 1] = currentInfo.Key.ToString();
            ActiveWorksheet.Cells[i, 2] = currentInfo.Value;
        }
    }
}