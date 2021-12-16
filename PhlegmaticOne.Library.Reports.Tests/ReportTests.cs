using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhlegmaticOne.Library.Database.Connection;
using PhlegmaticOne.Library.Database.Repository;
using PhlegmaticOne.Library.Database.Services;
using PhlegmaticOne.Library.Domain.Models;
using PhlegmaticOne.Library.Reports.ExcelReportProvider;
using PhlegmaticOne.Library.Reports.FileReportProvider;
using PhlegmaticOne.Library.Reports.FileReportProvider.Implementation;
using PhlegmaticOne.Library.Reports.PdfReportProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhlegmaticOne.Library.Reports.Tests;

[TestClass]
public class ReportTests
{
    private static readonly string _directoryPath = AppDomain.CurrentDomain.BaseDirectory;
    [TestMethod]
    public async Task FileReport_BookLendings_Test()
    {
        var repository = new SqlRepository(DefaultConnectionStringGetter.LibraryConnectionStringGetter);
        var libraryService = new LibraryService(repository);
        var entities = await libraryService.GetBookLendingsAsync();
        var reportProvider =
            new FileReportProvider<IDictionary<Book, int>>
                (_directoryPath, "book_lendings.txt", new BookLendingsReportBuilder());
        await reportProvider.BuildReportAsync(entities);
    }
    [TestMethod]
    public async Task FileReport_AbonentLendings_Test()
    {
        var start = DateTime.Parse("01.01.2001");
        var finish = DateTime.Parse("01.01.2022");
        var repository = new SqlRepository(DefaultConnectionStringGetter.LibraryConnectionStringGetter);
        var libraryService = new LibraryService(repository);
        var entities = await libraryService.GetAbonentLendingsAsync(start, finish);
        var reportProvider =
            new FileReportProvider<IDictionary<Abonent, IEnumerable<IGrouping<Genre, Book>>>>
                (_directoryPath, "abonent_lendings.txt", new AbonentLendingsReportBuilder());
        await reportProvider.BuildReportAsync(entities);
    }
    [TestMethod]
    public async Task ExcelReport_BookLendings_Test()
    {
        var repository = new SqlRepository(DefaultConnectionStringGetter.LibraryConnectionStringGetter);
        var libraryService = new LibraryService(repository);
        var entities = await libraryService.GetBookLendingsAsync();
        var reportProvider =
            new ExcelBookLendingsReportProvider(_directoryPath, "book_lendings.xlsx");
        await reportProvider.BuildReportAsync(entities);
    }
    [TestMethod]
    public async Task ExcelReport_AbonentLendings_Test()
    {
        var start = DateTime.Parse("01.01.2001");
        var finish = DateTime.Parse("01.01.2022");
        var repository = new SqlRepository(DefaultConnectionStringGetter.LibraryConnectionStringGetter);
        var libraryService = new LibraryService(repository);
        var entities = await libraryService.GetAbonentLendingsAsync(start, finish);
        var reportProvider =
            new ExcelAbonentLendingsReportProvider(_directoryPath, "abonent_lendings.xlsx", start, finish);
        await reportProvider.BuildReportAsync(entities);
    }
    [TestMethod]
    public async Task PdfReport_BookLendings_Test()
    {
        var repository = new SqlRepository(DefaultConnectionStringGetter.LibraryConnectionStringGetter);
        var libraryService = new LibraryService(repository);
        var entities = await libraryService.GetBookLendingsAsync();
        var reportProvider =
            new PdfBookLendingsReportProvider(_directoryPath, "book_lendings.pdf");
        await reportProvider.BuildReportAsync(entities);
    }
    [TestMethod]
    public async Task PdfReport_AbonentLendings_Test()
    {
        var start = DateTime.Parse("01.01.2001");
        var finish = DateTime.Parse("01.01.2022");
        var repository = new SqlRepository(DefaultConnectionStringGetter.LibraryConnectionStringGetter);
        var libraryService = new LibraryService(repository);
        var entities = await libraryService.GetAbonentLendingsAsync(start, finish);
        var reportProvider =
            new PdfAbonentLendingsReportProvider(_directoryPath, "abonent_lendings.pdf", start, finish);
        await reportProvider.BuildReportAsync(entities);
    }

}