namespace PhlegmaticOne.Library.Reports.Base;
/// <summary>
/// Represents contract for making reports from different entities
/// </summary>
public interface IReportProvider<in T>
{
    /// <summary>
    /// Builds report asynchronously
    /// </summary>
    /// <param name="entity">Entity for report building</param>
    Task BuildReportAsync(T entity);
}