using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhlegmaticOne.Library.Database.Connection;
using PhlegmaticOne.Library.Database.Repository;
using PhlegmaticOne.Library.Domain.Models;
using System.Threading.Tasks;

namespace PhlegmaticOne.Library.DatabaseTests.Repository;

[TestClass()]
public class SqlRepositoryTests
{
    [TestMethod()]
    public async Task ReadAllTest()
    {
        var entities = await new SqlRepository(DefaultConnectionStringGetter.LibraryConnectionStringGetter).ReadAll<Lending>();
        Assert.IsNotNull(entities);
    }
}