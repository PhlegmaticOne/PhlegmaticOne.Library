using PhlegmaticOne.Library.Reports.Base;

namespace PhlegmaticOne.Library.Reports.FileReportProvider;

public class FileReportProvider<T> : IReportProvider<T>
{
    private readonly IReportBuilder<T> _builder;
    private readonly FileInfo _fileInfo;
    public FileReportProvider(string directoryPath, string fileName, IReportBuilder<T> builder)
    {
        _builder = builder;
        _fileInfo = new DirectoryInfo(directoryPath).GetFiles().First(f => f.Name.Contains(fileName));
    }
    public async Task SaveAsync(T entity) => await File.WriteAllTextAsync(_fileInfo.FullName, _builder.Build(entity));
}