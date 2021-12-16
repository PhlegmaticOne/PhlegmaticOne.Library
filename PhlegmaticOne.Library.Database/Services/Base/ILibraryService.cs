using PhlegmaticOne.Library.Domain.Models;

namespace PhlegmaticOne.Library.Database.Services;
/// <summary>
/// Library service for configuring different information
/// </summary>
public interface ILibraryService
{
    /// <summary>
    /// Get book lendings asynchronously
    /// </summary>
    public Task<IDictionary<Book, int>> GetBookLendingsAsync();
    /// <summary>
    /// Get abonent lendings asynchronously
    /// </summary>
    public Task<IDictionary<Abonent, IEnumerable<IGrouping<Genre, Book>>>>
        GetAbonentLendingsAsync(DateTime start, DateTime finish);
    /// <summary>
    /// Get most popular author asynchronously
    /// </summary>
    public Task<Author> GetMostPopularAuthorAsync();
    /// <summary>
    /// Get most reading abonent asynchronously
    /// </summary>
    /// <returns></returns>
    public Task<Abonent> GetMostReadingAbonentAsync();
    /// <summary>
    /// Get most popular genre asynchronously
    /// </summary>
    /// <returns></returns>
    public Task<Genre> GetMostPopularGenreAsync();
    /// <summary>
    /// Get repair required books asynchronously
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<Book>> GetRepairRequiredBooksAsync();
}