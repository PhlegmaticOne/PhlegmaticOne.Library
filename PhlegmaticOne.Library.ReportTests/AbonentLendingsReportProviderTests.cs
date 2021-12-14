using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhlegmaticOne.Library.Database.Connection;
using PhlegmaticOne.Library.Database.Repository;
using PhlegmaticOne.Library.Database.Services;
using PhlegmaticOne.Library.Reports.PdfReportProvider;

namespace PhlegmaticOne.Library.ReportTests;

[TestClass()]
public class AbonentLendingsReportProviderTests
{
    [TestMethod()]
    public async Task PdfAbonentLendingsReportProviderTest()
    {
        var start = DateTime.Parse("01.01.2003");
        var end = DateTime.Parse("31.12.2021");
        var abonentLendings =
            await new LibraryService(new SqlRepository(DefaultConnectionStringGetter.LibraryConnectionStringGetter))
                .GetAbonentLendingsAsync(start, end);
        var reportProvider =
            new PdfAbonentLendingsReportProvider(AppDomain.CurrentDomain.BaseDirectory, "abonent_lendings.pdf", start, end);
        await reportProvider.SaveAsync(abonentLendings);
    }
    [TestMethod()]
    public async Task PdfBookLendingsReportProviderTest()
    {
        var bookLendings =
            await new LibraryService(new SqlRepository(DefaultConnectionStringGetter.LibraryConnectionStringGetter))
                .GetBookLendingsAsync();
        var reportProvider =
            new PdfBookLendingsPdfReportProvider(AppDomain.CurrentDomain.BaseDirectory, "book_lendings.pdf");
        await reportProvider.SaveAsync(bookLendings);
    }
}