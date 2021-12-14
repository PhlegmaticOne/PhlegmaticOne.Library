using PhlegmaticOne.Library.Database.Repository;
using PhlegmaticOne.Library.Domain.Models;

namespace PhlegmaticOne.Library.Database.Services;

public class LibraryService : ILibraryService
{
    private readonly IRepository _repository;
    public LibraryService(IRepository repository) => _repository = repository;
    public async Task<IDictionary<Book, int>> GetBookLendingsAsync()
    {
        var entities = await _repository.ReadAll<Lending>();
        return entities.GroupBy(x => x.Book).ToDictionary(key => key.Key, v => v.Count());
    }
    public async Task<IDictionary<Abonent, IEnumerable<IGrouping<Genre, Book>>>> GetAbonentLendingsAsync(DateTime start, DateTime finish)
    {
        var entities = await _repository.ReadAll<Lending>();
        return entities.GroupBy(x => x.Abonent).ToDictionary(k => k.Key,
                                v => v.Select(x => x.Book).GroupBy(x => x.Genre));
    }
    public async Task<Author> GetMostPopularAuthorAsync()
    {
        var entities = await _repository.ReadAll<Lending>();
        return entities.SelectMany(x => x.Book.Authors).GroupBy(x => x).MaxBy(x => x.Count()).Key;
    }
    public async Task<Abonent> GetMostReadingAbonentAsync()
    {
        var entities = await _repository.ReadAll<Lending>();
        return entities.GroupBy(x => x.Abonent).MaxBy(x => x.Count()).Key;
    }
    public async Task<Genre> GetMostPopularGenreAsync()
    {
        var entities = await _repository.ReadAll<Lending>();
        return entities.GroupBy(x => x.Book.Genre).MaxBy(x => x.Count()).Key;
    }

    public async Task<IEnumerable<Book>> GetRepairRequiredBooksAsync()
    {
        var entities = await _repository.ReadAll<Lending>();
        return entities.Where(p => p.IsReturned && p.State.IsRepairNeeded()).Select(e => e.Book);
    }
}