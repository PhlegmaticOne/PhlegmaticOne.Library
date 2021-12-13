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
    public void LibraryServiceTest()
    {
        Assert.Fail();
    }

    [TestMethod()]
    public void GetBookLendingsAsyncTest()
    {
        Assert.Fail();
    }

    [TestMethod()]
    public void GetAbonentLendingsAsyncTest()
    {
        Assert.Fail();
    }

    [TestMethod()]
    public void GetMostPopularAuthorAsyncTest()
    {
        Assert.Fail();
    }

    [TestMethod()]
    public void GetMostReadingAbonentAsyncTest()
    {
        Assert.Fail();
    }

    [TestMethod()]
    public async Task GetMostPopularGenreAsyncTest()
    {
        var genre = await new LibraryService(new SqlRepository(DefaultConnectionStringGetter.LibraryConnectionStringGetter))
                              .GetMostPopularGenreAsync();
        Assert.IsNotNull(genre);
    }
}