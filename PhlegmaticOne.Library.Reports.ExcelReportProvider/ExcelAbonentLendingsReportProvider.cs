using PhlegmaticOne.Library.Domain.Models;
using Excel = Microsoft.Office.Interop.Excel;

namespace PhlegmaticOne.Library.Reports.ExcelReportProvider;

/// <summary>
/// Represents excel report provider for abonent lendings 
/// </summary>
public class ExcelAbonentLendingsReportProvider : ExcelReportProviderBase<IDictionary<Abonent, IEnumerable<IGrouping<Genre, Book>>>>
{
    private readonly DateTime _start;
    private readonly DateTime _finish;

    protected ExcelAbonentLendingsReportProvider(string directoryPath, string fileName) :
        base(directoryPath, fileName)
    { }
    /// <summary>
    /// Initializes new ExcelAbonentLendingsReportProvider instance
    /// </summary>
    /// <param name="directoryPath">Path to directory for future file</param>
    /// <param name="fileName">Future file name</param>
    /// <param name="start">Start date of searched results</param>
    /// <param name="finish">End date of searched results</param>
    public ExcelAbonentLendingsReportProvider(string directoryPath, string fileName, DateTime start, DateTime finish) :
        this(directoryPath, fileName)
    {
        _start = start;
        _finish = finish;
    }

    protected override void HeaderConfiguring(IDictionary<Abonent, IEnumerable<IGrouping<Genre, Book>>> entity)
    {
        var header = ActiveWorksheet.Range[ActiveWorksheet.Cells[1, 1], ActiveWorksheet.Cells[1, 3]];
        header.Merge();
        header.Cells[1, 1] = $"Abonent lendings from {_start:dd-MM-yy} to {_finish:dd-MM-yy}";
        header.HorizontalAlignment = Excel.Constants.xlCenter;
        header.Font.Bold = true;
    }
    protected override void ColumnNamesConfiguring(IDictionary<Abonent, IEnumerable<IGrouping<Genre, Book>>> entity)
    {
        ActiveWorksheet.Cells[2, 1] = "Abonent";
        ActiveWorksheet.Cells[2, 2] = "Genre";
        ActiveWorksheet.Cells[2, 3] = "Books";
    }

    protected override void DataFillConfiguring(IDictionary<Abonent, IEnumerable<IGrouping<Genre, Book>>> entity)
    {
        var currentRow = 3;
        for (int i = 3; i < entity.Count + 3; i++)
        {
            var abonentLending = entity.ElementAt(i - 3);
            ActiveWorksheet.Cells[currentRow, 1] = abonentLending.Key.ToString();
            foreach (var grouping in abonentLending.Value)
            {
                var currentColumn = 2;
                ActiveWorksheet.Cells[currentRow, currentColumn] = grouping.Key.ToString();
                foreach (var book in grouping)
                {
                    ActiveWorksheet.Cells[currentRow, ++currentColumn] = book.ToString();
                }
                ++currentRow;
            }
        }
    }
}