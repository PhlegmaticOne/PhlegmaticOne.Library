namespace PhlegmaticOne.Library.Reports.FileReportProvider;

public interface IReportBuilder<in T>
{
    string Build(T entity);
}