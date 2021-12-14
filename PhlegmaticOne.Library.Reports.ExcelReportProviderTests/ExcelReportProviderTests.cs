using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhlegmaticOne.Library.Database.Connection;
using PhlegmaticOne.Library.Database.Repository;
using PhlegmaticOne.Library.Database.Services;
using PhlegmaticOne.Library.Reports.ExcelReportProvider;

namespace PhlegmaticOne.Library.Reports.ReportProvidersTests;

[TestClass()]
public class ExcelReportProviderTests
{
    [TestMethod()]
    public async Task SaveBookLendingsAsyncTest()
    {
        var bookLendings =
            await new LibraryService(new SqlRepository(DefaultConnectionStringGetter.LibraryConnectionStringGetter))
                .GetBookLendingsAsync();
        var excelProvider = new BookLendingsReportProvider(AppDomain.CurrentDomain.BaseDirectory, "book_lendings");
        await excelProvider.SaveAsync(bookLendings);
    }
    [TestMethod()]
    public async Task SaveAbonentLendingsAsyncTest()
    {
        var start = DateTime.Parse("01.01.2001");
        var end = DateTime.Parse("31.12.2021");
        var abonentLendings =
            await new LibraryService(new SqlRepository(DefaultConnectionStringGetter.LibraryConnectionStringGetter))
                .GetAbonentLendingsAsync(start, end);
        var excelProvider = new AbonentLendingsReportProvider(AppDomain.CurrentDomain.BaseDirectory, "abonent_lendings", start, end);
        await excelProvider.SaveAsync(abonentLendings);
    }
}