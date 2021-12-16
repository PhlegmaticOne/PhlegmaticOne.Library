using PhlegmaticOne.Library.Reports.Base;

namespace PhlegmaticOne.Library.Reports.FileReportProvider;
/// <summary>
/// Represents instance for making report in text format
/// </summary>
public class FileReportProvider<T> : IReportProvider<T>
{
    private readonly IReportBuilder<T> _builder;
    private readonly string _fullPath;
    /// <summary>
    /// Initializes new FileReportProvider instance
    /// </summary>
    /// <param name="directoryPath">Path to directory for future file</param>
    /// <param name="fileName">Future file name</param>
    /// <param name="builder">Builder for entity to get pattern of it to save</param>
    public FileReportProvider(string directoryPath, string fileName, IReportBuilder<T> builder)
    {
        _builder = builder;
        _fullPath = Path.Combine(directoryPath, fileName);
    }
    public async Task BuildReportAsync(T entity) => await File.WriteAllTextAsync(_fullPath, _builder.Build(entity));
}