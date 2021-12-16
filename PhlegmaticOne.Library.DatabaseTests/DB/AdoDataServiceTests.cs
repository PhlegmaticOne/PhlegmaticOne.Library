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
        new() { Name = "Novel" },
        new() { Name = "Play" },
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
        },
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
        new()
        {
            Name = "Martin Eden",
            Genre = _genres.First(g => g.Name == "Romance"),
            Authors = new List<Author>() { _authors.First(a => a.Name == "Jack") },
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
    public async Task DeleteAsyncTest()
    {
        await using var context = await AdoDataServiceFactory.DefaultInstanceAsync(_getter);
        var deletedCount = await context.DeleteAsync<Book>(1011);
        Assert.AreEqual(1, deletedCount);
    }

    [TestMethod()]
    public async Task UpdateAsyncTest()
    {
        await using var context = await AdoDataServiceFactory.DefaultInstanceAsync(_getter);
        var book = await context.GetFullAsync<Book>(2008);
        var author = await context.GetLazyAsync<Author>(3017);
        book.Authors.Add(author);
        await context.UpdateAsync(book.Id, book);
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
        var author = await context.GetFullAsync<Author>(3018);
        Assert.IsNotNull(author);
    }
    [TestMethod()]
    public async Task AddAdditionalAsyncTest()
    {
        await using var context = await AdoDataServiceFactory.DefaultInstanceAsync(_getter);
        var newBook = new Book
        {
            Name = "The Cherry Orchard",
            Genre = _genres.Last()
        };
        var newAuthor = new Author()
        {
            Name = "Anton",
            Surname = "Chekhov",
            Books = new List<Book>() { newBook }
        };
        var newLending = new Lending()
        {
            Abonent = _abonents[3],
            Book = newBook,
            LendingDate = DateTime.Parse("10.10.2021"),
            IsReturned = true,
            State = _states[3],
            ReturnDate = DateTime.Parse("20.10.2021")
        };
        await context.AddAsync(newBook);
        await context.AddAsync(newAuthor);
        await context.AddAsync(newLending);
    }
}