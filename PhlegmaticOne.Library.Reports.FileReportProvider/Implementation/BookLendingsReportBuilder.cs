﻿using System.Text;
using PhlegmaticOne.Library.Domain.Models;

namespace PhlegmaticOne.Library.Reports.FileReportProvider.Implementation;

public class BookLendingsReportBuilder : IReportBuilder<IDictionary<Book, int>>
{
    public string Build(IDictionary<Book, int> entity)
    {
        var sb = new StringBuilder();
        foreach (var info in entity)
        {
            sb.AppendLine($"{info.Key} was lended {info.Key} times");
        }
        return sb.ToString();
    }
}