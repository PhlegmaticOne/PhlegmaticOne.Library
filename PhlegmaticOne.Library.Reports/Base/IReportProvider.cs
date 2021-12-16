namespace PhlegmaticOne.Library.Reports.Base;

public interface IReportProvider<in T>
{
    Task BuildReport(T entity);
}