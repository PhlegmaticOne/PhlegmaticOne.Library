using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhlegmaticOne.Library.Database.Connection;
using PhlegmaticOne.Library.Database.DB;
using PhlegmaticOne.Library.Database.Factory;
using PhlegmaticOne.Library.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhlegmaticOne.Library.DatabaseTests.DB;


[TestClass()]
public class AdoDataServiceTests
{
    private static readonly List<Gender> _genders = new()
    {
        new() { Name = "Male" },
        new() { Name = "Female" },
        new() { Name = "Other" }
    };

    private static readonly List<Genre> _genres = new()
    {
        new() { Name = "Romance" },
        new() { Name = "Detective" },
        new() { Name = "Thriller" },
        new() { Name = "Adventure" },
        new() { Name = "Comedy" },
        new() { Name = "Realism" },
        new() { Name = "Fantasy" },
        new() { Name = "Folklore" },
        new() { Name = "Religion" },
        new() { Name = "Novel" }
    };

    private static readonly List<State> _states = new()
    {
        new() { Name = "Excellent" },
        new() { Name = "Good" },
        new() { Name = "Satisfactorily" },
        new() { Name = "Bad" },
        new() { Name = "Terrible" }
    };

    private static readonly List<Abonent> _abonents = new()
    {
        new()
        {
            Name = "Semen",
            Surname = "Swallow",
            Patronymic = "Sergeevich",
            BirthDate = DateTime.Parse("24.02.2003"),
            Gender = _genders[0]
        },
        new()
        {
            Name = "Alisa",
            Surname = "Dvachevskaya",
            Patronymic = "Vasilevna",
            BirthDate = DateTime.Parse("31.01.2000"),
            Gender = _genders[1]
        },
        new()
        {
            Name = "Elena",
            Surname = "Wristova",
            Patronymic = "Andreevna",
            BirthDate = DateTime.Parse("01.01.2001"),
            Gender = _genders[1]
        },
        new()
        {
            Name = "Slavyana",
            Surname = "Leadova",
            Patronymic = "Alexeevna",
            BirthDate = DateTime.Parse("23.11.2000"),
            Gender = _genders[1]
        },
        new()
        {
            Name = "Yulya",
            Surname = "Catova",
            Patronymic = "Anatolievna",
            BirthDate = DateTime.Parse("11.08.2002"),
            Gender = _genders[1]
        }
    };


    private static readonly List<Author> _authors = new()
    {
        new()
        {
            Name = "Aleksandr",
            Surname = "Pushkin"
        },
        new()
        {
            Name = "Fedor",
            Surname = "Dostoevsky"
        },
        new()
        {
            Name = "Mikhail",
            Surname = "Lermontov"
        },
        new()
        {
            Name = "Lev",
            Surname = "Tolstoy"
        },
        new()
        {
            Name = "Jack",
            Surname = "London"
        },
        new()
        {
            Name = "Arcadiy",
            Surname = "Strugacky"
        },
        new()
        {
            Name = "Boris",
            Surname = "Strugacky"
        }
    };

    private static readonly List<Book> _books = new()
    {
        new()
        {
            Name = "Captain`s daughter",
            Genre = _genres.First(r => r.Name == "Romance"),
            Authors = new List<Author>() { _authors.First(a => a.Surname == "Pushkin") }
        },
        new()
        {
            Name = "War and Peace",
            Genre = _genres.First(r => r.Name == "Romance"),
            Authors = new List<Author>() { _authors.First(a => a.Surname == "Tolstoy") }
        },
        new()
        {
            Name = "Hero of our time",
            Genre = _genres.First(r => r.Name == "Romance"),
            Authors = new List<Author>() { _authors.First(a => a.Surname == "Lermontov") }
        },
        new()
        {
            Name = "Crime and Punishment",
            Genre = _genres.First(r => r.Name == "Thriller"),
            Authors = new List<Author>() { _authors.First(a => a.Surname == "Dostoevsky") }
        },
        new()
        {
            Name = "The Sea Wolf",
            Genre = _genres.First(r => r.Name == "Adventure"),
            Authors = new List<Author>() { _authors.First(a => a.Surname == "London") }
        },
        new()
        {
            Name = "Roadside Picnic",
            Genre = _genres.First(r => r.Name == "Fantasy"),
            Authors = _authors.Where(a => a.Surname == "Strugacky").ToList()
        },
    };

    private static readonly List<Lending> _lendings = new()
    {
        new()
        {
            Abonent = _abonents[0],
            Book = _books[0],
            LendingDate = DateTime.Parse("11.11.2021"),
            IsReturned = true,
            ReturnDate = DateTime.Parse("20.11.2021"),
            State = _states[0]
        },
        new()
        {
            Abonent = _abonents[1],
            Book = _books[1],
            LendingDate = DateTime.Parse("11.11.2021")
        },
        new()
        {
            Abonent = _abonents[2],
            Book = _books[2],
            LendingDate = DateTime.Parse("11.11.2021")
        },
        new()
        {
            Abonent = _abonents[3],
            Book = _books[3],
            LendingDate = DateTime.Parse("11.11.2021")
        },
    };

    private const string _serverName = @"(localdb)\MSSQLLocalDB";
    private const string _dbName = @"LibraryDataBase";
    private readonly IConnectionStringGetter _getter = new DefaultConnectionStringGetter(_serverName, _dbName);
    [TestMethod()]
    public async Task EnsureDeletedAsyncTest()
    {
        await using var context = await AdoDataServiceFactory.DefaultInstanceAsync(_getter);
        await context.EnsureDeletedAsync();
    }

    [TestMethod()]
    public async Task AddAsyncTest()
    {
        await using var context = await AdoDataServiceFactory.DefaultInstanceAsync(_getter);
        await AddAll(_genders, context);
        await AddAll(_genres, context);
        await AddAll(_states, context);
        await AddAll(_abonents, context);
        await AddAll(_authors, context);
        await AddAll(_books, context);
        await AddAll(_lendings, context);
    }

    private async Task AddAll<TEntity>(IEnumerable<TEntity> entities, AdoDataService service) where TEntity : DomainModelBase
    {
        foreach (var entity in entities)
        {
            await service.AddAsync(entity);
        }
    }
    [TestMethod()]
    public async Task GetIdOfExistingTest()
    {
        await using var context = await AdoDataServiceFactory.DefaultInstanceAsync(_getter);
        var sss = await context.AddAsync(_genders.First());
        var aaa = await context.AddAsync(_abonents.First());
    }

    [TestMethod()]
    public void DeleteAsyncTest()
    {
        var book = _books[0];
        var props =
            book.GetType().GetProperties().ToDictionary(key => key.Name, value => value.GetValue(book));

        Assert.IsNotNull(props);
    }

    [TestMethod()]
    public async Task UpdateAsyncTest()
    {
        var list = Convert.ChangeType(_authors, typeof(List<>).MakeGenericType(typeof(Author)));
    }

    [TestMethod()]
    public async Task GetLazyAsyncTest()
    {
        await using var context = await AdoDataServiceFactory.DefaultInstanceAsync(_getter);
        var abonent = await context.GetLazyAsync<Abonent>(2016);
        Assert.IsNotNull(abonent);
    }

    [TestMethod()]
    public async Task GetFullAsyncTest()
    {
        await using var context = await AdoDataServiceFactory.DefaultInstanceAsync(_getter);
        var book = await context.GetFullAsync<Book>(13);
        Assert.IsNotNull(book);
    }
}