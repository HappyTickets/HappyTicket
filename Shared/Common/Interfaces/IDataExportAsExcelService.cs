using Microsoft.Extensions.Localization;
using Shared.Common.Enums;

namespace Shared.Common.Interfaces
{
    public interface IDataExportAsExcelService
    {
        string[] TargetProperties { get; set; }

        Dictionary<string, LanguageTextDirection> LanguagesWithDirections { get; }

        /// <summary>
        /// A method that takes enumerable data from a data source, generates an excel file based on the data, and finally returns the file as a stream of bytes and its name.
        /// </summary>
        Task<(string, byte[])> GenerateExcelFileAsync<ExcelModelType, LocalizerModelType>(IEnumerable<ExcelModelType> data,
                                                                                          string currentLanguageCode,
                                                                                          string worksheetName,
                                                                                          IStringLocalizer<LocalizerModelType> stringLocalizer,
                                                                                          string[]? excludedProperties = null);
    }
}
