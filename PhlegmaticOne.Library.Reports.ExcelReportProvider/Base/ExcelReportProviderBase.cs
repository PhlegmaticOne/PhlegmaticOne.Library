using PhlegmaticOne.Library.Reports.Base;
using Excel = Microsoft.Office.Interop.Excel;

namespace PhlegmaticOne.Library.Reports.ExcelReportProvider;
/// <summary>
/// Represents provider for building Excel reports from abonent lendings
/// </summary>
public abstract class ExcelReportProviderBase<T> : IReportProvider<T>
{
    private readonly string _directoryPath;
    private readonly string _fileName;
    protected Excel.Application Application;
    protected Excel.Worksheet ActiveWorksheet;
    /// <summary>
    /// Initializes new ExcelReportProviderBase instance
    /// </summary>
    /// <param name="directoryPath">Path to directory for future file</param>
    /// <param name="fileName">Future file name</param>
    protected ExcelReportProviderBase(string directoryPath, string fileName)
    {
        _directoryPath = directoryPath;
        _fileName = fileName;
    }
    public virtual async Task BuildReportAsync(T entity)
    {
        await Task.Run(() =>
        {
            try
            {
                Application = new Excel.Application
                {
                    Visible = false,
                    DisplayAlerts = false,
                    SheetsInNewWorkbook = 1
                };
                var workBook = Application.Workbooks.Add(Type.Missing);
                ActiveWorksheet = workBook.Worksheets.get_Item(1) as Excel.Worksheet;
                ActiveWorksheet.Activate();
                HeaderConfiguring(entity);
                ColumnNamesConfiguring(entity);
                DataFillConfiguring(entity);
                ActiveWorksheet.UsedRange.Columns.AutoFit();
                Application.Application.ActiveWorkbook
                    .SaveAs($"{_directoryPath}{_fileName}", Type.Missing, Type.Missing, Type.Missing,
                        Type.Missing, Type.Missing, Excel.XlSaveAsAccessMode.xlNoChange,
                        Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            }
            finally
            {
                Application.Quit();
            }
        });
    }
    protected abstract void HeaderConfiguring(T entity);
    protected abstract void ColumnNamesConfiguring(T entity);
    protected abstract void DataFillConfiguring(T entity);
}