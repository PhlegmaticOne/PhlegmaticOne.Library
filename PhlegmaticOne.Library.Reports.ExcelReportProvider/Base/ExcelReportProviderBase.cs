using PhlegmaticOne.Library.Reports.Base;
using Excel = Microsoft.Office.Interop.Excel;

namespace PhlegmaticOne.Library.Reports.ExcelReportProvider;

public abstract class ExcelReportProviderBase<T> : IReportProvider<T>
{
    private readonly string _directoryPath;
    private readonly string _fileName;
    protected Excel.Application Application;
    protected Excel.Worksheet ActiveWorksheet;

    protected ExcelReportProviderBase(string directoryPath, string fileName)
    {
        _directoryPath = directoryPath;
        _fileName = fileName;
    }
    public virtual async Task SaveAsync(T entity)
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
                    .SaveAs($"{_directoryPath}{_fileName}.xlsx", Type.Missing, Type.Missing, Type.Missing,
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