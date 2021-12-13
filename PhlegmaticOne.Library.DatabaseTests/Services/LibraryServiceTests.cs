using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhlegmaticOne.Library.Database.Connection;
using PhlegmaticOne.Library.Database.Repository;
using PhlegmaticOne.Library.Database.Services;

namespace PhlegmaticOne.Library.DatabaseTests.Services;

[TestClass()]
public class LibraryServiceTests
{
    [TestMethod()]
    public async Task GetBookLendingsAsyncTest()
    {
        var bookLendings =
            await new LibraryService(new SqlRepository(DefaultConnectionStringGetter.LibraryConnectionStringGetter))
                .GetBookLendingsAsync();
        Assert.IsNotNull(bookLendings);
    }

    [TestMethod()]
    public async Task GetAbonentLendingsAsyncTest()
    {
        var abonentLendings =
            await new LibraryService(new SqlRepository(DefaultConnectionStringGetter.LibraryConnectionStringGetter))
                .GetAbonentLendingsAsync(DateTime.Parse("01.01.2001"), DateTime.Parse("01.01.2022"));
        Assert.IsNotNull(abonentLendings);
    }

    [TestMethod()]
    public async Task GetMostPopularAuthorAsyncTest()
    {
        var mostPopularAuthor =
            await new LibraryService(new SqlRepository(DefaultConnectionStringGetter.LibraryConnectionStringGetter))
                .GetMostPopularAuthorAsync();
        Assert.IsNotNull(mostPopularAuthor);
    }

    [TestMethod()]
    public async Task GetMostReadingAbonentAsyncTest()
    {
        var mostReadingAbonent =
            await new LibraryService(new SqlRepository(DefaultConnectionStringGetter.LibraryConnectionStringGetter))
                .GetMostReadingAbonentAsync();
        Assert.IsNotNull(mostReadingAbonent);
    }

    [TestMethod()]
    public async Task GetMostPopularGenreAsyncTest()
    {
        var genre = await new LibraryService(new SqlRepository(DefaultConnectionStringGetter.LibraryConnectionStringGetter))
                              .GetMostPopularGenreAsync();
        Assert.IsNotNull(genre);
    }
}