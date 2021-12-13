using PhlegmaticOne.Library.Database.Repository;
using PhlegmaticOne.Library.Domain.Models;

namespace PhlegmaticOne.Library.Database.Services;

public class LibraryService : ILibraryService
{
    private readonly IRepository _repository;

    public LibraryService(IRepository repository) => _repository = repository;

    public Task<IDictionary<Book, int>> GetBookLendingsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<IDictionary<Abonent, IGrouping<Genre, Book>>> GetAbonentLendingsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Author> GetMostPopularAuthorAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Abonent> GetMostReadingAbonentAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<Genre> GetMostPopularGenreAsync()
    {
        var entities = await _repository.ReadAll<Lending>();
        var genres = entities.Select(e => e.Book).GroupBy(e => e.Genre);
        return new Genre();
    }
}