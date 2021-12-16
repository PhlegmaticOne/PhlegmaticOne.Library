using PhlegmaticOne.Library.Reports.Base;

namespace PhlegmaticOne.Library.Reports.FileReportProvider;

public class FileReportProvider<T> : IReportProvider<T>
{
    private readonly IReportBuilder<T> _builder;
    private readonly string _fullPath;
    public FileReportProvider(string directoryPath, string fileName, IReportBuilder<T> builder)
    {
        _builder = builder;
        _fullPath = Path.Combine(directoryPath, fileName);
    }
    public async Task BuildReport(T entity) => await File.WriteAllTextAsync(_fullPath, _builder.Build(entity));
}