﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhlegmaticOne.Library.Database.Connection;
using PhlegmaticOne.Library.Database.DB;
using PhlegmaticOne.Library.Database.Factory;
using PhlegmaticOne.Library.Database.Relationships;
using PhlegmaticOne.Library.Database.SqlCommandBuilders;
using PhlegmaticOne.Library.Domain.Models;

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
    private static readonly List<Genre> _genres = new()
    {
        new() { Name = "Detective" },
        new() { Name = "Thriller" },
        new() { Name = "Adventure" },
        new() { Name = "Romance" },
        new() { Name = "Comedy" },
        new() { Name = "Realism" },
        new() { Name = "Fantasy" },
        new() { Name = "Folklore" },
        new() { Name = "Religion" },
        new() { Name = "Novel" }
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
        }
    };

    private static readonly List<Book> _books = new()
    {
        new()
        {
            Name = "War and Peace",
            Genre = _genres.First(r => r.Name == "Romance")
        },
        new()
        {
            Name = "Captain's daughter",
            Genre = _genres.First(r => r.Name == "Romance")
        },
        new()
        {
            Name = "Hero of our time",
            Genre = _genres.First(r => r.Name == "Romance")
        },
        new()
        {
            Name = "Crime and Punishment",
            Genre = _genres.First(r => r.Name == "Thriller")
        },
        new()
        {
            Name = "The Sea Wolf",
            Genre = _genres.First(r => r.Name == "Adventure")
        },
    };

    private static readonly List<Lending> _lendings = new()
    {
        new()
        {
            Abonent = _abonents[0],
            Book = _books[0],
            LendingDate = DateTime.Parse("11.11.2021")
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

    [TestMethod()]
    public async Task EnsureDeletedAsyncTest()
    {
        var serverName = @"(localdb)\MSSQLLocalDB";
        var dataBaseName = @"LibraryDataBase";
        var getter = new DefaultConnectionStringGetter(serverName, dataBaseName);
        await using var context = await AdoDataServiceFactory.DefaultInstanceAsync(getter);
        await context.EnsureDeletedAsync();
    }

    [TestMethod()]
    public async Task AddAsyncTest()
    {
        var book = _books[1];
        book.Authors.Add(_authors.First());
        var serverName = @"(localdb)\MSSQLLocalDB";
        var dataBaseName = @"LibraryDataBase";
        var getter = new DefaultConnectionStringGetter(serverName, dataBaseName);
        await using var context = await AdoDataServiceFactory.DefaultInstanceAsync(getter);
        await context.AddAsync(_authors.First());
        await context.AddAsync(book.Genre);
        await context.AddAsync(book);
    }
    [TestMethod()]
    public async Task GetIdOfExistingTest()
    {
        var serverName = @"(localdb)\MSSQLLocalDB";
        var dataBaseName = @"LibraryDataBase";
        var getter = new DefaultConnectionStringGetter(serverName, dataBaseName);
        await using var context = await AdoDataServiceFactory.DefaultInstanceAsync(getter);
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
        var serverName = @"(localdb)\MSSQLLocalDB";
        var dataBaseName = @"LibraryDataBase";
        var getter = new DefaultConnectionStringGetter(serverName, dataBaseName);
        await using var context = await AdoDataServiceFactory.DefaultInstanceAsync(getter);
        await context.AddAsync(_genders.First());
    }

    [TestMethod()]
    public async Task GetLazyAsyncTest()
    {
        var serverName = @"(localdb)\MSSQLLocalDB";
        var dataBaseName = @"LibraryDataBase";
        var getter = new DefaultConnectionStringGetter(serverName, dataBaseName);
        var db = AdoDataServiceFactory.DefaultInstanceAsync(getter);
    }

    [TestMethod()]
    public void GetFullAsyncTest()
    {
        Assert.IsTrue(typeof(ICollection<Author>).IsAssignableTo(typeof(IEnumerable<DomainModelBase>)));
    }
}