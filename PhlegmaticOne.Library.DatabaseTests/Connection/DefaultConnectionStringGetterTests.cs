using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PhlegmaticOne.Library.Database.Connection.Tests;

[TestClass()]
public class DefaultConnectionStringGetterTests
{
    [TestMethod()]
    public void GetNamedConnectionStringTest()
    {
        var serverName = @"(localdb)\MSSQLLocalDB";
        var dataBaseName = @"LibraryDataBase";
        var getter = new DefaultConnectionStringGetter(serverName, dataBaseName);
        var connectionString = getter.GetConnectionString();
        Assert.IsNotNull(connectionString);
    }
}