namespace PhlegmaticOne.Library.Reports.FileReportProvider;
/// <summary>
/// Represents contract for building pattern to save it in file from entity
/// </summary>
public interface IReportBuilder<in T>
{
    /// <summary>
    /// Builds pattern to save it in file from entity
    /// </summary>
    string Build(T entity);
}