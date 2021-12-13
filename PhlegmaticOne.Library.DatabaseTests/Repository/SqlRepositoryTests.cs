using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhlegmaticOne.Library.Database.Connection;
using PhlegmaticOne.Library.Database.Factory;
using PhlegmaticOne.Library.Database.Repository;
using PhlegmaticOne.Library.Domain.Models;

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