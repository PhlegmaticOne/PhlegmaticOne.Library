using PhlegmaticOne.Library.Domain.Models;

namespace PhlegmaticOne.Library.Database.Services;

public interface ILibraryService
{
    public Task<IDictionary<Book, int>> GetBookLendingsAsync();
    public Task<IDictionary<Abonent, IEnumerable<IGrouping<Genre, Book>>>> GetAbonentLendingsAsync(DateTime start, DateTime finish);
    public Task<Author> GetMostPopularAuthorAsync();
    public Task<Abonent> GetMostReadingAbonentAsync();
    public Task<Genre> GetMostPopularGenreAsync();
    public Task<IEnumerable<Book>> GetRepairRequiredBooksAsync();
}