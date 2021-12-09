using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhlegmaticOne.Library.Database.Connection;
using PhlegmaticOne.Library.Database.DB;
using PhlegmaticOne.Library.Database.Factory;
using PhlegmaticOne.Library.Database.SqlCommandBuilders;
using PhlegmaticOne.Library.Domain.Models;

namespace PhlegmaticOne.Library.DatabaseTests.DB;

[TestClass()]
public class AdoDataServiceTests
{
    [TestMethod()]
    public async Task AddAsyncTest()
    {
        var serverName = @"(localdb)\MSSQLLocalDB";
        var dataBaseName = @"LibraryDataBase";
        var getter = new DefaultConnectionStringGetter(serverName, dataBaseName);
        var commandExpressionBuilder = new SqlCommandExpressionBuilder();
        var db = AdoDataServiceFactory.GetInstance(getter, commandExpressionBuilder);
        var gender = new Gender
        {
            Name = "Male",
        };
        var abonent = new Abonent
        {
            Gender = gender,
            BirthDate = DateTime.Parse("24.02.2003"),
            Name = "Alexander",
            Surname = "Krotov",
            Patronymic = "Vyacheslavovich",
        };
        var added = await db.AddAsync(abonent);
        Assert.IsTrue(added);
    }
    [TestMethod()]
    public async Task GetIdOfExistingTest()
    {
        var serverName = @"(localdb)\MSSQLLocalDB";
        var dataBaseName = @"LibraryDataBase";
        var getter = new DefaultConnectionStringGetter(serverName, dataBaseName);
        var commandExpressionBuilder = new SqlCommandExpressionBuilder();
        var db = AdoDataServiceFactory.GetInstance(getter, commandExpressionBuilder);
        var gender = new Gender
        {
            Name = "Male",
        };
        var id = await db.GetIdOfExisting(gender);
        Assert.AreEqual(1, id);
    }

    [TestMethod()]
    public void DeleteAsyncTest()
    {
        Assert.Fail();
    }

    [TestMethod()]
    public void UpdateAsyncTest()
    {
        Assert.Fail();
    }

    [TestMethod()]
    public void GetLazyAsyncTest()
    {
        Assert.Fail();
    }

    [TestMethod()]
    public void GetFullAsyncTest()
    {
        Assert.Fail();
    }
}