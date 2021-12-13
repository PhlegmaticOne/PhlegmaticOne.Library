﻿using PhlegmaticOne.Library.Domain.Models;

namespace PhlegmaticOne.Library.Database.Services;

public interface ILibraryService
{
    public Task<IDictionary<Book, int>> GetBookLendingsAsync();
    public Task<IDictionary<Abonent, IGrouping<Genre, Book>>> GetAbonentLendingsAsync();
    public Task<Author> GetMostPopularAuthorAsync();
    public Task<Abonent> GetMostReadingAbonentAsync();
    public Task<Genre> GetMostPopularGenreAsync();
}